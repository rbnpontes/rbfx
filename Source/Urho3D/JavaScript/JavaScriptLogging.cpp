#include "JavaScriptLogging.h"
#include "../IO/Log.h"

namespace Urho3D
{
    void Console_Print(duk_context* ctx, unsigned argc, LogLevel logLvl) {
        ea::string output = "[JavaScript]: ";
        for (unsigned i = 0; i < argc; ++i)
        {
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
                case DUK_TYPE_STRING: 
                    output.append(duk_get_string(ctx, i)); 
                    break;
                case DUK_TYPE_OBJECT:
                {
                    duk_dup(ctx, i);
                    output.append(duk_json_encode(ctx, -1));
                    duk_pop(ctx);
                }
                    break;
                case DUK_TYPE_POINTER: 
                    output.append(Format("{:x}", duk_get_pointer(ctx, i))); 
                    break;
                case DUK_TYPE_LIGHTFUNC: 
                    output.append("[Function]"); 
                    break;
                default: 
                    output.append(duk_to_string(ctx, i)); 
                    break;
            }

            if (i < argc - 1)
                output.append(" ");
        }

        Log::GetLogger().Write(logLvl, output);
    }
    void JavaScript_SetupLogger(duk_context* ctx)
    {
        duk_push_object(ctx);

        duk_push_c_function(ctx, [](duk_context* ctx) {
            Console_Print(ctx, duk_get_top(ctx), LOG_DEBUG);
            return 0;
        }, DUK_VARARGS);
        duk_dup(ctx, -1);
        duk_put_prop_string(ctx, -3, "log");
        duk_put_prop_string(ctx, -2, "debug");

        duk_push_c_function(ctx, [](duk_context* ctx) {
            Console_Print(ctx, duk_get_top(ctx), LOG_WARNING);
            return 0;
        }, DUK_VARARGS);
        duk_put_prop_string(ctx, -2, "warn");

        duk_push_c_function(ctx, [](duk_context* ctx) {
            Console_Print(ctx, duk_get_top(ctx), LOG_ERROR);
            return 0;
        }, DUK_VARARGS);
        duk_put_prop_string(ctx, -2, "error");

        duk_put_global_string(ctx, "console");
    }
}
