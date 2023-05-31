// # Generated Source File
#include "JavaScriptBindings.h"
#include "JavaScriptComponent.h"
#include "JavaScriptDummy.h"
#include "../Core/Context.h"
#include "../Core/ObjectReflection.h"
#include "../IO/Log.h"
#include "../Resource/ResourceCache.h"
#include "../Resource/Resource.h"
#include "../Scene/Serializable.h"
#include "../UI/Font.h"
#include "../UI/UIElement.h"
#include "../UI/UISelectable.h"
#include "../UI/Text.h"
#include "JavaScriptGeneratedBindings.h"
#include "JavaScriptBindingUtils.h"

namespace Urho3D {
    ea::vector<SharedPtr<JavaScriptEventHandle>> gEventHandles_;
    ea::queue<unsigned> gAvailableEventIds_;
    ea::unordered_map<StringHash, ea::shared_ptr<TypeInfo>> gRegisteredComponents_;

    duk_context* JavaScriptBindings::dukCtx_ = nullptr;
    Context* JavaScriptBindings::rbfxCtx_ = nullptr;

    void JavaScriptBindings::RequireType(duk_context* ctx, duk_idx_t obj_idx, StringHash typeName) {
        if(!duk_get_prop_string(ctx, obj_idx, JS_OBJECT_PTR_PROP)) {
            duk_pop(ctx);
            return;
        }
        Object* instance = static_cast<Object*>(duk_get_pointer_default(ctx, -1, nullptr));
        URHO3D_ASSERTLOG(instance, "Instance is null");
        URHO3D_ASSERTLOG(instance->IsInstanceOf(typeName), "Invalid InstanceOf type.");
    }
    Object* JavaScriptBindings::GetObjectInstance(duk_context* ctx, duk_idx_t obj_idx, StringHash typeName) {
        if(!duk_get_prop_string(ctx, obj_idx, JS_OBJECT_PTR_PROP)) {
            duk_pop(ctx);
            return nullptr;
        }
        Object* instance = static_cast<Object*>(duk_get_pointer_default(ctx, -1, nullptr));
        duk_pop(ctx);
        if(instance->IsInstanceOf(typeName))
            return instance;
        return nullptr;
    }
    Object* JavaScriptBindings::GetObjectInstance(duk_context* ctx, duk_idx_t obj_idx) {
        if(!duk_get_prop_string(ctx, obj_idx, JS_OBJECT_PTR_PROP)) {
            duk_pop(ctx);
            return nullptr;
        }
        Object* instance = static_cast<Object*>(duk_get_pointer_default(ctx, -1, nullptr));
        duk_pop(ctx);

        return instance;
    }

    void JavaScriptBindings::Setup(Context* rbfxCtx) {
        if (dukCtx_)
            return;
        dukCtx_ = duk_create_heap(
            nullptr,
            nullptr,
            nullptr,
            nullptr,
            JavaScriptBindings::HandleFatalError
        );
        rbfxCtx_ = rbfxCtx;
        Setup_Bindings(dukCtx_);
    }
    void JavaScriptBindings::Destroy() {
        gEventHandles_.clear();
        if (dukCtx_)
            duk_destroy_heap(dukCtx_);
        dukCtx_ = nullptr;
    }
    void JavaScriptBindings::RunCode(const ea::string& code) {
        if (!dukCtx_) {
            if (!rbfxCtx_)
                return;
            Setup(rbfxCtx_);
        }

        URHO3D_ASSERTLOG(dukCtx_, "duk_context has not been created.");

        // If we run code directly on heap
        // all resouces allocated will stay alive until heap is destroyed.
        // In this way, finalizers will be called at end of function.
        ea::string scopedCode = "(function() {\n";
        scopedCode.append(code);
        scopedCode.append("})();");

        duk_eval_string_noresult(dukCtx_, scopedCode.c_str());
    }

    void JavaScriptBindings::HandleFatalError(void* udata, const char* msg) {
        ea::string errMsg(msg == nullptr ? "No message" : msg);
        URHO3D_LOGERROR("**FATAL ERROR** {}", errMsg);
    }

    void Call_Event(unsigned callerId, StringHash eventType, const VariantMap& eventArgs) {
        duk_context* ctx = JavaScriptBindings::GetJSCtx();

        duk_push_global_stash(ctx);
        if (!duk_get_prop_index(ctx, -1, callerId)) {
            URHO3D_LOGWARNING("Invalid Event Id");
            return;
        }

        duk_push_uint(ctx, eventType.Value());
        push_variant(ctx, eventArgs);
        duk_call(ctx, 2);
    }
    void Register_Event(duk_context* ctx, duk_idx_t callback_idx, StringHash eventType)
    {
        unsigned id = 0u;

        SharedPtr<JavaScriptEventHandle> handle;

        if (duk_get_prop_string(ctx, callback_idx, JS_EVENT_CALLBACK_ID_PROP)) {
            id = duk_get_uint(ctx, -1);
            duk_pop(ctx);

            URHO3D_ASSERTLOG(id < gEventHandles_.size(), "Callback has event id hidden property but event handle does not exists.");
            handle = gEventHandles_.at(id);
        }
        else if (gAvailableEventIds_.empty()) {
            id = gEventHandles_.size();
            gEventHandles_.push_back(nullptr);
        }
        else {
            id = gAvailableEventIds_.front();
            gAvailableEventIds_.pop();
        }

        if (!handle) {
            handle = MakeShared<JavaScriptEventHandle>(JavaScriptBindings::GetContext());
            handle->SetId(id);

            duk_push_uint(ctx, id);
            duk_put_prop_string(ctx, callback_idx, JS_EVENT_CALLBACK_ID_PROP);

            duk_push_global_stash(ctx);
            duk_dup(ctx, callback_idx);
            duk_put_prop_index(ctx, -2, id);
            duk_pop(ctx);
        }

        handle->Connect(eventType);
        gEventHandles_[id] = handle;
    }
    void Remove_Event(duk_context* ctx, duk_idx_t callback_idx, StringHash eventType)
    {
        if (!duk_get_prop_string(ctx, callback_idx, JS_EVENT_CALLBACK_ID_PROP)) {
            URHO3D_LOGWARNING("Callback used has not been registered on any event.");
            return;
        }

        unsigned id = duk_get_uint(ctx, -1);
        duk_pop(ctx);

        URHO3D_ASSERTLOG(gEventHandles_[id], "callback has event id hidden property but event handle does not exists.");

        auto handle = gEventHandles_[id];
        handle->Disconnect(eventType);

        if (handle->GetConnectedEvents() == 0) {
            gEventHandles_[id] = nullptr;
            // store id as next slot available.
            gAvailableEventIds_.push(id);

            // remove stored callback
            duk_push_global_stash(ctx);
            duk_del_prop_index(ctx, -1, id);
            duk_pop(ctx);
        }
    }

    void JSComponent_SetFinalizer(duk_context* ctx, duk_idx_t this_idx, JavaScriptComponent* instance) {
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_current_function(ctx);
            duk_get_prop_string(ctx, -1, JS_OBJECT_PTR_PROP);

            void* ptr = duk_get_pointer_default(ctx, -1, nullptr);
            JavaScriptComponent* component = static_cast<JavaScriptComponent*>(ptr);
            if (component) {
                component->ReleaseHeapptr(ctx);
                component->ReleaseRef();
            }

            return 0;
        }, 1);
        duk_push_pointer(ctx, instance);
        duk_put_prop_string(ctx, -2, JS_OBJECT_PTR_PROP);
        duk_set_finalizer(ctx, this_idx);
    }
    SharedPtr<Object> JSComponent_Factory(const TypeInfo* type, Context* context) {
        SharedPtr<JavaScriptComponent> result(new JavaScriptComponent(context, const_cast<TypeInfo*>(type)));
        duk_context* ctx = JavaScriptBindings::GetJSCtx();
        push_object(ctx, result);

        return result;
    }
    void Call_RegisterComponent(duk_context* ctx, duk_idx_t ctor_idx, const char* typeName) {
        URHO3D_ASSERTLOG(typeName, "Typename cannot be empty.");

        {
            unsigned typeLen = strlen(typeName);
            for (unsigned i = 0; i < typeLen; ++i) {
                char c = typeName[i];
                URHO3D_ASSERTLOG(
                    (c >= 48 && c <= 57 && i > 0) ||
                    (c >= 65 && c <= 90) ||
                    (c >= 97 && c <= 122) ||
                    c == '_',
                    "Invalid Type name for this component"
                );
            }
        }

        StringHash typeHash = typeName;
        Context* engineCtx = JavaScriptBindings::GetContext();

        auto typeIt = gRegisteredComponents_.find(typeHash);
        // Remove registered type
        if (typeIt != gRegisteredComponents_.end()) {
            if (engineCtx->IsReflected(typeHash))
                engineCtx->RemoveReflection(typeHash);
            gRegisteredComponents_.erase(typeIt);
        }
        else {
            // We don't want that user overwrites system types.
            URHO3D_ASSERTLOG(!engineCtx->IsReflected(typeName), Format("{} is a engine registered type, its not possible to change or overwrite.", typeName));
        }

        ea::shared_ptr<TypeInfo> typeInfo = ea::make_shared<TypeInfo>(typeName, Component::GetTypeInfoStatic());

        JavaScriptDummy::SetTypeInfoStatic(typeInfo.get());
        ObjectReflection* objReflection = engineCtx->AddFactoryReflection<JavaScriptDummy>("Component/JavaScript");

        objReflection->SetObjectFactory(JSComponent_Factory);

        gRegisteredComponents_[typeHash] = typeInfo;

        // Delete object from global if has been registered.
        if (duk_get_global_string(ctx, typeName)) {
            duk_pop(ctx);
            duk_push_global_object(ctx);
            duk_del_prop_string(ctx, -1, typeName);
            duk_pop(ctx);
        }

        // Add to global JS Component
        duk_push_c_function(ctx, [](duk_context* ctx) {
            if (!duk_is_constructor_call(ctx)) {
				URHO3D_LOGERROR("Invalid Constructor Call. Must call with 'new' keyword.");
				return DUK_RET_TYPE_ERROR;
			}

            duk_push_current_function(ctx);
            duk_get_prop_string(ctx, -1, JS_OBJECT_TYPE_PROP);
            const char* typeName = duk_get_string(ctx, -1);
            duk_pop_2(ctx);

            duk_idx_t argc = duk_get_top(ctx);
            SharedPtr<JavaScriptComponent> instance;

            if(argc > 1) {
				URHO3D_LOGERROR("Invalid Constructor Call for Type Text.");
				return DUK_RET_TYPE_ERROR;
			}

            if (argc == 1)
                instance = static_cast<JavaScriptComponent*>(duk_require_pointer(ctx, 0));
            else {
                Context* engineCtx = JavaScriptBindings::GetContext();
                TypeInfo* typeInfo = const_cast<TypeInfo*>(engineCtx->GetReflection(typeName)->GetTypeInfo());
                instance = new JavaScriptComponent(engineCtx, typeInfo);
            }

            instance->AddRef();

            duk_push_this(ctx);

            // acquire js script constructor callback from stash and add to push instance
            duk_push_global_stash(ctx);
            duk_get_prop_string(ctx, -1, JS_OBJECT_COMPONENT_PROP);
            duk_get_prop_string(ctx, -1, typeName);
            duk_put_prop_string(ctx, -4, JS_OBJECT_COMPONENT_PROP);
            duk_pop_2(ctx);

            // insert heapptr to heap pointers table
            void* heapptr = duk_get_heapptr(ctx, argc);
            lock_safe_heapptr(ctx, heapptr);

            if (argc == 0)
                instance->SetHeapPointer(heapptr);

            Component_ctor(ctx, argc, instance);
            JSComponent_SetFinalizer(ctx, argc, instance);

            instance->SetupBindings(ctx, argc);
            // execute js script constructor callback
            duk_push_this(ctx);
            duk_push_string(ctx, JS_OBJECT_COMPONENT_PROP);
            duk_call_prop(ctx, -2, 0);

            // return this
            duk_push_this(ctx);
            return 1;
        }, DUK_VARARGS);

        duk_push_string(ctx, typeName);
        duk_put_prop_string(ctx, -2, JS_OBJECT_TYPE_PROP);
        duk_put_global_string(ctx, typeName);

        // store constructor constructor into global stash
        duk_push_global_stash(ctx);
        if (!duk_get_global_string(ctx, JS_OBJECT_COMPONENT_PROP)) {
            duk_pop(ctx);
            duk_push_object(ctx);
            duk_dup(ctx, -1);
            duk_put_prop_string(ctx, -3, JS_OBJECT_COMPONENT_PROP);
        }

        duk_dup(ctx, ctor_idx);
        duk_put_prop_string(ctx, -2, typeName);
        duk_pop(ctx);
    }

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
    void Wrap_Object(duk_context* ctx, Object* instance) {
        URHO3D_ASSERTLOG(instance, "instance cannot be null");
        duk_get_global_string(ctx, instance->GetTypeName().c_str());
        if(duk_is_null_or_undefined(ctx, -1)) {
            // If constructor has not declared. Return raw Object.
            duk_pop(ctx);
            
            URHO3D_LOGWARNING("Object with typename {} is not mapped.", instance->GetTypeName());
            instance->AddRef();
            duk_idx_t obj_idx = duk_get_top(ctx);
            duk_push_object(ctx);
            Object_ctor(ctx, obj_idx, instance);
            Object_Finalizer(ctx, obj_idx, instance);
        } else {
            duk_push_pointer(ctx, instance);
            duk_new(ctx, 1);
        }
    }
    void Setup_Primitive(duk_context* ctx, duk_idx_t obj_idx, const char* primitiveTypeName) {
        duk_push_string(ctx, primitiveTypeName);
        duk_put_prop_string(ctx, obj_idx, JS_OBJECT_TYPE_PROP);
        duk_push_string(ctx, "type");
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_this(ctx);
            duk_get_prop_string(ctx, -1, JS_OBJECT_TYPE_PROP);
            return 1;
        }, 0);
        duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
    }
    duk_ret_t Object_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
        URHO3D_ASSERTLOG(instance, "can't define object constructor with null instance");
        duk_push_pointer(ctx, instance);
        duk_put_prop_string(ctx, obj_idx, JS_OBJECT_PTR_PROP);
        duk_push_string(ctx, instance->GetTypeName().c_str());
        duk_put_prop_string(ctx, obj_idx, JS_OBJECT_TYPE_PROP);
        // type prop
        duk_push_string(ctx, "type");
        duk_push_c_function(ctx, [](duk_context* ctx){
            duk_push_this(ctx);
            duk_get_prop_string(ctx, -1, JS_OBJECT_TYPE_PROP);
            return 1;
        }, 0);
        duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
        
        return 0;
    }
    duk_ret_t Object_Finalizer(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
        duk_push_c_function(ctx, [](duk_context* ctx) {
            duk_push_current_function(ctx);
            duk_get_prop_string(ctx, -1, JS_OBJECT_PTR_PROP);

            void* ptr = duk_get_pointer_default(ctx, -1, nullptr);
            if (ptr) {
                Object* obj = static_cast<Object*>(ptr);
                obj->ReleaseRef();
            }

            return 0;
        }, 1);
        duk_push_pointer(ctx, instance);
        duk_put_prop_string(ctx, -2, JS_OBJECT_PTR_PROP);
        duk_set_finalizer(ctx, obj_idx);

        return 0;
    }
}
