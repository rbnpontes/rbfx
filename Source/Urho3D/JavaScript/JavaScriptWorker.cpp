#include "JavaScriptWorker.h"
#include "JavaScriptSystem.h"
#include "../Core/WorkQueue.h"
#include "../Core/Context.h"
#include "../IO/Log.h"

#define JS_WORKERS_TABLE_PROP "__workers"
#define JS_WORKER_THREAD_PROP DUK_HIDDEN_SYMBOL("thread")
#define JS_WORKER_DATA_PROP DUK_HIDDEN_SYMBOL("data")
#define JS_WORKER_FUNCTION_PROP "__workerFn"

namespace Urho3D
{
    struct WorkerData {
        bool invalid_;
        void* heapptr_;
        duk_context* ctx_;
    };

    void js_free_worker(WorkerData* data);
    void js_worker_fatal_error(void* udata, const char* msg)
    {
        ea::string errMsg(msg == nullptr ? "no message" : msg);
        URHO3D_LOGERROR("**FATAL ERROR(Worker)** {}", errMsg);
        URHO3D_ASSERTLOG(udata, "userdata is required to free worker on fatal error");

        WorkerData* data = static_cast<WorkerData*>(udata);
        // if data is invalid, then main thread has been destroyed by error
        // or something like that. in this case, we just destroy instead of
        // call to free on main thread.
        if (data->invalid_) {
            delete data;
            return;
        }
        data->invalid_ = true;

        // worker must be freed on main thread
        JavaScriptSystem::GetContext()->GetSubsystem<WorkQueue>()->PostTaskForMainThread(
            [=]() {
                js_free_worker(data);
            }
        );
    }

    /// create worker context. duk_context* result will be stored at WorkerData struct
    void js_create_worker_heap(WorkerData* data)
    {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::CreateThreadContext(js_worker_fatal_error, data));
        data->ctx_ = ctx;

        // add thread object to rbfx global object
        duk_get_global_string(ctx, "rbfx");
        duk_push_object(ctx);
        duk_put_prop_string(ctx, -2, "thread");
        duk_pop(ctx);
    }
    void js_free_worker(WorkerData* workerData) {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetJSCtx());

        // remove worker data from worker object
        duk_push_heapptr(ctx, workerData->heapptr_);
        duk_del_prop_string(ctx, -1, JS_WORKER_DATA_PROP);
        duk_pop(ctx);

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_WORKERS_TABLE_PROP);
        duk_push_pointer(ctx, workerData->heapptr_);
        duk_del_prop(ctx, -2);

        duk_pop_2(ctx);
        // note: delete worker data is not required
        // because finalizer will destroy struct on main thread
    }

    void js_handle_thread_call(unsigned threadIdx, WorkerData* data)
    {
        duk_context* ctx = data->ctx_;
        duk_idx_t func_idx = duk_get_top(ctx) - 1;
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

    void js_push_function_to_thread(duk_context* ctx, duk_context* thread_ctx, duk_idx_t function_idx)
    {
        // compile function into bytecode
        duk_dup(ctx, function_idx);
        duk_dump_function(ctx);

        // read buffer and copy to thread context
        duk_size_t buff_size = 0;
        void* buff_data = duk_get_buffer(ctx, -1, &buff_size);

        void* thread_buff_data = duk_push_buffer(thread_ctx, buff_size, false);
        memcpy(thread_buff_data, buff_data, buff_size);

        // free function bytecode
        duk_pop(ctx);

        // load bytecode function on thread context
        duk_load_function(thread_ctx);
    }

    duk_idx_t js_worker_file_create(duk_context* ctx)
    {
        return 1;
    }
    duk_idx_t js_worker_func_create(duk_context* ctx)
    {
        duk_push_this(ctx);
        duk_idx_t push_idx = duk_get_top(ctx) - 1;
        void* heapptr = duk_get_heapptr(ctx, push_idx);

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_WORKERS_TABLE_PROP);

        // store workers at global stash, this will prevent
        // worker to be collected by the GC.
        duk_push_pointer(ctx, heapptr);
        duk_dup(ctx, push_idx);
        duk_put_prop(ctx, -3);
        duk_pop_2(ctx);

        WorkerData* data = new WorkerData();
        data->heapptr_ = heapptr;
        js_create_worker_heap(data);

        js_push_function_to_thread(ctx, data->ctx_, 0);

        duk_push_pointer(ctx, data);
        duk_put_prop_string(ctx, push_idx, JS_WORKER_DATA_PROP);

        duk_push_this(ctx);

        JavaScriptSystem::GetContext()->GetSubsystem<WorkQueue>()->PostTask([data](unsigned threadIdx, WorkQueue* queue)
        {
                js_handle_thread_call(threadIdx, data);
        });

        return 1;
    }
    duk_idx_t js_worker_ctor(duk_context* ctx)
    {
        duk_require_constructor_call(ctx);

        if (duk_is_string(ctx, 0))
            return js_worker_file_create(ctx);
        else if (duk_is_function(ctx, 0))
            return js_worker_func_create(ctx);
        return duk_error(ctx, DUK_ERR_TYPE_ERROR, "Worker constructor requires script file path or function.");
    }

    void js_setup_worker_bindings(duk_context* ctx)
    {
        duk_push_c_lightfunc(ctx, js_worker_ctor, 1, 1, 0);
        duk_put_global_string(ctx, "Worker");

        // define workers tables
        duk_push_global_stash(ctx);
        duk_push_object(ctx);
        duk_put_prop_string(ctx, -2, JS_WORKERS_TABLE_PROP);
        duk_pop(ctx);
    }
}
