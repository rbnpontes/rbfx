#include "JavaScriptErrors.h"

#define JS_TYPE_AGGREGATE_ERR "AggregateError"
namespace Urho3D
{
    duk_idx_t js_aggregate_error(duk_context* ctx)
    {
        duk_idx_t argc = duk_get_top(ctx);

        duk_push_this(ctx);

        duk_push_string(ctx, JS_TYPE_AGGREGATE_ERR);
        duk_put_prop_string(ctx, -2, "name");

        if (argc > 0)
        {
            duk_dup(ctx, 0);
            duk_put_prop_string(ctx, -2, "errors");
        }
        if (argc > 1)
        {
            duk_dup(ctx, 1);
            const char* msg = duk_get_string(ctx, -1);
            duk_put_prop_string(ctx, -2, "message");
        }

        return 1;
    }

    void js_push_aggregate_error(duk_context* ctx, duk_idx_t errors_idx)
    {
        js_push_aggregate_error(ctx, errors_idx, "");
    }
    void js_push_aggregate_error(duk_context* ctx, duk_idx_t errors_idx, duk_idx_t msg_idx)
    {
        duk_get_global_string(ctx, JS_TYPE_AGGREGATE_ERR);
        duk_dup(ctx, errors_idx);
        duk_dup(ctx, msg_idx);
        duk_new(ctx, 2);
    }
    void js_push_aggregate_error(duk_context* ctx, duk_idx_t errors_idx, const char* message)
    {
        duk_push_string(ctx, message);
        duk_idx_t msg_idx = duk_get_top_index(ctx);
        js_push_aggregate_error(ctx, errors_idx, msg_idx);
        duk_remove(ctx, msg_idx);
    }
    void js_push_aggregate_error(duk_context* ctx, const char* message)
    {
        duk_push_array(ctx);
        duk_idx_t arr_idx = duk_get_top_index(ctx);
        js_push_aggregate_error(ctx, arr_idx, message);
        duk_remove(ctx, arr_idx);
    }
    void js_setup_error_bindings(duk_context* ctx)
    {
        duk_push_c_function(ctx, js_aggregate_error, DUK_VARARGS);
        {
            duk_get_global_string(ctx, "Error");
            duk_set_prototype(ctx, -2);
        }
        duk_put_global_string(ctx, JS_TYPE_AGGREGATE_ERR);
    }
}
