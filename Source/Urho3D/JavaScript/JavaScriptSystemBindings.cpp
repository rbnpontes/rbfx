#include "JavaScriptSystemBindings.h"
#include "JavaScriptOperations.h"
#include "JavaScriptSystem.h"
#include "JavaScriptComponent.h"
#include "../Core/Context.h"

#define JS_HIDDEN_COMPONENT_TYPE DUK_HIDDEN_SYMBOL("__type")
#define JS_HIDDEN_COMPONENT_CTOR DUK_HIDDEN_SYMBOL("__ctor")
namespace Urho3D
{
    duk_ret_t GetSubsystem_Call(duk_context* ctx)
    {
        StringHash type = rbfx_require_string_hash(ctx, 0);
        Object* obj = JavaScriptSystem::GetContext()->GetSubsystem(type);
        rbfx_push_object(ctx, obj);
        return 1;
    }

    duk_ret_t ComponenCreation_Call(duk_context* ctx) {
        duk_idx_t argc = duk_get_top(ctx);
        duk_push_current_function(ctx);
        duk_get_prop_string(ctx, -1, JS_HIDDEN_COMPONENT_TYPE);

        TypeInfo* typeInfo = static_cast<TypeInfo*>(duk_get_pointer(ctx, -1));

        if (!duk_is_constructor_call(ctx))
        {
            duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "invalid constructor call. must call with 'new' keyword.");
            return duk_throw(ctx);
        }

        JavaScriptProxyComponent* instance = nullptr;
        if (argc == 0) {
            instance = new JavaScriptProxyComponent(JavaScriptSystem::GetContext(), typeInfo);
        }
        else {
            instance = static_cast<JavaScriptProxyComponent*>(duk_require_pointer(ctx, 0));
            if (!instance)
            {
                duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "pointer argument is null");
                return duk_throw(ctx);
            }
        }

        void* heapptr = instance->GetJSHeapptr();
        if (rbfx_is_valid_heapptr(ctx, heapptr))
        {
            duk_push_heapptr(ctx, heapptr);
            return 1;
        }

        duk_push_this(ctx);
        heapptr = duk_get_heapptr(ctx, -1);
        instance->SetJSHeapptr(heapptr);
        instance->AddRef();

        rbfx_lock_heapptr(ctx, heapptr);

        // Call Component Wrap method.
        if (duk_get_global_string(ctx, "Component")) {
            duk_push_this(ctx);
            duk_push_pointer(ctx, instance);
            duk_call_method(ctx, 1);
        }
        else {
            duk_push_this(ctx);
            rbfx_object_wrap(ctx, duk_get_top(ctx) - 1, instance);
        }

        duk_push_this(ctx);
        rbfx_set_finalizer(ctx, -1, instance);

        // call constructor call
        duk_push_current_function(ctx);
        duk_get_prop_string(ctx, -1, JS_HIDDEN_COMPONENT_CTOR);
        duk_push_this(ctx);
        duk_call_method(ctx, 0);

        duk_push_this(ctx);
        return 1;
    }
    duk_ret_t ComponentWrap_Call(duk_context* ctx)
    {
        ea::string typeName = duk_require_string(ctx, 1);
        auto reflection = JavaScriptSystem::GetContext()->GetReflection(typeName);
        // throw error if object with this name has been declared.
        if (reflection)
        {
            ea::string error = Format("'{}' has been declared.", typeName);
            duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, error.c_str());
            return duk_throw(ctx);
        }

        // validate typename
        {
            for (unsigned i = 0; i < typeName.length(); ++i) {
                char c = typeName.at(i);
                if (!((c >= 48 && c <= 57 && i > 0) ||
                    (c >= 65 && c <= 90) ||
                    (c >= 97 && c <= 122) ||
                    c == '_'))
                {
                    ea::string error = Format("'{}' is invalid declaration type.", typeName);
                    duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, error.c_str());
                    return duk_throw(ctx);
                }
            }
        }

        TypeInfo* typeInfo = new TypeInfo(typeName.c_str(), JavaScriptComponent::GetTypeInfoStatic());

        duk_push_c_function(ctx, ComponenCreation_Call, DUK_VARARGS);
        // store constructor call into wrapper call
        duk_dup(ctx, 0);
        duk_put_prop_string(ctx, -2, JS_HIDDEN_COMPONENT_CTOR);
        // store type info into wrapper call
        duk_push_pointer(ctx, typeInfo);
        duk_put_prop_string(ctx, -2, JS_HIDDEN_COMPONENT_TYPE);

        return 1;
    }

    void JavaScript_SetupSystemBindings(duk_context* ctx)
    {
        duk_push_object(ctx);

        duk_push_c_function(ctx, GetSubsystem_Call, 1);
        duk_put_prop_string(ctx, -2, "getSubsystem");

        duk_push_c_function(ctx, ComponentWrap_Call, 2);
        duk_put_prop_string(ctx, -2, "component");

        duk_put_global_string(ctx, "rbfx");
    }
}
