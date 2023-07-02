#include "JavaScriptWorker.h"
#include "JavaScriptSystem.h"
#include "JavaScriptOperations.h"
#include "JavaScriptAsset.h"
#include "../Core/WorkQueue.h"
#include "../Core/Context.h"
#include "../IO/Log.h"
#include "../Core/Mutex.h"
#include "../Resource/ResourceCache.h"
#include "../Core/Thread.h"
#include <EASTL/atomic.h>
#include <future>

#define JS_WORKERS_TABLE_PROP "__workers"
#define JS_WORKER_THREAD_PROP DUK_HIDDEN_SYMBOL("thread")
#define JS_WORKER_DATA_PROP DUK_HIDDEN_SYMBOL("data")
#define JS_WORKER_FUNCTION_PROP "__workerFn"

namespace Urho3D
{
    struct WorkerData
    {
        duk_context* ctx_;
        unsigned refs_;
        unsigned thread_idx_;
        void* heapptr_;
    };

    ea::unordered_map<void*, WorkerData*> g_thread_data_;
    Mutex g_thread_sync_;

    bool js_worker_lock(void* data);
    void js_worker_unlock();
    void js_worker_data_release(WorkerData* data);
    void js_worker_emit_error_evt(const char* msg, void* heapptr);
    void js_worker_post_message(duk_context* ctx, WorkerData* worker_data, const char* evtName, Variant value);
    duk_idx_t js_worker_post_message_call(duk_context* ctx);
    void js_worker_store_data_at_obj(duk_context* ctx, duk_idx_t obj_idx, WorkerData* data);

    void js_worker_fatal_error(void* udata, const char* msg)
    {
        ea::string errMsg(msg == nullptr ? "no message" : msg);
        URHO3D_LOGERROR("**FATAL ERROR(Worker)** {}", errMsg);
        URHO3D_ASSERTLOG(udata, "userdata is required to free worker on fatal error");

        WorkerData* data = static_cast<WorkerData*>(udata);
        if (js_worker_lock(data)) {
            data->ctx_ = nullptr; /*fatal error automatically frees context*/

            js_worker_emit_error_evt(msg, data->heapptr_);
            js_worker_data_release(data);
            js_worker_unlock();
        }
    }

    /// create worker context. duk_context* result will be stored at WorkerData struct
    void js_create_worker_heap(WorkerData* data)
    {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::CreateThreadContext(js_worker_fatal_error, data));
        data->ctx_ = ctx;

        // add thread object to rbfx global object
        duk_get_global_string(ctx, "rbfx");
        duk_get_prop_string(ctx, -1, "thread");

        duk_push_boolean(ctx, false);
        duk_put_prop_string(ctx, -2, "isMainThread");

        duk_pop_2(ctx);

        duk_push_pointer(ctx, data);
        duk_put_global_string(ctx, JS_WORKER_DATA_PROP);

        duk_push_c_lightfunc(ctx, js_worker_post_message_call, 2, 2, 0);
        duk_put_global_string(ctx, "postMessage");
    }

    bool js_worker_lock(void* data)
    {
        g_thread_sync_.Acquire();
        if (g_thread_data_.contains(data))
            return true;
        js_worker_unlock();
        return false;
    }
    void js_worker_unlock()
    {
        g_thread_sync_.Release();
    }

    void js_worker_release(void* heapptr)
    {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetJSCtx());
        if (!ctx)
            return;
        // always free on main thread
        if (Thread::IsMainThread() && heapptr)
        {
            duk_push_global_stash(ctx);
            duk_get_prop_string(ctx, -1, JS_WORKERS_TABLE_PROP);
            duk_push_pointer(ctx, heapptr);
            duk_del_prop(ctx, -2);
            duk_pop_2(ctx);
        }
        else
        {
            std::promise<void>* barrier = new std::promise<void>();
            std::future<void> barrier_future = barrier->get_future();

            WorkQueue* workQueue = JavaScriptSystem::GetContext()->GetSubsystem<WorkQueue>();
            workQueue->PostTaskForMainThread([heapptr, barrier]() {
                js_worker_release(heapptr);
                barrier->set_value();
            });

            barrier_future.wait();
            delete barrier;
        }
    }
    /// rembember to lock mutex first
    void js_worker_data_release(WorkerData* data)
    {
        --data->refs_;

        js_worker_release(data->heapptr_);

        if (data->refs_ == 0)
        {
            g_thread_data_.erase(data);

            WorkQueue* workQueue = JavaScriptSystem::GetContext()->GetSubsystem<WorkQueue>();
            if (data->ctx_)
            {
                duk_context* ctx = data->ctx_;
                data->ctx_ = nullptr;
                // free thread context on correct thread idx
                workQueue->PostTaskForThread(
                [ctx]() {
                        duk_destroy_heap(ctx);
                }, TaskPriority::Highest, data->thread_idx_);
            }

            delete data;
        }
    }

    // store worker data at global map
    void js_worker_data_set(WorkerData* data)
    {
        g_thread_sync_.Acquire();
        g_thread_data_[data] = data;
        g_thread_sync_.Release();
    }

    void js_worker_thread_call(unsigned threadIdx, WorkerData* data)
    {
        duk_context* ctx = data->ctx_;
        duk_idx_t func_idx = duk_get_top_index(ctx);
        URHO3D_ASSERTLOG(!duk_is_null_or_undefined(ctx, func_idx), "error at execute worker thread. function call is null or undefined!");

        // setup thread index before call
        duk_get_global_string(ctx, "rbfx");
        duk_get_prop_string(ctx, -1, "thread");
        duk_push_uint(ctx, threadIdx);
        duk_put_prop_string(ctx, -2, "index");
        duk_pop_2(ctx);

        duk_dup(ctx, func_idx);
        duk_push_global_object(ctx);
        // execute worker
        duk_call_method(ctx, 0);
    }

    void js_worker_push_bytecode(duk_context* thread_ctx, void* bytecode, duk_size_t bytecode_len)
    {
        void* dst = duk_push_buffer(thread_ctx, bytecode_len, false);
        memcpy(dst, bytecode, bytecode_len);

        duk_load_function(thread_ctx);
    }
    void js_worker_push_function(duk_context* ctx, duk_context* thread_ctx, duk_idx_t function_idx)
    {
        // compile function into bytecode
        duk_dup(ctx, function_idx);
        duk_dump_function(ctx);

        duk_size_t bytecode_len = 0;
        void* bytecode = duk_get_buffer(ctx, -1, &bytecode_len);

        js_worker_push_bytecode(thread_ctx, bytecode, bytecode_len);

        // free function bytecode from main context.
        duk_pop(ctx);
    }

    duk_idx_t js_worker_finalizer(duk_context* ctx) {
        duk_push_current_function(ctx);
        if (duk_get_prop_string(ctx, -1, JS_WORKER_DATA_PROP))
        {
            WorkerData* data = static_cast<WorkerData*>(duk_get_pointer(ctx, -1));
            if (js_worker_lock(data)) {
                js_worker_data_release(data);
                js_worker_unlock();
            }
        }
        return 0;
    }
    void js_worker_set_finalizer(duk_context* ctx, duk_idx_t obj_idx, WorkerData* data)
    {
        duk_push_c_function(ctx, js_worker_finalizer, 0);
        duk_push_pointer(ctx, data);
        duk_put_prop_string(ctx, -2, JS_WORKER_DATA_PROP);
        duk_set_finalizer(ctx, obj_idx);
    }

    bool js_worker_get_prop(duk_context* ctx, void* heapptr, const char* prop)
    {
        if (!heapptr)
            return false;
        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_WORKERS_TABLE_PROP);

        duk_push_pointer(ctx, heapptr);
        if (duk_get_prop(ctx, -2))
            return duk_get_prop_string(ctx, -1, prop);
        return false;
    }

    void js_worker_emit_error_evt(const char* msg, void* heapptr)
    {
        if (!heapptr)
            return;

        if (Thread::IsMainThread()) {
            duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetJSCtx());
            duk_idx_t top = duk_get_top(ctx);

            if (js_worker_get_prop(ctx, heapptr, "onerror"))
            {
                duk_push_heapptr(ctx, heapptr);
                duk_push_error_object(ctx, DUK_ERR_ERROR, msg);
                duk_call_method(ctx, 1);
            }

            duk_pop_n(ctx, duk_get_top(ctx) - top);
        }
        else
        {
            std::promise<void>* barrier = new std::promise<void>();
            std::future<void> barrier_future = barrier->get_future();

            WorkQueue* workQueue = JavaScriptSystem::GetContext()->GetSubsystem<WorkQueue>();

            workQueue->PostTaskForMainThread([msg, heapptr, barrier]() {
                js_worker_emit_error_evt(msg, heapptr);
                barrier->set_value();
            });

            barrier_future.wait();
            delete barrier;
        }
    }

    void js_worker_post_message(duk_context* ctx, WorkerData* worker_data, const char* evtName, Variant value)
    {
        WorkQueue* workQueue = JavaScriptSystem::GetContext()->GetSubsystem<WorkQueue>();
        // if call is from main thread, then post message is from main thread to worker
        // otherwise, then post message is from worker to thread.
        if (Thread::IsMainThread())
        {
            
            if (js_worker_lock(worker_data))
            {
                duk_context* thread_ctx = worker_data->ctx_;
                unsigned thread_idx = worker_data->thread_idx_;
                js_worker_unlock();

                // skip post message if worker thread has been freed.
                if (!thread_ctx) return;

                workQueue->PostTaskForThread([worker_data, evtName, value]()
                {
                        if (js_worker_lock(worker_data))
                        {
                            duk_context* ctx = worker_data->ctx_;
                            js_worker_unlock();

                            if (!ctx)
                                return;

                            duk_idx_t top = duk_get_top(ctx);
                            if (duk_get_global_string(ctx, "onmessage"))
                            {
                                if (duk_is_function(ctx, -1))
                                {
                                    duk_push_global_object(ctx);
                                    duk_push_string(ctx, evtName);
                                    rbfx_push_variant(ctx, value);
                                    duk_call_method(ctx, 2);
                                }
                            }
                            duk_pop_n(ctx, duk_get_top(ctx) - top);
                        }
                }, TaskPriority::Medium, thread_idx);
            }
        }
        else
        {
            if (js_worker_lock(worker_data))
            {
                void* heapptr = worker_data->heapptr_;
                js_worker_unlock();

                // skip post message if worker has been freed on main thread.
                if (!heapptr) return;

                workQueue->PostTaskForMainThread([heapptr, evtName, value]()
                {
                        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetJSCtx());
                        duk_idx_t top = duk_get_top(ctx);
                        if (js_worker_get_prop(ctx, heapptr, "onmessage"))
                        {
                            if (duk_is_function(ctx, -1))
                            {
                                duk_push_heapptr(ctx, heapptr);
                                duk_push_string(ctx, evtName);
                                rbfx_push_variant(ctx, value);
                                duk_call_method(ctx, 2);
                            }
                        }

                        duk_pop_n(ctx, duk_get_top(ctx) - top);
                });
            }
        }
    }

    duk_idx_t js_worker_post_message_call(duk_context* ctx)
    {
        const char* evt = duk_require_string(ctx, 0);
        Variant vary = rbfx_get_variant(ctx, 1);

        WorkerData* data = nullptr;
        if (Thread::IsMainThread())
        {
            duk_push_this(ctx);
            duk_get_prop_string(ctx, -1, JS_WORKER_DATA_PROP);
            data = static_cast<WorkerData*>(duk_get_pointer(ctx, -1));

            duk_pop_2(ctx);
        }
        else
        {
            duk_get_global_string(ctx, JS_WORKER_DATA_PROP);
            data = static_cast<WorkerData*>(duk_get_pointer(ctx, -1));
            duk_pop(ctx);
        }
        js_worker_post_message(ctx, data, evt, vary);

        return 0;
    }
    duk_idx_t js_worker_terminate_call(duk_context* ctx)
    {
        duk_push_this(ctx);

        duk_get_prop_string(ctx, -1, JS_WORKER_DATA_PROP);
        WorkerData* data = static_cast<WorkerData*>(duk_get_pointer(ctx, -1));
        duk_pop_2(ctx);

        if (js_worker_lock(data))
        {
            js_worker_data_release(data);
            js_worker_unlock();
        }

        return 0;
    }

    void js_worker_setup_methods(duk_context* ctx, duk_idx_t obj_idx)
    {
        // terminate method
        duk_push_c_function(ctx, js_worker_terminate_call, 0);
        duk_put_prop_string(ctx, obj_idx, "terminate");
        // postMessage method
        duk_push_c_function(ctx, js_worker_post_message_call, 2);
        duk_put_prop_string(ctx, obj_idx, "postMessage");
    }
    void* js_worker_store_heapptr(duk_context* ctx, duk_idx_t obj_idx)
    {
        void* heapptr = duk_get_heapptr(ctx, obj_idx);
        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_WORKERS_TABLE_PROP);

        // store workers at global stash, this will prevent
        // worker to be collected by the GC.
        duk_push_pointer(ctx, heapptr);
        duk_dup(ctx, obj_idx);
        duk_put_prop(ctx, -3);
        duk_pop_2(ctx);

        return heapptr;
    }
    void js_worker_store_data_at_obj(duk_context* ctx, duk_idx_t obj_idx, WorkerData* data)
    {
        duk_push_pointer(ctx, data);
        duk_put_prop_string(ctx, obj_idx, JS_WORKER_DATA_PROP);
    }

    void js_worker_load_and_compile(duk_context* ctx, JavaScriptAsset* asset, ByteVector& bytecode)
    {
        duk_push_string(ctx, asset->GetSource().c_str());
        duk_push_string(ctx, asset->GetName().c_str());

        duk_compile(ctx, 0);
    }

    void js_worker_setup_instance(duk_context* ctx, duk_idx_t push_idx, WorkerData* data)
    {
        js_worker_store_data_at_obj(ctx, push_idx, data);

        js_worker_set_finalizer(ctx, push_idx, data);
        js_worker_setup_methods(ctx, push_idx);

        duk_dup(ctx, push_idx);

        std::promise<void>* barrier = new std::promise<void>();
        std::future<void> barrier_future = barrier->get_future();

        JavaScriptSystem::GetContext()->GetSubsystem<WorkQueue>()->PostTask([data, barrier](unsigned threadIdx, WorkQueue* queue)
        {
                data->thread_idx_ = threadIdx;
                ++data->refs_;
                barrier->set_value();
                js_worker_thread_call(threadIdx, data);
        });

        // wait worker setup thread index before leaves control
        barrier_future.wait();
        delete barrier;
    }

    duk_idx_t js_worker_file_create(duk_context* ctx)
    {
        const char* file = duk_get_string(ctx, 0);
        ResourceCache* cache = JavaScriptSystem::GetContext()->GetSubsystem<ResourceCache>();
        JavaScriptAsset* asset = cache->GetResource<JavaScriptAsset>(file);

        if (!asset)
            return duk_error(ctx, DUK_ERR_ERROR, "failed to create Worker, script file was not found. Name=%s", file);

        duk_push_this(ctx);
        duk_idx_t push_idx = duk_get_top_index(ctx);
        void* heapptr = js_worker_store_heapptr(ctx, push_idx);

        WorkerData* data = new WorkerData();
        data->heapptr_ = heapptr;
        data->refs_ = 1;
        js_create_worker_heap(data);

        js_worker_data_set(data);

        ByteVector bytecode;
        js_worker_load_and_compile(data->ctx_, asset, bytecode);

        js_worker_setup_instance(ctx, push_idx, data);

        return 1;
    }
    duk_idx_t js_worker_func_create(duk_context* ctx)
    {
        duk_push_this(ctx);
        duk_idx_t push_idx = duk_get_top_index(ctx);
        void* heapptr = js_worker_store_heapptr(ctx, push_idx);

        WorkerData* data = new WorkerData();
        data->heapptr_ = heapptr;
        data->refs_ = 1;
        js_create_worker_heap(data);

        js_worker_data_set(data);

        js_worker_push_function(ctx, data->ctx_, 0);

        js_worker_setup_instance(ctx, push_idx, data);

        return 1;
    }
    duk_idx_t js_worker_ctor(duk_context* ctx)
    {
        duk_require_constructor_call(ctx);

        if (!Thread::IsMainThread())
            return duk_error(ctx, DUK_ERR_ERROR, "Workers must be created at main thread. is not allowed to create a worker inside a worker.");

        if (duk_is_string(ctx, 0))
            return js_worker_file_create(ctx);
        else if (duk_is_function(ctx, 0))
            return js_worker_func_create(ctx);
        return duk_error(ctx, DUK_ERR_TYPE_ERROR, "Worker constructor requires script file path or function.");
    }

    unsigned js_worker_get_thread_idx(duk_context* ctx)
    {
        unsigned thread_idx = M_MAX_UNSIGNED;
        WorkerData* data = nullptr;
        if (duk_get_global_string(ctx, JS_WORKER_DATA_PROP))
            data = static_cast<WorkerData*>(duk_get_pointer(ctx, -1));
        duk_pop(ctx);

        if (js_worker_lock(data)) {
            thread_idx = data->thread_idx_;
            js_worker_unlock();
        }

        return thread_idx;
    }

    void js_setup_worker_bindings(duk_context* ctx)
    {
#if URHO3D_THREADING
        duk_push_c_lightfunc(ctx, js_worker_ctor, 1, 1, 0);
        duk_put_global_string(ctx, "Worker");

        // define workers tables
        duk_push_global_stash(ctx);
        duk_push_object(ctx);
        duk_put_prop_string(ctx, -2, JS_WORKERS_TABLE_PROP);
        duk_pop(ctx);

        duk_get_global_string(ctx, "rbfx");
        {
            duk_push_object(ctx);
            {
                duk_push_boolean(ctx, true);
                duk_put_prop_string(ctx, -2, "isMainThread");

                duk_push_int(ctx, -1);
                duk_put_prop_string(ctx, -2, "index");
            }
            duk_put_prop_string(ctx, -2, "thread");
        }
        duk_pop(ctx);
#endif
    }
}
