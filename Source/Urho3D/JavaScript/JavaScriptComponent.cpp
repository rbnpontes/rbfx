#include "JavaScriptComponent.h"
#include "JavaScriptSystem.h"
#include "JavaScriptOperations.h"
#include "../Scene/Node.h"
#include "../Scene/Scene.h"
#include "../Graphics/DebugRenderer.h"
#include <duktape/duktape.h>

namespace Urho3D
{
#define JS_BEGIN(ctx) \
    duk_idx_t __top = duk_get_top(ctx);
#define JS_END(ctx) \
    duk_pop_n(ctx, duk_get_top(ctx) - __top);

    JavaScriptComponentUtils::JavaScriptComponentUtils(Component* instance) : instance_(instance) {}

    void JavaScriptComponentUtils::SetProperty(const ea::string& key, const Variant& value)
    {
        if (!instance_)
            return;
        if (!instance_->GetJSHeapptr())
            return;

        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetJSCtx());
        JS_BEGIN(ctx)
        {
            duk_push_heapptr(ctx, instance_->GetJSHeapptr());
            duk_push_string(ctx, key.c_str());
            rbfx_push_variant(ctx, value);
            duk_put_prop(ctx, -3);
        }
        JS_END(ctx)
    }
    Variant JavaScriptComponentUtils::GetProperty(const ea::string& key)
    {
        Variant result;
        if (!instance_)
            return result;
        if (!instance_->GetJSHeapptr())
            return result;

        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetJSCtx());
        JS_BEGIN(ctx)
        {
            duk_push_heapptr(ctx, instance_->GetJSHeapptr());
            duk_push_string(ctx, key.c_str());
            duk_get_prop(ctx, -2);
            result = rbfx_get_variant(ctx, duk_get_top_index(ctx));
        }
        JS_END(ctx)

        return result;
    }
    bool JavaScriptComponentUtils::HasProperty(const ea::string& key)
    {
        bool result = false;
        if (!instance_)
            return result;
        if (!instance_->GetJSHeapptr())
            return result;

        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetJSCtx());
        JS_BEGIN(ctx)
        {
            duk_push_heapptr(ctx, instance_->GetJSHeapptr());
            duk_push_string(ctx, key.c_str());
            result = duk_get_prop(ctx, -2);
        }
        JS_END(ctx)

        return result;
    }
    StringVector JavaScriptComponentUtils::GetProperties()
    {
        StringVector result;
        if (!instance_)
            return result;
        if (!instance_->GetJSHeapptr())
            return result;

        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetJSCtx());
        JS_BEGIN(ctx)
        {
            duk_push_heapptr(ctx, instance_->GetJSHeapptr());
            duk_enum(ctx, -1, 0);
            while (duk_next(ctx, -1, 0))
            {
                result.push_back(duk_get_string_default(ctx, -1, ""));
                duk_pop(ctx);
            }
        }
        JS_END(ctx)

        return result;
    }
    Variant JavaScriptComponentUtils::CallFunction(const ea::string& key)
    {
        VariantVector args;
        return CallFunction(key, args);
    }
    Variant JavaScriptComponentUtils::CallFunction(const ea::string& key, const Variant& value)
    {
        VariantVector args;
        args.push_back(value);
        return CallFunction(key, args);
    }
    Variant JavaScriptComponentUtils::CallFunction(const ea::string& key, const VariantVector& args)
    {
        Variant result;

        if (!instance_)
            return result;
        if (!instance_->GetJSHeapptr())
            return result;
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetJSCtx());
        JS_BEGIN(ctx)
        {
            duk_push_heapptr(ctx, instance_->GetJSHeapptr());
            if(duk_get_prop_string(ctx, -1, key.c_str()))
            {
                duk_dup(ctx, -2);
                for (unsigned i = 0; i < args.size(); ++i)
                    rbfx_push_variant(ctx, args[i]);
                duk_call_method(ctx, args.size());

                result = rbfx_get_variant(ctx, duk_get_top_index(ctx));
            }
        }
        JS_END(ctx)

        return result;
    }
    Variant JavaScriptComponentUtils::CallFunction(const ea::string& key, const ea::function<unsigned(duk_context*)>& argsCall)
    {
        Variant result;

        if (!instance_)
            return result;
        if (!instance_->GetJSHeapptr())
            return result;
        duk_context* ctx = static_cast<duk_context*>(JavaScriptSystem::GetJSCtx());
        JS_BEGIN(ctx)
        {
            duk_push_heapptr(ctx, instance_->GetJSHeapptr());
            if(duk_get_prop_string(ctx, -1, key.c_str()))
            {
                duk_dup(ctx, -2);
                duk_call_method(ctx, argsCall(ctx));
                result = rbfx_get_variant(ctx, duk_get_top_index(ctx));
            }
        }
        JS_END(ctx)

        return result;
    }
    
    void JavaScriptComponentUtils::OnSetEnabled()
    {
        CallFunction("onSetEnabled");
    }
    void JavaScriptComponentUtils::GetDependencyNode(ea::vector<Node*>& nodes)
    {
        CallFunction("getDependencyNode", [nodes](duk_context* ctx)
        {
                duk_push_array(ctx);
                for (unsigned i = 0; i < nodes.size(); ++i)
                {
                    rbfx_push_object(ctx, nodes[i]);
                    duk_put_prop_index(ctx, -2, i);
                }
                return nodes.size();
        });
    }
    void JavaScriptComponentUtils::DrawDebugGeometry(DebugRenderer* debug, bool depthTest)
    {
        VariantVector arguments;
        arguments.push_back(debug);
        arguments.push_back(depthTest);

        CallFunction("drawDebugGeometry", arguments);
    }
    void JavaScriptComponentUtils::OnNodeSet(Node* previousNode, Node* currentNode)
    {
        VariantVector arguments;
        arguments.push_back(previousNode);
        arguments.push_back(currentNode);

        CallFunction("onNodeSet", arguments);
    }
    void JavaScriptComponentUtils::OnSceneSet(Scene* scene)
    {
        VariantVector arguments;
        arguments.push_back(scene);

        CallFunction("onSceneSet", arguments);
    }
    void JavaScriptComponentUtils::OnMarkedDirty(Node* node)
    {
        VariantVector arguments;
        arguments.push_back(node);

        CallFunction("onMarkedDirty", arguments);
    }
    void JavaScriptComponentUtils::OnNodeSetEnabled(Node* node)
    {
        VariantVector arguments;
        arguments.push_back(node);

        CallFunction("onNodeSetEnabled", arguments);
    }

    JavaScriptComponent::JavaScriptComponent(Context* context) :
        Component(context),
        utils_(ea::make_unique<JavaScriptComponentUtils>(this))
    {}

    void JavaScriptComponent::OnSetEnabled()
    {
        utils_->OnSetEnabled();
    }

    void JavaScriptComponent::GetDependencyNode(ea::vector<Node*>& nodes)
    {
        utils_->GetDependencyNode(nodes);
    }

    void JavaScriptComponent::DrawDebugGeometry(DebugRenderer* debug, bool depthTest)
    {
        utils_->DrawDebugGeometry(debug, depthTest);
    }

    void JavaScriptComponent::OnNodeSet(Node* previousNode, Node* currentNode)
    {
        utils_->OnNodeSet(previousNode, currentNode);
    }

    void JavaScriptComponent::OnSceneSet(Scene* scene)
    {
        utils_->OnSceneSet(scene);
    }

    void JavaScriptComponent::OnMarkedDirty(Node* node)
    {
        utils_->OnMarkedDirty(node);
    }

    void JavaScriptComponent::OnNodeSetEnabled(Node* node)
    {
        utils_->OnNodeSetEnabled(node);
    }
}
