#include "JavaScriptPromise.h"
#include "JavaScriptOperations.h"
#include "JavaScriptErrors.h"

/// @{
/// Promise implementation is heavily inspired on promise-polyfill
/// npm package: https://github.com/taylorhakes/promise-polyfill/blob/master/src/index.js
/// @}

#define JS_PROMISE_MAGIC_ID DUK_HIDDEN_SYMBOL("__promise__") // used to identify promise objects

#define JS_PROMISE_STATE_PROP DUK_HIDDEN_SYMBOL("state")
#define JS_PROMISE_HANDLED_PROP DUK_HIDDEN_SYMBOL("handled")
#define JS_PROMISE_VALUE_PROP DUK_HIDDEN_SYMBOL("value")
#define JS_PROMISE_DEFERREDS_PROP DUK_HIDDEN_SYMBOL("deferreds")
#define JS_PROMISE_DONE_PROP DUK_HIDDEN_SYMBOL("done")

#define JS_PROMISE_RESOLVE DUK_HIDDEN_SYMBOL("Promise_resolve")
#define JS_PROMISE_REJECT DUK_HIDDEN_SYMBOL("Promise_reject")

#define JS_PROMISE_HANDLER_ONFULFILLED_PROP "onFulfilled"
#define JS_PROMISE_HANDLER_ONREJECTED_PROP "onRejected"
#define JS_PROMISE_HANDLER_PROMISE_PROP "promise"

#define JS_PROMISE_INTERNAL_SELF_PROP "self"

namespace Urho3D
{
    enum class PromiseState
    {
        pending = 0u,
        fulfilled = 1u,
        rejected = 2u,
        waiting = 3u /*special state, used when is waiting nested promise*/
    };
    struct PromiseAllResolutionData {
        duk_idx_t data_idx_;
        duk_idx_t item_idx_;
    };

    bool js_promise_handle_done(duk_context* ctx);
    void js_promise_handle(duk_context* ctx, duk_idx_t self_idx, duk_idx_t deferred_idx);
    void js_promise_resolve(duk_context* ctx, duk_idx_t self_idx, duk_idx_t value_idx);
    void js_promise_reject(duk_context* ctx, duk_idx_t self_idx, duk_idx_t error_idx);
    void js_promise_do_resolve(duk_context* ctx, duk_idx_t func_idx, duk_idx_t self_idx);

    PromiseState js_promise_get_state(duk_context* ctx, duk_idx_t promise_idx)
    {
        duk_get_prop_string(ctx, promise_idx, JS_PROMISE_STATE_PROP);
        PromiseState state = (PromiseState)duk_get_uint(ctx, -1);
        duk_pop(ctx);
        return state;
    }
    bool js_promise_is_handled(duk_context* ctx, duk_idx_t promise_idx)
    {
        duk_get_prop_string(ctx, promise_idx, JS_PROMISE_HANDLED_PROP);
        bool result = duk_get_boolean(ctx, -1);
        duk_pop(ctx);
        return result;
    }
    bool js_promise_is_promise(duk_context* ctx, duk_idx_t promise_idx)
    {
        if (duk_is_null_or_undefined(ctx, promise_idx) || !duk_is_object(ctx, promise_idx))
            return false;
        bool exists = duk_get_prop_string(ctx, promise_idx, JS_PROMISE_MAGIC_ID);
        duk_pop(ctx);
        return exists;
    }

    void js_promise_handle(duk_context* ctx, duk_idx_t self_idx, duk_idx_t deferred_idx)
    {
        duk_idx_t top = duk_get_top(ctx);
        while (js_promise_get_state(ctx, self_idx) == PromiseState::waiting)
        {
            duk_get_prop_string(ctx, self_idx, JS_PROMISE_VALUE_PROP);
            self_idx = duk_get_top_index(ctx);
        }

        if (js_promise_get_state(ctx, self_idx) == PromiseState::pending)
        {
            duk_get_prop_string(ctx, self_idx, JS_PROMISE_DEFERREDS_PROP);
            duk_idx_t len = duk_get_length(ctx, -1);
            duk_dup(ctx, deferred_idx);
            duk_put_prop_index(ctx, -2, len);

            duk_pop_n(ctx, duk_get_top(ctx) - top);
            return;
        }

        duk_push_boolean(ctx, true);
        duk_put_prop_string(ctx, self_idx, JS_PROMISE_HANDLED_PROP);


        duk_get_global_string(ctx, "setImmediate");
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_current_function(ctx);

            duk_get_prop_string(ctx, -1, JS_PROMISE_INTERNAL_SELF_PROP);
            duk_idx_t self_idx = duk_get_top_index(ctx);

            duk_get_prop_string(ctx, -2, "deferred");
            duk_idx_t deferred_idx = duk_get_top_index(ctx);

            duk_idx_t callback_idx = 0;
            PromiseState state = js_promise_get_state(ctx, self_idx);
            if (state == PromiseState::fulfilled)
            {
                duk_get_prop_string(ctx, deferred_idx, JS_PROMISE_HANDLER_ONFULFILLED_PROP);
                callback_idx = duk_get_top_index(ctx);
            }
            else
            {
                duk_get_prop_string(ctx, deferred_idx, JS_PROMISE_HANDLER_ONREJECTED_PROP);
                callback_idx = duk_get_top_index(ctx);
            }

            duk_get_prop_string(ctx, deferred_idx, JS_PROMISE_HANDLER_PROMISE_PROP);
            duk_idx_t promise_idx = duk_get_top_index(ctx);
            duk_get_prop_string(ctx, self_idx, JS_PROMISE_VALUE_PROP);
            duk_idx_t value_idx = duk_get_top_index(ctx);

            if (duk_is_null_or_undefined(ctx, callback_idx))
            {
                if (state == PromiseState::fulfilled)
                    js_promise_resolve(ctx, promise_idx, value_idx);
                else
                    js_promise_reject(ctx, promise_idx, value_idx);
                return 0;
            }

            // try call
            duk_idx_t try_call_idx = duk_get_top(ctx);
            {
                duk_push_c_function(ctx, [](duk_context* ctx) {
                    duk_push_current_function(ctx);
                    duk_idx_t this_idx = duk_get_top_index(ctx);

                    duk_get_prop_string(ctx, this_idx, JS_PROMISE_INTERNAL_SELF_PROP);
                    duk_idx_t promise_idx = duk_get_top_index(ctx);

                    duk_get_prop_string(ctx, this_idx, "cb");
                    duk_get_prop_string(ctx, this_idx, "value");
                    duk_call(ctx, 1);

                    js_promise_resolve(ctx, promise_idx, duk_get_top_index(ctx));
                    return 0;
                }, 0);
                duk_dup(ctx, callback_idx);
                duk_put_prop_string(ctx, try_call_idx, "cb");
                duk_dup(ctx, value_idx);
                duk_put_prop_string(ctx, try_call_idx, "value");
                duk_dup(ctx, promise_idx);
                duk_put_prop_string(ctx, try_call_idx, JS_PROMISE_INTERNAL_SELF_PROP);
            }
            // catch call
            duk_idx_t catch_call_idx = duk_get_top(ctx);
            {
                duk_push_c_function(ctx, [](duk_context* ctx) {
                    duk_push_current_function(ctx);
                    duk_get_prop_string(ctx, -1, JS_PROMISE_HANDLER_PROMISE_PROP);

                    js_promise_reject(ctx, duk_get_top_index(ctx), 0);
                    return 0;
                }, 0);
                duk_dup(ctx, promise_idx);
                duk_put_prop_string(ctx, catch_call_idx, JS_PROMISE_HANDLER_PROMISE_PROP);
            }

            rbfx_try_catch(ctx, try_call_idx, catch_call_idx);
            return 0;
        }, 0);
        duk_dup(ctx, self_idx);
        duk_put_prop_string(ctx, -2, JS_PROMISE_INTERNAL_SELF_PROP);
        duk_dup(ctx, deferred_idx);
        duk_put_prop_string(ctx, -2, "deferred");
        // execute setImmediate
        duk_call(ctx, 1);

        duk_pop_n(ctx, duk_get_top(ctx) - top);

    }
    void js_promise_finale(duk_context* ctx, duk_idx_t self_idx)
    {
        PromiseState state = js_promise_get_state(ctx, self_idx);
        duk_idx_t deferreds_idx = 0;
        duk_idx_t deferreds_len = 0;
        {
            duk_get_prop_string(ctx, self_idx, JS_PROMISE_DEFERREDS_PROP);
            deferreds_idx = duk_get_top_index(ctx);
            deferreds_len = duk_get_length(ctx, deferreds_idx);
        }

        if (state == PromiseState::rejected && deferreds_len == 0)
        {
            duk_get_global_string(ctx, "setImmediate");
            duk_push_c_function(ctx, [](duk_context* ctx) {
                duk_push_current_function(ctx);
                duk_get_prop_string(ctx, -1, JS_PROMISE_INTERNAL_SELF_PROP);
                bool handled = js_promise_is_handled(ctx, duk_get_top_index(ctx));
                duk_get_prop_string(ctx, -1, JS_PROMISE_VALUE_PROP);
                duk_idx_t value_idx = duk_get_top_index(ctx);

                if (!handled)
                {
                    duk_get_global_string(ctx, "console");
                    duk_get_prop_string(ctx, -1, "error");

                    duk_push_string(ctx, "Uncaught Promise. Possible Unhandled Promise Rejection:");
                    duk_dup(ctx, value_idx);

                    duk_call(ctx, 2);
                }
                return 0;
            }, 0);
            duk_dup(ctx, self_idx);
            duk_put_prop_string(ctx, -2, JS_PROMISE_INTERNAL_SELF_PROP);

            duk_call(ctx, 1);
        }

        for (duk_idx_t i = 0; i < deferreds_len; ++i)
        {
            duk_idx_t top = duk_get_top(ctx);

            duk_get_prop_index(ctx, deferreds_idx, i);
            js_promise_handle(ctx, self_idx, duk_get_top_index(ctx));

            duk_pop_n(ctx, duk_get_top(ctx) - top);
        }
        duk_push_array(ctx);
        duk_put_prop_string(ctx, self_idx, JS_PROMISE_DEFERREDS_PROP);
    }

    void js_promise_resolve(duk_context* ctx, duk_idx_t self_idx, duk_idx_t value_idx)
    {
        // try call
        duk_idx_t try_idx = duk_get_top(ctx);
        {
            duk_push_c_function(ctx, [](duk_context* ctx) {
                // Promise Resolution Procedure: https://github.com/promises-aplus/promises-spec#the-promise-resolution-procedure
                duk_push_current_function(ctx);
                duk_get_prop_string(ctx, -1, JS_PROMISE_INTERNAL_SELF_PROP);
                duk_idx_t self_idx = duk_get_top_index(ctx);
                duk_get_prop_string(ctx, -2, "value");
                duk_idx_t value_idx = duk_get_top_index(ctx);

                if (duk_equals(ctx, self_idx, value_idx))
                {
                    duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "A promise cannot be resolve with itself.");
                    return duk_throw(ctx);
                }

                if (!duk_is_null_or_undefined(ctx, value_idx) &&
                    (duk_is_object(ctx, value_idx) || duk_is_function(ctx, value_idx)))
                {
                    duk_idx_t then_idx = duk_get_top(ctx);
                    duk_get_prop_string(ctx, value_idx, "then");
                    if (js_promise_is_promise(ctx, value_idx)) {
                        duk_pop(ctx);

                        duk_push_uint(ctx, (unsigned)PromiseState::waiting);
                        duk_put_prop_string(ctx, self_idx, JS_PROMISE_STATE_PROP);

                        duk_dup(ctx, value_idx);
                        duk_put_prop_string(ctx, self_idx, JS_PROMISE_VALUE_PROP);

                        js_promise_finale(ctx, self_idx);
                        return 0;
                    }
                    else if (duk_is_function(ctx, then_idx))
                    {
                        rbfx_bind(ctx, then_idx, value_idx);
                        js_promise_do_resolve(ctx, duk_get_top_index(ctx), self_idx);
                        return 0;
                    }
                }

                duk_push_uint(ctx, (unsigned)PromiseState::fulfilled);
                duk_put_prop_string(ctx, self_idx, JS_PROMISE_STATE_PROP);

                duk_dup(ctx, value_idx);
                duk_put_prop_string(ctx, self_idx, JS_PROMISE_VALUE_PROP);

                js_promise_finale(ctx, self_idx);

                return 0;
            }, 0);
            duk_dup(ctx, self_idx);
            duk_put_prop_string(ctx, -2, JS_PROMISE_INTERNAL_SELF_PROP);
            // copy value to try call
            duk_dup(ctx, value_idx);
            duk_put_prop_string(ctx, -2, "value");
        }
        // catch call
        duk_idx_t catch_idx = duk_get_top(ctx);
        {
            duk_push_c_function(ctx, [](duk_context* ctx) {
                if(!js_promise_handle_done(ctx))
                    return 0;
                js_promise_reject(ctx, duk_get_top_index(ctx), 0);
                return 0;
            }, 0);
            duk_dup(ctx, self_idx);
            duk_put_prop_string(ctx, -2, JS_PROMISE_INTERNAL_SELF_PROP);
        }

        rbfx_try_catch(ctx, try_idx, catch_idx);
    }

    void js_promise_reject(duk_context* ctx, duk_idx_t self_idx, duk_idx_t error_idx)
    {
        duk_push_uint(ctx, (unsigned)PromiseState::rejected);
        duk_put_prop_string(ctx, self_idx, JS_PROMISE_STATE_PROP);
        duk_dup(ctx, error_idx);
        duk_put_prop_string(ctx, self_idx, JS_PROMISE_VALUE_PROP);

        js_promise_finale(ctx, self_idx);
    }

    bool js_promise_handle_done(duk_context* ctx)
    {
        duk_push_current_function(ctx);
        duk_get_prop_string(ctx, -1, JS_PROMISE_INTERNAL_SELF_PROP);
        duk_get_prop_string(ctx, -1, JS_PROMISE_DONE_PROP);

        bool done = duk_get_boolean(ctx, -1);
        duk_pop(ctx);

        if (done)
            return false;

        duk_push_boolean(ctx, true);
        duk_put_prop_string(ctx, -2, JS_PROMISE_DONE_PROP);

        return true;
    }
    void js_promise_do_resolve(duk_context* ctx, duk_idx_t func_idx, duk_idx_t self_idx)
    {
        // try call
        duk_idx_t try_idx = duk_get_top(ctx);
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_current_function(ctx);
            duk_get_prop_string(ctx, 0, "call");

            duk_get_prop_string(ctx, 0, "resolve");
            duk_get_prop_string(ctx, 0, "reject");

            duk_call(ctx, 2);
            return 0;
        }, 0);
        duk_dup(ctx, func_idx);
        duk_put_prop_string(ctx, -2, "call");
        {   // resolve call
            duk_push_c_function(ctx, [](duk_context* ctx) {
                if (duk_get_top(ctx) == 0)
                    duk_push_null(ctx);
                if (!js_promise_handle_done(ctx))
                    return 0;
                // do resolve call
                js_promise_resolve(ctx, duk_get_top_index(ctx), 0);
                return 0;
            }, 1);
            duk_dup(ctx, self_idx);
            duk_put_prop_string(ctx, -2, JS_PROMISE_INTERNAL_SELF_PROP);
            duk_put_prop_string(ctx, try_idx, "resolve");
        }
        {   // reject call
            duk_push_c_function(ctx, [](duk_context* ctx) {
                if (!js_promise_handle_done(ctx))
                    return 0;
                // do reject call
                js_promise_reject(ctx, duk_get_top_index(ctx), 0);
                return 0;
            }, 1);
            duk_dup(ctx, self_idx);
            duk_put_prop_string(ctx, -2, JS_PROMISE_INTERNAL_SELF_PROP);
            duk_put_prop_string(ctx, try_idx, "reject");
        }

        // catch call
        duk_idx_t catch_idx = duk_get_top(ctx);
        duk_push_c_function(ctx, [](duk_context* ctx) {
            if (!js_promise_handle_done(ctx))
                return 0;
            // do reject call
            js_promise_reject(ctx, duk_get_top_index(ctx), 0);
            return 0;
        }, 1);
        duk_dup(ctx, self_idx);
        duk_put_prop_string(ctx, -2, JS_PROMISE_INTERNAL_SELF_PROP);

        rbfx_try_catch(ctx, try_idx, catch_idx);
    }

    /********************************
    * BEGIN PROMISE STATIC BINDINGS *
    *********************************/
    /// @{
    /// this method is implementation of Promise.resolve
    /// but a method with this name exists, and wrap makes
    /// sense.
    /// @}
    duk_idx_t js_promise_wrap(duk_context* ctx)
    {
        if (js_promise_is_promise(ctx, 0))
            return 1;

        duk_get_global_string(ctx, "Promise");
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_current_function(ctx);
            duk_dup(ctx, 0);
            duk_get_prop_string(ctx, -2, "value");
            duk_call(ctx, 1);
            return 0;
        }, 2);
        duk_dup(ctx, 0);
        duk_put_prop_string(ctx, -2, "value");

        duk_new(ctx, 1);
        return 1;
    }
    /// @{
    /// same of js_promise_wrap, but in this case
    /// reject will be called.
    /// @}
    duk_idx_t js_promise_make_reject(duk_context* ctx)
    {
        duk_get_global_string(ctx, "Promise");
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_current_function(ctx);
            duk_dup(ctx, 1);
            duk_get_prop_string(ctx, -2, "value");
            duk_call(ctx, 1);
            return 0;
        }, 2);
        duk_dup(ctx, 0);
        duk_put_prop_string(ctx, -2, "value");
        duk_new(ctx, 1);

        return 1;
    }
    duk_idx_t js_promise_race(duk_context* ctx)
    {
        duk_get_global_string(ctx, "Promise");
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_current_function(ctx);
            duk_get_prop_string(ctx, -1, "value");
            duk_idx_t value_idx = duk_get_top_index(ctx);

            if (!duk_is_array(ctx, value_idx))
            {
                duk_dup(ctx, 1);
                duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "Invalid argument. Promise.race argument must be Iterable(such Array).");
                duk_call(ctx, 1);
                return 0;
            }

            duk_get_global_string(ctx, "Promise");
            duk_get_prop_string(ctx, -1, "resolve");
            duk_idx_t race_call_idx = duk_get_top_index(ctx);

            duk_idx_t length = duk_get_length(ctx, value_idx);
            for (duk_idx_t i = 0; i < length; ++i)
            {
                duk_idx_t top = duk_get_top(ctx);
                duk_dup(ctx, race_call_idx);
                duk_get_prop_index(ctx, value_idx, i);
                duk_call(ctx, 1);

                duk_push_string(ctx, "then");
                duk_dup(ctx, 0);
                duk_dup(ctx, 1);
                duk_call_prop(ctx, -4, 2);

                duk_pop_n(ctx, duk_get_top(ctx) - top);
            }

            return 0;
        }, 2);
        duk_dup(ctx, 0);
        duk_put_prop_string(ctx, -2, "value");
        duk_new(ctx, 1);

        return 1;
    }

    void js_promise_all_resolution(duk_context* ctx, duk_idx_t values_idx, duk_idx_t item_idx, duk_idx_t data_idx)
    {
        duk_get_global_string(ctx, "Promise");
        duk_push_string(ctx, "resolve");
        duk_get_prop_index(ctx, values_idx, item_idx);
        duk_call_prop(ctx, -3, 1);

        duk_idx_t promise_idx = duk_get_top_index(ctx);
        duk_push_string(ctx, "then");
        // promise fulfill
        {
            duk_push_c_function(ctx, [](duk_context* ctx) {
                duk_push_current_function(ctx);

                duk_uidx_t idx = 0;
                {
                    duk_get_prop_string(ctx, -1, "idx");
                    idx = duk_get_uint(ctx, -1);
                    duk_pop(ctx);
                }

                duk_get_prop_string(ctx, -1, "data");
                duk_idx_t data_idx = duk_get_top_index(ctx);
                duk_idx_t remainings = 0;
                {
                    duk_get_prop_string(ctx, -1, "remainings");
                    remainings = duk_get_uint(ctx, -1);
                    duk_pop(ctx);
                }

                duk_get_prop_string(ctx, data_idx, "output");
                duk_idx_t output_idx = duk_get_top_index(ctx);
                // fill output
                duk_dup(ctx, 0);
                duk_put_prop_index(ctx, output_idx, idx);

                --remainings;
                if (remainings > 0)
                {
                    duk_push_uint(ctx, remainings);
                    duk_put_prop_string(ctx, data_idx, "remainings");
                    return 0;
                }

                duk_get_prop_string(ctx, data_idx, "resolve");
                duk_dup(ctx, output_idx);
                duk_call(ctx, 1);

                return 0;
            }, 1);
            duk_dup(ctx, data_idx);
            duk_put_prop_string(ctx, -2, "data");
            duk_push_uint(ctx, item_idx);
            duk_put_prop_string(ctx, -2, "idx");
        }
        // promise rejected
        {
            duk_push_c_function(ctx, [](duk_context* ctx) {
                duk_push_current_function(ctx);
                duk_get_prop_string(ctx, -1, "data");
                duk_get_prop_string(ctx, -1, "reject");
                duk_dup(ctx, 0);
                duk_call(ctx, 1);
                return 0;
            }, 1);
            duk_dup(ctx, data_idx);
            duk_put_prop_string(ctx, -2, "data");
        }
        duk_call_prop(ctx, promise_idx, 2);
    }
    duk_idx_t js_promise_all(duk_context* ctx)
    {
        duk_get_global_string(ctx, "Promise");
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_current_function(ctx);
            duk_get_prop_string(ctx, -1, "value");
            duk_idx_t value_idx = duk_get_top_index(ctx);

            if (!duk_is_array(ctx, value_idx))
            {
                duk_dup(ctx, 1);
                duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "Invalid argument. Promise.all argument must be Iterable(such Array).");
                duk_call(ctx, 1);
                return 0;
            }

            duk_idx_t length = duk_get_length(ctx, value_idx);
            duk_idx_t remainings = length;
            duk_idx_t data_idx = duk_get_top(ctx);
            duk_push_object(ctx);

            duk_push_string(ctx, "slice");
            duk_call_prop(ctx, value_idx, 0);
            duk_put_prop_string(ctx, data_idx, "output");

            duk_dup(ctx, 0);
            duk_put_prop_string(ctx, data_idx, "resolve");

            duk_dup(ctx, 1);
            duk_put_prop_string(ctx, data_idx, "reject");

            duk_push_uint(ctx, length);
            duk_put_prop_string(ctx, data_idx, "remainings");
   
            for (duk_idx_t i = 0; i < length; ++i)
                js_promise_all_resolution(ctx, value_idx, i, data_idx);

            return 0;
        }, 2);
        duk_dup(ctx, 0);
        duk_put_prop_string(ctx, -2, "value");
        duk_new(ctx, 1);

        return 1;
    }

    duk_idx_t js_promise_any(duk_context* ctx)
    {
        duk_get_global_string(ctx, "Promise");
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_current_function(ctx);
            duk_get_prop_string(ctx, -1, "values");
            duk_idx_t values_idx = duk_get_top_index(ctx);

            if (duk_is_null_or_undefined(ctx, values_idx) || !duk_is_array(ctx, values_idx))
            {
                duk_dup(ctx, 1);
                duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "Invalid argument. Promise.any argument must be Iterable(such Array).");
                duk_call(ctx, 1);
                return 0;
            }

            duk_idx_t values_length = duk_get_length(ctx, values_idx);
            if (values_length == 0)
            {
                duk_dup(ctx, 1);
                js_push_aggregate_error(ctx, "All promises were rejected");
                duk_call(ctx, 1);
                return 0;
            }

            // reject call
            duk_idx_t reject_call_idx = duk_get_top(ctx);
            {
                duk_push_c_function(ctx, [](duk_context* ctx)
                {
                    duk_push_current_function(ctx);
                    duk_idx_t this_idx, errors_idx;
                    duk_uidx_t values_length, errors_length;
                    {
                        this_idx = duk_get_top_index(ctx);

                        duk_get_prop_string(ctx, this_idx, "errors");
                        errors_idx = duk_get_top_index(ctx);

                        duk_get_prop_string(ctx, this_idx, "values_length");
                        values_length = duk_get_uint(ctx, -1);
                    }

                    errors_length = duk_get_length(ctx, errors_idx);
                    // store rejection error
                    duk_dup(ctx, 0);
                    duk_put_prop_index(ctx, errors_idx, errors_length++);


                    if (errors_length == values_length)
                    {
                        duk_get_prop_string(ctx, this_idx, "reject");
                        duk_idx_t top = duk_get_top(ctx);
                        js_push_aggregate_error(ctx, errors_idx, "All promises were rejected");
                        top = duk_get_top(ctx);
                        duk_call(ctx, 1);
                    }
                    return 0;
                }, 1);
                duk_dup(ctx, 1);
                duk_put_prop_string(ctx, -2, "reject");
                duk_push_array(ctx);
                duk_put_prop_string(ctx, -2, "errors");
                duk_push_uint(ctx, values_length);
                duk_put_prop_string(ctx, -2, "values_length");
            }

            for (duk_idx_t i = 0; i < values_length; ++i)
            {
                duk_idx_t top = duk_get_top(ctx);
                duk_get_global_string(ctx, "Promise");
                duk_push_string(ctx, "resolve");
                duk_get_prop_index(ctx, values_idx, i);
                duk_call_prop(ctx, -3, 1);

                duk_push_string(ctx, "then");
                duk_dup(ctx, 0);
                duk_dup(ctx, reject_call_idx);
                duk_call_prop(ctx, -4, 2);

                duk_pop_n(ctx, duk_get_top(ctx) - top);
            }
            return 0;
        }, 2);
        duk_dup(ctx, 0);
        duk_put_prop_string(ctx, -2, "values");
        duk_new(ctx, 1);
        return 1;
    }

    duk_idx_t js_promise_all_settled_resolution(duk_context* ctx)
    {
        duk_push_current_function(ctx);

        duk_idx_t data_idx, values_idx, resolve_idx;
        duk_uidx_t idx, remainings;
        duk_bool_t is_err = false;
        {
            duk_get_prop_string(ctx, -1, "idx");
            idx = duk_get_uint(ctx, -1);
            duk_pop(ctx);

            duk_get_prop_string(ctx, -1, "isErr");
            is_err = duk_get_boolean(ctx, -1);
            duk_pop(ctx);

            duk_get_prop_string(ctx, -1, "data");
            data_idx = duk_get_top_index(ctx);

            duk_get_prop_string(ctx, data_idx, "values");
            values_idx = duk_get_top_index(ctx);

            duk_get_prop_string(ctx, data_idx, "remainings");
            remainings = duk_get_uint(ctx, -1);

            duk_get_prop_string(ctx, data_idx, "resolve");
            resolve_idx = duk_get_top_index(ctx);
        }

        // check for error first
        if (is_err)
        {
            duk_push_object(ctx);
            duk_push_string(ctx, "rejected");
            duk_put_prop_string(ctx, -2, "status");

            duk_dup(ctx, 0);
            duk_put_prop_string(ctx, -2, "reason");

            duk_put_prop_index(ctx, values_idx, idx);
        }
        else
        {
            duk_push_object(ctx);
            duk_push_string(ctx, "fulfilled");
            duk_put_prop_string(ctx, -2, "status");

            duk_dup(ctx, 0);
            duk_put_prop_string(ctx, -2, "value");

            duk_put_prop_index(ctx, values_idx, idx);
        }

        --remainings;

        if (remainings == 0)
        {
            duk_dup(ctx, resolve_idx);
            duk_dup(ctx, values_idx);
            duk_call(ctx, 1);
            return 0;
        }

        duk_push_uint(ctx, remainings);
        duk_put_prop_string(ctx, data_idx, "remainings");

        return 0;
    }
    duk_idx_t js_promise_all_settled(duk_context* ctx) {
        duk_get_global_string(ctx, "Promise");
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_current_function(ctx);
            duk_get_prop_string(ctx, -1, "values");
            duk_idx_t values_idx = duk_get_top_index(ctx);

            if (duk_is_null_or_undefined(ctx, values_idx) || !duk_is_array(ctx, values_idx))
            {
                duk_dup(ctx, 1);
                duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "Invalid argument. Promise.allSettled argument must be Iterable(such Array).");
                duk_call(ctx, 1);
                return 0;
            }

            duk_idx_t values_length = duk_get_length(ctx, values_idx);
            if (values_length == 0)
            {
                duk_dup(ctx, 0);
                duk_push_array(ctx);
                duk_call(ctx, 1);
                return 0;
            }

            duk_idx_t data_idx = duk_get_top(ctx);
            {
                duk_push_object(ctx);

                duk_dup(ctx, values_idx);
                duk_push_string(ctx, "slice");
                duk_call_prop(ctx, -2, 0);

                duk_put_prop_string(ctx, data_idx, "values");

                duk_push_uint(ctx, values_length);
                duk_put_prop_string(ctx, data_idx, "remainings");

                duk_dup(ctx, 0);
                duk_put_prop_string(ctx, data_idx, "resolve");
            }

            duk_get_global_string(ctx, "Promise");
            for (duk_idx_t idx = 0; idx < values_length; ++idx)
            {
                duk_idx_t top = duk_get_top(ctx);
                duk_push_string(ctx, "resolve");
                duk_get_prop_index(ctx, values_idx, idx);
                duk_call_prop(ctx, -3, 1);

                duk_idx_t promise_idx = duk_get_top_index(ctx);

                duk_push_string(ctx, "then");

                duk_push_c_function(ctx, js_promise_all_settled_resolution, 1);
                duk_dup(ctx, data_idx);
                duk_put_prop_string(ctx, -2, "data");
                duk_push_uint(ctx, idx);
                duk_put_prop_string(ctx, -2, "idx");
                duk_push_boolean(ctx, false);
                duk_put_prop_string(ctx, -2, "isErr");

                duk_push_c_function(ctx, js_promise_all_settled_resolution, 1);
                duk_dup(ctx, data_idx);
                duk_put_prop_string(ctx, -2, "data");
                duk_push_uint(ctx, idx);
                duk_put_prop_string(ctx, -2, "idx");
                duk_push_boolean(ctx, true);
                duk_put_prop_string(ctx, -2, "isErr");

                duk_call_prop(ctx, promise_idx, 2);

                duk_pop_n(ctx, duk_get_top(ctx) - top);
            }
            return 0;
        }, 2);
        duk_dup(ctx, 0);
        duk_put_prop_string(ctx, -2, "values");
        duk_new(ctx, 1);
        return 1;
    }
    /********************************
    * END PROMISE STATIC BINDINGS *
    *********************************/

    duk_idx_t js_promise_noop(duk_context* ctx)
    {
        return 0;
    }
    duk_idx_t js_promise_listen(duk_context* ctx, duk_idx_t fulfill_idx, duk_idx_t reject_idx)
    {
        duk_push_object(ctx);
        duk_idx_t deferred_idx = duk_get_top_index(ctx);

        duk_dup(ctx, fulfill_idx);
        duk_put_prop_string(ctx, deferred_idx, JS_PROMISE_HANDLER_ONFULFILLED_PROP);

        duk_dup(ctx, reject_idx);
        duk_put_prop_string(ctx, deferred_idx, JS_PROMISE_HANDLER_ONREJECTED_PROP);

        duk_get_global_string(ctx, "Promise");
        duk_push_c_function(ctx, js_promise_noop, 2);
        duk_new(ctx, 1);
        duk_idx_t promise_idx = duk_get_top_index(ctx);
        duk_dup(ctx, promise_idx);
        duk_put_prop_string(ctx, deferred_idx, JS_PROMISE_HANDLER_PROMISE_PROP);

        duk_push_this(ctx);
        js_promise_handle(ctx, duk_get_top_index(ctx), deferred_idx);

        duk_dup(ctx, promise_idx);
        return 1;
    }

    duk_idx_t js_promise_then(duk_context* ctx) {
        duk_idx_t top = duk_get_top(ctx);
        if (top == 0)
        {
            duk_push_null(ctx);
            duk_push_null(ctx);
        }
        else if (top == 1)
        {
            duk_require_function(ctx, 0);
            duk_push_null(ctx);
        }
        else if (top == 2)
        {
            duk_require_function(ctx, 0);
            duk_require_function(ctx, 1);
        }
        else
        {
            duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "invalid arguments");
            return duk_throw(ctx);
        }

        return js_promise_listen(ctx, 0, 1);
    }
    duk_idx_t js_promise_catch(duk_context* ctx) {
        duk_require_function(ctx, 0);
        duk_push_null(ctx);
        return js_promise_listen(ctx, 1, 0);
    }

    duk_idx_t js_promise_ctor(duk_context* ctx)
    {
        duk_require_constructor_call(ctx);
        duk_require_function(ctx, 0);

        duk_push_this(ctx);
        duk_idx_t this_idx = duk_get_top_index(ctx);
        // setup promies magic id
        duk_push_boolean(ctx, true);
        duk_put_prop_string(ctx, this_idx, JS_PROMISE_MAGIC_ID);
        // setup state prop
        duk_push_uint(ctx, 0);
        duk_put_prop_string(ctx, this_idx, JS_PROMISE_STATE_PROP);
        // setup handled prop
        duk_push_boolean(ctx, false);
        duk_put_prop_string(ctx, this_idx, JS_PROMISE_HANDLED_PROP);
        // setup value prop
        duk_push_null(ctx); // polyfill uses undefined instead of null
        duk_put_prop_string(ctx, this_idx, JS_PROMISE_VALUE_PROP);
        // setup deferreds prop
        duk_push_array(ctx);
        duk_put_prop_string(ctx, this_idx, JS_PROMISE_DEFERREDS_PROP);
        // done prop. this property is only used on doResolve call
        duk_push_boolean(ctx, false);
        duk_put_prop_string(ctx, this_idx, JS_PROMISE_DONE_PROP);

        duk_push_c_function(ctx, js_promise_then, DUK_VARARGS);
        duk_put_prop_string(ctx, this_idx, "then");

        duk_push_c_function(ctx, js_promise_catch, 1);
        duk_put_prop_string(ctx, this_idx, "catch");

        js_promise_do_resolve(ctx, 0, this_idx);

        duk_dup(ctx, this_idx);
        return 1;
    }
    void js_setup_promise_bindings(duk_context* ctx)
    {
        duk_push_c_function(ctx, js_promise_ctor, 1);

        duk_push_c_lightfunc(ctx, js_promise_wrap, 1, 1, 0);
        duk_put_prop_string(ctx, -2, "resolve");

        duk_push_c_lightfunc(ctx, js_promise_make_reject, 1, 1, 0);
        duk_put_prop_string(ctx, -2, "reject");

        duk_push_c_lightfunc(ctx, js_promise_race, 1, 1, 0);
        duk_put_prop_string(ctx, -2, "race");

        duk_push_c_lightfunc(ctx, js_promise_all, 1, 1, 0);
        duk_put_prop_string(ctx, -2, "all");

        duk_push_c_lightfunc(ctx, js_promise_any, 1, 1, 0);
        duk_put_prop_string(ctx, -2, "any");

        duk_push_c_lightfunc(ctx, js_promise_all_settled, 1, 1, 0);
        duk_put_prop_string(ctx, -2, "allSettled");

        duk_put_global_string(ctx, "Promise");
    }
}
