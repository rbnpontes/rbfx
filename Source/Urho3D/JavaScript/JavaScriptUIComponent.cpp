#include "JavaScriptUIComponent.h"

namespace Urho3D
{
    JavaScriptUIComponent::JavaScriptUIComponent(Context* context) :
        UIComponent(context),
        utils_(ea::make_unique<JavaScriptComponentUtils>(this))
    {}

    void JavaScriptUIComponent::OnSetEnabled()
    {
        utils_->OnSetEnabled();
    }

    void JavaScriptUIComponent::GetDependencyNode(ea::vector<Node*>& nodes)
    {
        utils_->GetDependencyNode(nodes);
    }

    void JavaScriptUIComponent::DrawDebugGeometry(DebugRenderer* debug, bool depthTest)
    {
        utils_->DrawDebugGeometry(debug, depthTest);
    }

    void JavaScriptUIComponent::OnNodeSet(Node* previousNode, Node* currentNode)
    {
        utils_->OnNodeSet(previousNode, currentNode);
    }

    void JavaScriptUIComponent::OnSceneSet(Scene* scene)
    {
        utils_->OnSceneSet(scene);
    }

    void JavaScriptUIComponent::OnMarkedDirty(Node* node)
    {
        utils_->OnMarkedDirty(node);
    }

    void JavaScriptUIComponent::OnNodeSetEnabled(Node* node)
    {
        utils_->OnNodeSetEnabled(node);
    }
}
