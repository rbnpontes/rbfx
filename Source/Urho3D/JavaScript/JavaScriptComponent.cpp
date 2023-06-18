#include "JavaScriptComponent.h"
#include "JavaScriptSystem.h"
#include "JavaScriptOperations.h"
#include "../Scene/Node.h"
#include "../Scene/Scene.h"
#include "../Graphics/DebugRenderer.h"
#include <duktape/duktape.h>

namespace Urho3D
{
    JavaScriptComponent::JavaScriptComponent(Context* context) :
        Component(context) {}

    void JavaScriptComponent::OnSetEnabled()
    {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetDukCtx());
        if (GetJSProperty("onSetEnabled"))
        {
            duk_dup(ctx, -2);
            duk_call_method(ctx, 0);
        }
    }

    void JavaScriptComponent::GetDependencyNode(ea::vector<Node*>& desc)
    {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetDukCtx());
        if (GetJSProperty("getDependencyNode"))
        {
            duk_dup(ctx, -2);
            duk_push_array(ctx);
            for (unsigned i = 0; i < desc.size(); ++i) {
                Node* node = desc.at(i);
                rbfx_push_object(ctx, node);
                duk_put_prop_index(ctx, -2, i);
            }
            duk_call_method(ctx, 1);
        }
    }

    void JavaScriptComponent::DrawDebugGeometry(DebugRenderer* debug, bool depthTest)
    {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetDukCtx());
        if (GetJSProperty("drawDebugGeometry"))
        {
            duk_dup(ctx, -2);

            rbfx_push_object(ctx, debug);
            duk_push_boolean(ctx, depthTest);

            duk_call_method(ctx, 2);
        }
    }

    void JavaScriptComponent::OnNodeSet(Node* previousNode, Node* currentNode)
    {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetDukCtx());
        if (GetJSProperty("onNodeSet"))
        {
            duk_dup(ctx, -2);
            rbfx_push_object(ctx, previousNode);
            rbfx_push_object(ctx, currentNode);

            duk_call_method(ctx, 2);
        }
    }

    void JavaScriptComponent::OnSceneSet(Scene* scene)
    {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetDukCtx());
        if (GetJSProperty("onSceneSet"))
        {
            duk_dup(ctx, -2);

            rbfx_push_object(ctx, scene);
            duk_call_method(ctx, 1);
        }
    }

    void JavaScriptComponent::OnMarkedDirty(Node* node)
    {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetDukCtx());
        if (GetJSProperty("onMarkedDirty"))
        {
            duk_dup(ctx, -2);

            rbfx_push_object(ctx, node);
            duk_call_method(ctx, 1);
        }
    }

    void JavaScriptComponent::OnNodeSetEnabled(Node* node)
    {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetDukCtx());
        if (GetJSProperty("onNodeSetEnabled"))
        {
            duk_dup(ctx, -2);

            rbfx_push_object(ctx, node);
            duk_call_method(ctx, 1);
        }
    }

    bool JavaScriptComponent::GetJSProperty(const char* propKey)
    {
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetDukCtx());
        if (!GetJSHeapptr())
            return false;
        rbfx_push_object(ctx, this);
        return duk_get_prop_string(ctx, -1, propKey);
    }


    JavaScriptProxyComponent::JavaScriptProxyComponent(Context* context, TypeInfo* typeInfo) :
        JavaScriptComponent(context),
        typeInfo_(typeInfo)
    {
    }
}
