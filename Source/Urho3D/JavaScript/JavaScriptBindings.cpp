#include "JavaScriptBindings.h"
#include "../IO/Log.h"
#include "../Resource/ResourceCache.h"
#include "../UI/Text.h"
#include "../UI/UIElement.h"
#include "../UI/UISelectable.h"

#define OBJECT_PTR_PROP DUK_HIDDEN_SYMBOL("__ptr")
#define OBJECT_TYPE_PROP DUK_HIDDEN_SYMBOL("__type")

namespace Urho3D
{
    typedef duk_ret_t(*duk_ctor_function)(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
    static JavaScriptSystem* sJsSystemInstance = nullptr;
    static Object* sCurrentObject = nullptr;

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
                output.append(duk_to_string(ctx, -1));
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

#define OBJECT_GETTER(ctx, obj_idx, propName, getFn) \
    duk_push_string(ctx, propName); \
    duk_push_c_function(ctx, [](duk_context* ctx) { \
        duk_idx_t idx = duk_get_top(ctx); \
        duk_push_this(ctx); \
        if(!duk_get_prop_string(ctx, -1, OBJECT_PTR_PROP)) \
            return 0; \
        return getFn(ctx, idx, static_cast<Object*>(duk_get_pointer(ctx, -1))); \
    }, 0); \
    duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);

#define OBJECT_GETSET(ctx, obj_idx, propName, getFn, setFn) \
    duk_push_string(ctx, propName); \
    duk_push_c_function(ctx, [](duk_context* ctx) { \
        duk_idx_t idx = duk_get_top(ctx); \
        duk_push_this(ctx); \
        if(!duk_get_prop_string(ctx, -1, OBJECT_PTR_PROP)) \
            return 0; \
        return getFn(ctx, idx, static_cast<Object*>(duk_get_pointer(ctx, -1))); \
    }, 0); \
    duk_push_c_function(ctx, [](duk_context* ctx) { \
        duk_idx_t idx = duk_get_top(ctx); \
        duk_push_this(ctx); \
        if (!duk_get_prop_string(ctx, -1, OBJECT_PTR_PROP)) {\
            URHO3D_LOGERROR("Invalid object type. Instance pointer does not exists"); \
            return DUK_RET_ERROR; \
        } \
        void* ptr = duk_get_pointer(ctx, -1); \
        duk_dup(ctx, 0); \
        return setFn(ctx, idx, static_cast<Object*>(ptr)); \
    }, 1); \
    duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_SETTER | DUK_DEFPROP_HAVE_ENUMERABLE);

    duk_ret_t Object_Finalizer(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_current_function(ctx);
            duk_get_prop_string(ctx, -1, OBJECT_PTR_PROP);

            void* ptr = duk_get_pointer_default(ctx, -1, nullptr);
            if (ptr) {
                Object* obj = static_cast<Object*>(ptr);
                obj->ReleaseRef();
            }

            return 0;
        }, 1);
        duk_push_pointer(ctx, instance);
        duk_put_prop_string(ctx, -2, OBJECT_PTR_PROP);
        duk_set_finalizer(ctx, obj_idx);
        return 0;
    }
    duk_ret_t Object_Ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {

        URHO3D_ASSERTLOG(instance, "can't define object constructor with null instance");
        duk_push_pointer(ctx, instance);
        duk_put_prop_string(ctx, obj_idx, OBJECT_PTR_PROP);
        duk_push_string(ctx, instance->GetTypeName().c_str());
        duk_put_prop_string(ctx, obj_idx, OBJECT_TYPE_PROP);
        // type prop
        OBJECT_GETTER(ctx, obj_idx, "type", [](duk_context* ctx, duk_idx_t idx, Object* instance) {
            duk_get_prop_string(ctx, idx, OBJECT_TYPE_PROP);
            return 1;
        });

        return 0;
    }
    // Subsystems Ctors
    duk_ret_t ResourceCache_Ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
        Object_Ctor(ctx, obj_idx, instance);
        return 1;
    }
    // Scene Ctors
    duk_ret_t Serializable_Ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
        Object_Ctor(ctx, obj_idx, instance);
        return 1;
    }
    duk_ret_t Animatable_Ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
        Serializable_Ctor(ctx, obj_idx, instance);
        return 1;
    }

    // UI Ctors
    duk_ret_t UIElement_Ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
        Animatable_Ctor(ctx, obj_idx, instance);
        return 1;
    }
    duk_ret_t UISelectable_Ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
        UIElement_Ctor(ctx, obj_idx, instance);
        return 1;
    }
    duk_ret_t Text_Ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
        UISelectable_Ctor(ctx, obj_idx, instance);
        OBJECT_GETSET(ctx, obj_idx, "text",
        [](duk_context* ctx, duk_idx_t thisIdx, Object* instance) {
            Text* txt = static_cast<Text*>(instance);
            duk_push_string(ctx, txt->GetText().c_str());
            return 1;
        }, [](duk_context* ctx, duk_idx_t thisIdx, Object* instance) {
            duk_require_string(ctx, -1);
            Text* txt = static_cast<Text*>(instance);
            txt->SetText(duk_get_string(ctx, -1));
            return 0;
        });
        return 1;
    }

    static ea::unordered_map<StringHash, duk_ctor_function> gCtorCalls = {
        { ResourceCache::GetTypeStatic(), ResourceCache_Ctor }
    };


    duk_ret_t GetSubsystem_Call(duk_context* ctx) {
        duk_require_string(ctx, -1);
        ea::string typeName = duk_get_string(ctx, 0);
        Object* obj = sJsSystemInstance->GetSubsystem(typeName);

        if (obj == nullptr)
            return 0;
        auto ctorIt = gCtorCalls.find(typeName);
        if (ctorIt == gCtorCalls.end())
            return 0;
        duk_idx_t idx = duk_get_top(ctx);
        duk_push_object(ctx);
        duk_ret_t result = ctorIt->second(ctx, idx, obj);
        duk_dup(ctx, idx);
        return result;
    }
    void GetSubsystem_Setup(duk_context* ctx) {
        duk_push_c_function(ctx, GetSubsystem_Call, 1);
        duk_put_global_string(ctx, "GetSubsystem");
    }

#define DEF_OBJECT_CTOR(ctx, typeName, CtorCall) \
    duk_push_c_function(ctx, [](duk_context* ctx)-> duk_ret_t { \
        if(!duk_is_constructor_call(ctx)) \
            return DUK_RET_TYPE_ERROR; \
        duk_idx_t thisIdx = duk_get_top(ctx); \
        duk_push_this(ctx); \
        typeName* obj = new typeName(sJsSystemInstance->GetContext()); \
        obj->AddRef(); \
        Object_Finalizer(ctx, thisIdx, obj); \
        duk_ret_t result = CtorCall(ctx, thisIdx, obj); \
        duk_dup(ctx, thisIdx); \
        return result; \
    }, 0); \
    duk_put_global_string(ctx, #typeName);
    
    void JavaScript_SetupBindings(duk_context* ctx, JavaScriptSystem* system)
    {
        sJsSystemInstance = system;
        SetupConsoleLogger(ctx);
        GetSubsystem_Setup(ctx);

        DEF_OBJECT_CTOR(ctx, Serializable, Serializable_Ctor);
        DEF_OBJECT_CTOR(ctx, UIElement, UIElement_Ctor);
        DEF_OBJECT_CTOR(ctx, UISelectable, UISelectable_Ctor);
        DEF_OBJECT_CTOR(ctx, Text, Text_Ctor);
    }
}
