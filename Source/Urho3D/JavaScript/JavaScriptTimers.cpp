#include "JavaScriptTimers.h"
#include "../Core/Profiler.h"

#define JS_TIMER_CALLS_PROP "__timers"
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

    static JSTimer* g_timers = nullptr;
    static JSTimer* g_timers_2_delete = nullptr;

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
    void js_add_timer(duk_context* ctx, JSTimer* timer)
    {
        if (!timer) return;

        // add timer to timers list
        timer->prev_ = g_timers;
        if (g_timers)
            g_timers->next_ = timer;
        g_timers = timer;

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_TIMER_CALLS_PROP);

        duk_push_pointer(ctx, timer);
        duk_push_heapptr(ctx, timer->heapptr_);
        duk_put_prop(ctx, -3);

        duk_pop_2(ctx);
    }
    void js_destroy_native_timer(JSTimer* timer)
    {
        if (!timer) return;

        JSTimer* prev = timer->prev_;
        JSTimer* next = timer->next_;

        if (prev)
            prev->next_ = next;
        if (next)
            next->prev_ = prev;
        if (timer == g_timers)
            g_timers = prev;

        timer->prev_ = g_timers_2_delete;
        g_timers_2_delete = timer;
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
    void js_free_timers()
    {
        JSTimer* prev = g_timers_2_delete;
        while (prev != nullptr)
        {
            JSTimer* curr = prev;
            prev = prev->prev_;

            delete curr;
        }
        g_timers_2_delete = nullptr;
    }

    duk_idx_t js_add_timer_call(duk_context* ctx, bool is_interval)
    {
        JSTimer* timer = js_create_timer(ctx, 0, 1, is_interval);
        js_add_timer(ctx, timer);

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

    duk_idx_t js_clear_timer(duk_context* ctx)
    {
        JSTimer* timer = static_cast<JSTimer*>(duk_require_pointer(ctx, 0));

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_TIMER_CALLS_PROP);

        if(js_destroy_js_timer(ctx, duk_get_top(ctx) - 1, timer))
            js_destroy_native_timer(timer);

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
    duk_idx_t js_clear_all_timers_call(duk_context* ctx)
    {
        js_free_timers();

        g_timers_2_delete = g_timers;
        g_timers = nullptr;

        js_free_timers();
        js_destroy_all_js_timers(ctx);
        return 0;
    }

    void JavaScript_SetupTimerBindings(duk_context* ctx)
    {
        duk_push_c_lightfunc(ctx, js_set_timeout_call, 2, 2, 0);
        duk_put_global_string(ctx, "setTimeout");

        duk_push_c_lightfunc(ctx, js_set_interval_call, 2, 2, 0);
        duk_put_global_string(ctx, "setInterval");

        duk_push_c_lightfunc(ctx, js_clear_timeout_call, 1, 1, 0);
        duk_put_global_string(ctx, "clearTimeout");

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
    void JavaScript_ResolveTimers(duk_context* ctx)
    {
        URHO3D_PROFILE("JavaScript->ResolveTimers");
        // g_timers always point to the last element
        JSTimer* prev = g_timers;

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_TIMER_CALLS_PROP);

        duk_idx_t timers_idx = duk_get_top(ctx) - 1;
        duk_double_t now = duk_get_now(ctx);

        unsigned call_count = 0;
        // collect timer callbacks and add to delete list timeout timers
        while (prev != nullptr)
        {
            JSTimer* currTimer = prev;
            prev = prev->prev_;
            
            if (now >= currTimer->scheduleTime_)
            {
                duk_push_heapptr(ctx, currTimer->heapptr_);
                ++call_count;
                if (currTimer->isInterval_)
                    currTimer->scheduleTime_ = now + currTimer->timeout_; // calculate next exec time.
                else
                {
                    js_destroy_js_timer(ctx, timers_idx, currTimer);
                    js_destroy_native_timer(currTimer);
                }
            }

        }

        js_free_timers();
        // execute callbacks
        for (unsigned i = 0; i < call_count; ++i)
        {
            duk_call(ctx, 0);
            duk_pop(ctx);
        }

        duk_pop_2(ctx);
    }
    void JavaScript_ClearAllTimers()
    {
        js_free_timers();
    }
    void JavaScript_ClearInternalTimers(duk_context* ctx)
    {
        js_destroy_all_js_timers(ctx);
    }
}
