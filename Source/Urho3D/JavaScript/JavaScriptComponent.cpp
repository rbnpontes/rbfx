#include "JavaScriptComponent.h"
#include "JavaScriptBindings.h"
#include "JavaScriptBindingUtils.h"
namespace Urho3D
{
#define JS_HIDDEN_EVENT_CALLBACK_OBJ DUK_HIDDEN_SYMBOL("__eventcall")
    JavaScriptComponent::JavaScriptComponent(Context* context, TypeInfo* typeInfo) :
        Component(context),
        typeInfo_(typeInfo),
        heapptr_(nullptr) {}
    JavaScriptComponent::~JavaScriptComponent() {
        ReleaseHeapptr(JavaScriptBindings::GetJSCtx());
    }

    void JavaScriptComponent::OnSetEnabled() {
        duk_context* ctx = JavaScriptBindings::GetJSCtx();
        if (!push_safe_heapptr(ctx, heapptr_))
            return;

        duk_idx_t obj_idx = duk_get_top(ctx) - 1;
        if (!duk_get_prop_string(ctx, obj_idx, "onSetEnabled")) {
            duk_pop_2(ctx);
            return;
        }
        duk_pop(ctx);

        duk_push_string(ctx, "onSetEnabled");
        duk_call_prop(ctx, obj_idx, 0);
    }
    void JavaScriptComponent::DrawDebugGeometry(DebugRenderer* debug, bool depthTest) {
        duk_context* ctx = JavaScriptBindings::GetJSCtx();
        if (!push_safe_heapptr(ctx, heapptr_))
            return;
        duk_idx_t obj_idx = duk_get_top(ctx) - 1;
        if (!duk_get_prop_string(ctx, obj_idx, "drawDebugGeometry")) {
            duk_pop_2(ctx);
            return;
        }
        duk_pop(ctx);

        // Add Debug Renderer
        if (!duk_get_global_string(ctx, "DebugRenderer")) {
            duk_pop_2(ctx);
            return;
        }
        duk_push_pointer(ctx, debug);
        duk_new(ctx, 1);
        // Add depth test
        duk_push_boolean(ctx, depthTest);
        // Call prop method
        duk_push_string(ctx, "drawDebugGeometry");
        duk_call_prop(ctx, obj_idx, 2);
    }

    void JavaScriptComponent::OnNodeSet(Node* previousNode, Node* currentNode) {
        duk_context* ctx = JavaScriptBindings::GetJSCtx();
        if (!push_safe_heapptr(ctx, heapptr_))
            return;

        duk_idx_t obj_idx = duk_get_top(ctx) - 1;
        if (!duk_get_prop_string(ctx, obj_idx, "onNodeSet")) {
            duk_pop_2(ctx);
            return;
        }
        duk_pop(ctx);

        // Set Node
        if (!duk_get_global_string(ctx, "Node")) {
            duk_pop_2(ctx);
            return;
        }

        duk_push_pointer(ctx, previousNode);
        duk_new(ctx, 1);
        // Set Node
        duk_get_global_string(ctx, "Node");
        duk_push_pointer(ctx, previousNode);
        duk_new(ctx, 1);
        // Call prop method
        duk_push_string(ctx, "onNodeSet");
        duk_call_prop(ctx, obj_idx, 2);
    }
    void JavaScriptComponent::OnSceneSet(Scene* scene) {
        duk_context* ctx = JavaScriptBindings::GetJSCtx();
        if (!push_safe_heapptr(ctx, heapptr_))
            return;

        duk_idx_t obj_idx = duk_get_top(ctx) - 1;
        if (duk_get_prop_string(ctx, obj_idx, "onSceneSet")) {
            duk_pop_2(ctx);
            return;
        }
        duk_pop(ctx);

        if (!duk_get_global_string(ctx, "Scene")) {
            duk_pop_2(ctx);
            return;
        }

        duk_push_pointer(ctx, scene);
        duk_new(ctx, 1);

        duk_push_string(ctx, "onSceneSet");
        duk_call_prop(ctx, obj_idx, 1);
    }
    void JavaScriptComponent::OnMarkedDirty(Node* node) {
        duk_context* ctx = JavaScriptBindings::GetJSCtx();
        if (!push_safe_heapptr(ctx, heapptr_))
            return;

        duk_idx_t obj_idx = duk_get_top(ctx) - 1;
        if (!duk_get_prop_string(ctx, obj_idx, "onMarkedDirty")) {
            duk_pop_2(ctx);
            return;
        }
        duk_pop(ctx);

        if (!duk_get_global_string(ctx, "Node")) {
            duk_pop_2(ctx);
            return;
        }

        duk_push_pointer(ctx, node);
        duk_new(ctx, 1);

        duk_push_string(ctx, "onMarkedDirty");
        duk_call_prop(ctx, obj_idx, 1);
    }
    void JavaScriptComponent::OnNodeSetEnabled(Node* node) {
        duk_context* ctx = JavaScriptBindings::GetJSCtx();
        if (!push_safe_heapptr(ctx, heapptr_))
            return;

        duk_idx_t obj_idx = duk_get_top(ctx) - 1;
        if (!duk_get_prop_string(ctx, obj_idx, "onNodeSetEnabled")) {
            duk_pop_2(ctx);
            return;
        }
        duk_pop(ctx);

        if (!duk_get_global_string(ctx, "Node")) {
            duk_pop_2(ctx);
            return;
        }

        duk_push_pointer(ctx, node);
        duk_new(ctx, 1);

        duk_push_string(ctx, "onNodeSetEnabled");
        if (!duk_get_prop(ctx, obj_idx)) {
            duk_pop_n(ctx, 4);
            return;
        }

        duk_pop(ctx);
        duk_call_prop(ctx, obj_idx, 1);
    }

    void JavaScriptComponent::SetupBindings(duk_context* ctx, duk_idx_t this_idx) {
        SetupEventHandler(ctx, this_idx);
        duk_pop(ctx);
    }
    void JavaScriptComponent::ReleaseHeapptr(duk_context* ctx) {
        unlock_safe_heapptr(ctx, heapptr_);
        heapptr_ = nullptr;
    }
    void JavaScriptComponent::SetupEventHandler(duk_context* ctx, duk_idx_t this_idx) {
        duk_push_c_function(ctx, [](duk_context* ctx) {
            StringHash eventType = require_string_hash(ctx, 0);
            duk_require_function(ctx, 1);

            duk_push_this(ctx);
            JavaScriptComponent* component = static_cast<JavaScriptComponent*>(get_object(ctx, -1));

            duk_get_prop_string(ctx, -1, JS_HIDDEN_EVENT_CALLBACK_OBJ);
            duk_dup(ctx, 1);
            duk_put_prop_index(ctx, -2, eventType.Value());

            component->SubscribeToEvent(eventType, URHO3D_HANDLER(JavaScriptComponent, HandleEventCall));
            return 0;
        }, 2);
        duk_put_prop_string(ctx, this_idx, "subscribeToEvent");
        // create hidden event callback object
        duk_push_object(ctx);
        duk_put_prop_string(ctx, -2, JS_HIDDEN_EVENT_CALLBACK_OBJ);
    }
    void JavaScriptComponent::HandleEventCall(StringHash eventType, const VariantMap& eventArgs) {
        duk_context* ctx = JavaScriptBindings::GetJSCtx();
        if (!push_safe_heapptr(ctx, heapptr_))
            return;

        duk_get_prop_string(ctx, -1, JS_HIDDEN_EVENT_CALLBACK_OBJ);
        if (!duk_get_prop_index(ctx, -1, eventType.Value()))
            return;

        duk_dup(ctx, -3);
        duk_push_uint(ctx, eventType.Value());
        push_variant(ctx, eventArgs);

        duk_call_method(ctx, 2);
    }
}
