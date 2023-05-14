#include "JavaScriptBindings.h"
#include "../IO/Log.h"

namespace Urho3D
{
    duk_ret_t HandleLog(duk_context* ctx, LogLevel lvl)
    {
        unsigned argsLength = duk_get_top(ctx);

        ea::string output = "[JavaScript]: ";

        for (unsigned i = 0; i < argsLength; ++i) {
            duk_int_t type = duk_get_type(ctx, i);
            switch (type)
            {
            case DUK_TYPE_UNDEFINED:
                output.append("undefined");
                break;
            case DUK_TYPE_NULL:
                output.append("null");
                break;
            case DUK_TYPE_BOOLEAN:
                output.append(duk_get_boolean(ctx, i) ? "true" : "false");
                break;
            case DUK_TYPE_NUMBER:
                output.append(Format("{:d}", duk_get_number(ctx, i)));
                break;
            case DUK_TYPE_STRING:
                output.append(duk_get_string(ctx, i));
                break;
            case DUK_TYPE_OBJECT:
            case DUK_TYPE_BUFFER:
                output.append(duk_json_encode(ctx, i));
                break;
            case DUK_TYPE_POINTER:
                output.append(Format("{:x}", duk_get_pointer(ctx, i)));
                break;
            case DUK_TYPE_LIGHTFUNC:
                output.append("[Function]");
                break;
            default:
                continue;
                break;
            }

            if (i < argsLength - 1)
                output.append(" ");
        }

        Log::GetLogger().Write(lvl, output);

        return 0;
    }
    duk_ret_t LogTraceCall(duk_context* ctx) { return HandleLog(ctx, LOG_DEBUG); }
    duk_ret_t LogDbgCall(duk_context* ctx) { return HandleLog(ctx, LOG_DEBUG); }
    duk_ret_t LogInfoCall(duk_context* ctx) { return HandleLog(ctx, LOG_INFO); }
    duk_ret_t LogWarnCall(duk_context* ctx) { return HandleLog(ctx, LOG_WARNING); }
    duk_ret_t LogErrCall(duk_context* ctx) { return HandleLog(ctx, LOG_ERROR); }

    void SetupConsoleLogger(duk_context* ctx)
    {
        duk_push_object(ctx);

        // console.log
        duk_push_c_function(ctx, LogTraceCall, DUK_VARARGS);
        duk_put_prop_string(ctx, -2, "log");
        // console.debug
        duk_push_c_function(ctx, LogDbgCall, DUK_VARARGS);
        duk_put_prop_string(ctx, -2, "debug");
        // console.info
        duk_push_c_function(ctx, LogInfoCall, DUK_VARARGS);
        duk_put_prop_string(ctx, -2, "info");
        // console.warn
        duk_push_c_function(ctx, LogWarnCall, DUK_VARARGS);
        duk_put_prop_string(ctx, -2, "warn");
        // console.error
        duk_push_c_function(ctx, LogErrCall, DUK_VARARGS);
        duk_put_prop_string(ctx, -2, "error");

        duk_put_global_string(ctx, "console");
    }
    void JavaScript_SetupBindings(duk_context* ctx)
    {
        SetupConsoleLogger(ctx);
    }
}
