#include "JavaScriptComponentRegistry.h"
#include "../IO/Log.h"
#include "JavaScriptSystem.h"
#include "JavaScriptUIComponent.h"
#include "JavaScriptOperations.h"

#define JS_OBJ_FACTORIES_PROP "__factories"
#define JS_HIDDEN_COMPONENT_TYPE DUK_HIDDEN_SYMBOL("__type")
#define JS_HIDDEN_COMPONENT_CTOR DUK_HIDDEN_SYMBOL("__ctor")

namespace Urho3D
{
    enum class WrapType
    {
        Normal,
        UI
        /* If you add a new type, fill your type here*/
    };

    typedef Object* (*js_component_registry_ctor)(TypeInfo* typeInfo);
    typedef ObjectReflection* (*js_component_reflection_add)(TypeInfo* typeInfo);

    static Object* js_component_registry_internal_component_ctor(TypeInfo* typeInfo);
    static Object* js_component_registry_internal_uicomponent_ctor(TypeInfo* typeInfo);

    static ObjectReflection* js_component_registry_internal_component_reflection(TypeInfo* typeInfo);
    static ObjectReflection* js_component_registry_internal_uicomponent_reflection(TypeInfo* typeInfo);

    /* Constructor list, if you add new type must add here too */
    static ea::unordered_map<StringHash, js_component_registry_ctor> g_ctors = {
        { JavaScriptComponent::GetTypeStatic(), js_component_registry_internal_component_ctor },
        { JavaScriptUIComponent::GetTypeStatic(), js_component_registry_internal_uicomponent_ctor }
    };
    /* Typename here must be binding target to inherit correctly props */
    static ea::unordered_map<StringHash, const char*> g_types = {
        { JavaScriptComponent::GetTypeStatic(), Component::GetTypeNameStatic().c_str() },
        { JavaScriptUIComponent::GetTypeStatic(), UIComponent::GetTypeNameStatic().c_str() }
    };
    /* reflection register calls */
    static ea::unordered_map<StringHash, js_component_reflection_add> g_reflection_calls = {
        { JavaScriptComponent::GetTypeStatic(), js_component_registry_internal_component_reflection },
        { JavaScriptUIComponent::GetTypeStatic(), js_component_registry_internal_uicomponent_reflection }
    };

    /*
    * Constructor Methods
    */
    Object* js_component_registry_internal_component_ctor(TypeInfo* typeInfo) { return new JavaScriptProxyComponent(JavaScriptSystem::GetContext(), typeInfo); }
    Object* js_component_registry_internal_uicomponent_ctor(TypeInfo* typeInfo) { return new JavaScriptUIProxyComponent(JavaScriptSystem::GetContext(), typeInfo); }
    /*
    * Reflection Insert methods
    */
    ObjectReflection* js_component_registry_internal_component_reflection(TypeInfo* typeInfo) { return JavaScriptSystem::GetContext()->AddFactoryReflection<JavaScriptComponent>("Component/JavaScript"); }
    ObjectReflection* js_component_registry_internal_uicomponent_reflection(TypeInfo* typeInfo) { return JavaScriptSystem::GetContext()->AddFactoryReflection<JavaScriptUIComponent>("Component/JavaScript"); }

    /*
    * Helper Methods
    */
    static Object* js_component_registry_internal_instantiate_object(TypeInfo* typeInfo)
    {
        Object* result = nullptr;
        StringHash baseType = typeInfo->GetBaseTypeInfo()->GetType();

        auto ctorPair = g_ctors.find(baseType);
        URHO3D_ASSERTLOG(ctorPair != g_ctors.end(), "error at object instantiation. did you forget to add your type constructor ?");
        return ctorPair->second(typeInfo);
    }
    static const char* js_component_registry_internal_get_type_name(TypeInfo* typeInfo)
    {
        StringHash baseType = typeInfo->GetBaseTypeInfo()->GetType();

        auto typeNamePair = g_types.find(baseType);
        URHO3D_ASSERTLOG(typeNamePair != g_types.end(), "error at javascript type name. did you forget to add your type name entry ?");
        return typeNamePair->second;
    }
    static ObjectReflection* js_component_registry_internal_get_reflection_insert_call(TypeInfo* typeInfo)
    {
        StringHash baseType = typeInfo->GetBaseTypeInfo()->GetType();

        auto reflectionPair = g_reflection_calls.find(baseType);
        URHO3D_ASSERTLOG(reflectionPair != g_reflection_calls.end(), "error at javascript get reflection call. did you forget to add your reflection call entry ?");
        return reflectionPair->second(typeInfo);
    }

    static SharedPtr<Object> js_component_registry_internal_object_factory(const TypeInfo* type, Context* context)
    {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetJSCtx());
        duk_idx_t top = duk_get_top(ctx);

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_OBJ_FACTORIES_PROP);

        URHO3D_ASSERTLOG(
            duk_get_prop_string(ctx, -1, type->GetTypeName().c_str()),
            Format("can´t create object '{}' type. it seems this type has not yet registered.", type->GetTypeName())
        );

        duk_new(ctx, 0);
        SharedPtr<Object> result(static_cast<Object*>(rbfx_get_instance(ctx, -1)));

        duk_pop_n(ctx, duk_get_top(ctx) - top);
        return result;
    }

    static duk_idx_t js_component_registry_internal_instanceof(duk_context* ctx)
    {
        return 0;
    }

    static duk_idx_t js_component_registry_internal_creation(duk_context* ctx)
    {
        duk_idx_t argc = duk_get_top(ctx);
        duk_push_current_function(ctx);
        duk_get_prop_string(ctx, -1, JS_HIDDEN_COMPONENT_TYPE);

        TypeInfo* typeInfo = static_cast<TypeInfo*>(duk_get_pointer(ctx, -1));

        if (!duk_is_constructor_call(ctx))
        {
            duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "invalid constructor call. must call with 'new' keyword.");
            return duk_throw(ctx);
        }

        Object* instance = nullptr;
        if (argc == 0)
            instance = js_component_registry_internal_instantiate_object(typeInfo);
        else
        {
            instance = static_cast<Object*>(duk_require_pointer(ctx, 0));
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
        if (duk_get_global_string(ctx, js_component_registry_internal_get_type_name(typeInfo))) {
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
    static duk_idx_t js_component_registry_internal_wrap(duk_context* ctx, WrapType type)
    {
        duk_get_prop_string(ctx, 0, "name");
        ea::string typeName = duk_get_string_default(ctx, -1, "");
        duk_pop(ctx);

        if (typeName.empty())
        {
            duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "function used on wrap must have a name. Ex: function MyComponent() { ... }");
            return duk_throw(ctx);
        }

        auto reflection = JavaScriptSystem::GetContext()->GetReflection(typeName);
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

        TypeInfo* typeInfo = nullptr;

        if (type == WrapType::Normal)
            typeInfo = new TypeInfo(typeName.c_str(), JavaScriptComponent::GetTypeInfoStatic());
        else if (type == WrapType::UI)
            typeInfo = new TypeInfo(typeName.c_str(), JavaScriptUIComponent::GetTypeInfoStatic());

        URHO3D_ASSERTLOG(typeInfo, "Type info is null, did you forget to configure ?");

        duk_push_c_function(ctx, js_component_registry_internal_creation, DUK_VARARGS);
        // store constructor call into wrapper call
        duk_dup(ctx, 0);
        duk_put_prop_string(ctx, -2, JS_HIDDEN_COMPONENT_CTOR);
        // store type info into wrapper call
        duk_push_pointer(ctx, typeInfo);
        duk_put_prop_string(ctx, -2, JS_HIDDEN_COMPONENT_TYPE);

        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_current_function(ctx);
            if (duk_get_prop_string(ctx, -1, JS_HIDDEN_COMPONENT_TYPE)) {
                TypeInfo* typeInfo = static_cast<TypeInfo*>(duk_get_pointer(ctx, -1));
                delete typeInfo;
            }
            return 0;
        }, 0);
        duk_push_pointer(ctx, typeInfo);
        duk_put_prop_string(ctx, -2, JS_HIDDEN_COMPONENT_TYPE);
        duk_set_finalizer(ctx, -2);
        return 1;
    }

    duk_idx_t js_component_registry_uicomponent_wrap(duk_context* ctx)
    {
        return js_component_registry_internal_wrap(ctx, WrapType::UI);;
    }
    duk_idx_t js_component_registry_component_wrap(duk_context* ctx)
    {
        return js_component_registry_internal_wrap(ctx, WrapType::Normal);
    }

    duk_idx_t js_component_registry_register(duk_context* ctx)
    {
        URHO3D_PROFILE("js_component_registry_register");
        duk_require_function(ctx, 0);

        if (!duk_get_prop_string(ctx, 0, JS_HIDDEN_COMPONENT_TYPE))
        {
            duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, "invalid js component type. must use rbfx.component or rbfx.uicomponent to create a valid function.");
            return duk_throw(ctx);
        }

        TypeInfo* type = static_cast<TypeInfo*>(duk_get_pointer(ctx, -1));

        duk_push_global_stash(ctx);
        duk_get_prop_string(ctx, -1, JS_OBJ_FACTORIES_PROP);

        duk_dup(ctx, 0);
        duk_put_prop_string(ctx, -2, type->GetTypeName().c_str());
        duk_pop_2(ctx);

        Context* engineCtx = JavaScriptSystem::GetContext();
        ObjectReflection* componentReflection = js_component_registry_internal_get_reflection_insert_call(type);
        componentReflection->SetObjectFactory(js_component_registry_internal_object_factory);

        return 0;
    }

    void js_setup_component_registry(duk_context* ctx)
    {
        // setup registry
        duk_push_global_object(ctx);
        duk_push_object(ctx);
        duk_put_prop_string(ctx, -2, JS_OBJ_FACTORIES_PROP);
        duk_pop(ctx);

        // setup calls
        duk_get_global_string(ctx, "rbfx");

        duk_push_c_lightfunc(ctx, js_component_registry_register, 2, 2, 0);
        duk_put_prop_string(ctx, -2, "registerComponent");

        duk_push_c_lightfunc(ctx, js_component_registry_component_wrap, 1, 1, 0);
        duk_put_prop_string(ctx, -2, "component");

        duk_push_c_lightfunc(ctx, js_component_registry_uicomponent_wrap, 1, 1, 0);
        duk_put_prop_string(ctx, -2, "uicomponent");

        duk_pop(ctx);
    }
}
