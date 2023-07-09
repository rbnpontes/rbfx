#include "JavaScriptTimers.h"
#include "../Core/Profiler.h"
#include "../Core/Thread.h"
#include "../Core/WorkQueue.h"
#include "../Core/Context.h"
#include "../Core/Mutex.h"
#include "JavaScriptWorker.h"
#include "JavaScriptSystem.h"

#include <future>

#define JS_TIMER_CALLS_PROP "__timers"
#define JS_TIMER_HIDDEN_DUMMY_FINALIZER DUK_HIDDEN_SYMBOL("timer_dummy")

namespace Urho3D
{
    struct JSTimer
    {
        void* heapptr_;
        unsigned timeout_;
        duk_double_t scheduleTime_;
        bool isInterval_;
        JSTimer* next_;
        JSTimer* prev_;
    };
    struct JSTimerContext
    {
        duk_context* ctx_;
        unsigned thread_idx_;
        JSTimer* timers_;
        JSTimer* timers_2_delete_;
    };

    static Mutex g_timer_sync_;
    static ea::unordered_map<duk_context*, ea::shared_ptr<JSTimerContext>> g_timer_contexts_;

    // acquire mutex
    void js_timer_lock()
    {
        g_timer_sync_.Acquire();
    }
    void js_timer_unlock()
    {
        g_timer_sync_.Release();
    }

    JSTimer* js_create_timer(duk_context* ctx, duk_idx_t call_idx, duk_idx_t timeout_idx, bool isInterval)
    {
        duk_require_function(ctx, call_idx);
        unsigned timeout = duk_require_uint(ctx, timeout_idx);

        JSTimer* timer = new JSTimer();
        timer->timeout_ = timeout;
        timer->scheduleTime_ = duk_get_now(ctx) + timeout;
        timer->isInterval_ = isInterval;
        timer->heapptr_ = duk_get_heapptr(ctx, call_idx);
        timer->next_ = timer->prev_ = nullptr;

        return timer;
    }
    void js_add_timer(duk_context* ctx, ea::shared_ptr<JSTimerContext> timer_ctx, JSTimer* timer)
    {
        if (!timer) return;

        // add timer to timers list
        timer->prev_ = timer_ctx->timers_;
        if (timer_ctx->timers_)
            timer_ctx->timers_->next_ = timer;
        timer_ctx->timers_ = timer;

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_TIMER_CALLS_PROP);

        duk_push_pointer(ctx, timer);
        duk_push_heapptr(ctx, timer->heapptr_);
        duk_put_prop(ctx, -3);

        duk_pop_2(ctx);
    }
    void js_destroy_native_timer(ea::shared_ptr<JSTimerContext> timer_ctx, JSTimer* timer)
    {
        if (!timer_ctx || !timer) return;

        JSTimer* prev = timer->prev_;
        JSTimer* next = timer->next_;

        if (prev)
            prev->next_ = next;
        if (next)
            next->prev_ = prev;
        if (timer == timer_ctx->timers_)
            timer_ctx->timers_ = prev;

        timer->prev_ = timer_ctx->timers_2_delete_;
        timer_ctx->timers_2_delete_ = timer;
    }
    bool js_destroy_js_timer(duk_context* ctx, duk_idx_t timers_idx, JSTimer* timer)
    {
        if (!timer) return false;

        duk_push_pointer(ctx, timer);

        if (!duk_has_prop(ctx, timers_idx))
            return false;

        duk_push_pointer(ctx, timer);
        duk_del_prop(ctx, timers_idx);

        return true;
    }

    void js_destroy_all_js_timers(duk_context* ctx)
    {
        duk_push_global_stash(ctx);
        // put empty object to make object as invalid to GC collect calls
        duk_push_object(ctx);
        duk_put_prop_string(ctx, -2, JS_TIMER_CALLS_PROP);

        duk_pop(ctx);
    }
    void js_free_timers(ea::shared_ptr<JSTimerContext> timer_ctx)
    {
        JSTimer* prev = timer_ctx->timers_2_delete_;
        while (prev != nullptr)
        {
            JSTimer* curr = prev;
            prev = prev->prev_;

            delete curr;
        }
        timer_ctx->timers_2_delete_ = nullptr;
    }

    duk_idx_t js_timer_ctx_finalizer(duk_context* ctx)
    {
        js_timer_lock();
        g_timer_contexts_.erase(ctx);
        js_timer_unlock();
        return 0;
    }
    void js_add_timer_ctx_finalizer(duk_context* ctx)
    {
        // little hack
        // if worker context goes to destroy
        // remove then timer context from g_timer_contexts and free some memory.
        duk_push_object(ctx);
        duk_push_c_function(ctx, js_timer_ctx_finalizer, 0);
        duk_set_finalizer(ctx, -2);

        duk_put_global_string(ctx, JS_TIMER_HIDDEN_DUMMY_FINALIZER);
    }
    duk_idx_t js_add_timer_call(duk_context* ctx, bool is_interval)
    {
        JSTimer* timer = js_create_timer(ctx, 0, 1, is_interval);

        js_timer_lock();

        ea::shared_ptr<JSTimerContext> timer_ctx;
        auto timer_ctx_it = g_timer_contexts_.find(ctx);
        if (timer_ctx_it == g_timer_contexts_.end())
        {
            timer_ctx = ea::make_shared<JSTimerContext>();
            timer_ctx->ctx_ = ctx;
            timer_ctx->timers_ = nullptr;
            timer_ctx->timers_2_delete_ = nullptr;
            if (Thread::IsMainThread())
                timer_ctx->thread_idx_ = M_MAX_UNSIGNED;
            else
            {
                timer_ctx->thread_idx_ = js_worker_get_thread_idx(ctx);
                js_add_timer_ctx_finalizer(ctx);
            }


            g_timer_contexts_[ctx] = timer_ctx;
        }

        js_add_timer(ctx, timer_ctx, timer);

        js_timer_unlock();        

        duk_push_pointer(ctx, timer);
        return 1;
    }

    duk_idx_t js_set_timeout_call(duk_context* ctx)
    {
        URHO3D_PROFILE("setTimeout");

        return js_add_timer_call(ctx, false);
    }
    duk_idx_t js_set_interval_call(duk_context* ctx)
    {
        URHO3D_PROFILE("setInterval");
        return js_add_timer_call(ctx, true);
    }
    duk_idx_t js_set_immediate_call(duk_context* ctx)
    {
        URHO3D_PROFILE("setImmediate");
        // immediate works like setTimeout
        // in this case, immediate has timeout zero
        // this means that immediate calls must be
        // called on next frame.
        duk_push_uint(ctx, 0);
        return js_add_timer_call(ctx, false);
    }

    duk_idx_t js_clear_timer(duk_context* ctx)
    {
        JSTimer* timer = static_cast<JSTimer*>(duk_require_pointer(ctx, 0));
        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_TIMER_CALLS_PROP);

        if (js_destroy_js_timer(ctx, duk_get_top(ctx) - 1, timer))
        {
            js_timer_lock();
            auto timer_ctx_it = g_timer_contexts_.find(ctx);
            URHO3D_ASSERTLOG(timer_ctx_it != g_timer_contexts_.end(), "can´t clear timer that has not yet registered. it seems timer context has not been created.");

            js_destroy_native_timer(timer_ctx_it->second, timer);
            js_timer_unlock();
        }

        duk_pop_2(ctx);
        return 0;
    }
    duk_idx_t js_clear_timeout_call(duk_context* ctx)
    {
        return js_clear_timer(ctx);
    }
    duk_idx_t js_clear_interval_call(duk_context* ctx)
    {
        return js_clear_timer(ctx);
    }
    duk_idx_t js_clear_immediate_call(duk_context* ctx)
    {
        return js_clear_timer(ctx);
    }
    duk_idx_t js_clear_all_timers_call(duk_context* ctx)
    {
        js_timer_lock();
        auto timer_ctx_it = g_timer_contexts_.find(ctx);
        if (timer_ctx_it == g_timer_contexts_.end())
        {
            js_timer_unlock();
            return 0;
        }

        js_free_timers(timer_ctx_it->second);

        timer_ctx_it->second->timers_2_delete_ = timer_ctx_it->second->timers_;
        timer_ctx_it->second->timers_ = nullptr;

        js_free_timers(timer_ctx_it->second);

        js_timer_unlock();

        js_destroy_all_js_timers(ctx);
        return 0;
    }

    void js_setup_timer_bindings(duk_context* ctx)
    {
        duk_push_c_lightfunc(ctx, js_set_timeout_call, 2, 2, 0);
        duk_put_global_string(ctx, "setTimeout");

        duk_push_c_lightfunc(ctx, js_set_interval_call, 2, 2, 0);
        duk_put_global_string(ctx, "setInterval");

        duk_push_c_lightfunc(ctx, js_set_immediate_call, 1, 1, 0);
        duk_put_global_string(ctx, "setImmediate");

        duk_push_c_lightfunc(ctx, js_clear_timeout_call, 1, 1, 0);
        duk_put_global_string(ctx, "clearTimeout");

        duk_push_c_lightfunc(ctx, js_clear_immediate_call, 1, 1, 0);
        duk_put_global_string(ctx, "clearImmediate");

        duk_push_c_lightfunc(ctx, js_clear_interval_call, 1, 1, 0);
        duk_put_global_string(ctx, "clearInterval");

        duk_get_global_string(ctx, "rbfx");
        duk_push_c_lightfunc(ctx, js_clear_all_timers_call, 0, 0, 0);
        duk_put_prop_string(ctx, -2, "clearAllTimers");
        duk_pop(ctx);

        duk_push_global_stash(ctx);
        duk_push_object(ctx);
        duk_put_prop_string(ctx, -2, JS_TIMER_CALLS_PROP);
        duk_pop(ctx);
    }

    void js_resolve_timer(ea::shared_ptr<JSTimerContext> timer_ctx)
    {
        duk_context* ctx = timer_ctx->ctx_;
        // g_timers always point to the last element
        JSTimer* prev = timer_ctx->timers_;

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_TIMER_CALLS_PROP);

        duk_idx_t timers_idx = duk_get_top(ctx) - 1;
        duk_double_t now = duk_get_now(ctx);

        unsigned call_count = 0;
        // collect timer callbacks and add to delete list timeout timers
        while (prev != nullptr)
        {
            JSTimer* curr_timer = prev;
            prev = prev->prev_;
            
            if (now < curr_timer->scheduleTime_)
                continue;

            duk_push_heapptr(ctx, curr_timer->heapptr_);
            ++call_count;
            if (curr_timer->isInterval_)
                curr_timer->scheduleTime_ = now + curr_timer->timeout_; // calculate next exec time.
            else
            {
                js_destroy_js_timer(ctx, timers_idx, curr_timer);
                js_destroy_native_timer(timer_ctx, curr_timer);
            }
        }

        js_free_timers(timer_ctx);
        // execute callbacks
        for (unsigned i = 0; i < call_count; ++i)
        {
            duk_call(ctx, 0);
            duk_pop(ctx);
        }

        duk_pop_2(ctx);
    }
    void js_resolve_timers()
    {
        URHO3D_PROFILE("js_resolve_timers");
        URHO3D_ASSERTLOG(Thread::IsMainThread(), "js_resolve_timers must called on main thread.");

        WorkQueue* work_queue = JavaScriptSystem::GetContext()->GetSubsystem<WorkQueue>();
        js_timer_lock();
        auto values = g_timer_contexts_.values();
        js_timer_unlock();

        for (auto timer_ctx : values)
        {
            if (!timer_ctx)
                return;
            if (timer_ctx->thread_idx_ == M_MAX_UNSIGNED)
                js_resolve_timer(timer_ctx);
            else
                work_queue->PostTaskForThread([timer_ctx]() { js_resolve_timer(timer_ctx); }, TaskPriority::Medium, timer_ctx->thread_idx_);
        }
    }
    void js_clear_all_timers()
    {
        js_timer_lock();
        auto contexts = g_timer_contexts_.values();
        for (auto timer_ctx : contexts)
        {
            js_free_timers(timer_ctx);
        }
        g_timer_contexts_.clear();
        js_timer_unlock();
    }
    void js_clear_internal_timers(duk_context* ctx)
    {
        js_destroy_all_js_timers(ctx);
    }
}
