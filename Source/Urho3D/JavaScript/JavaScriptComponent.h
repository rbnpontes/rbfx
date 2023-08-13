//
// Copyright (c) 2017-2023 the rbfx project.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

#pragma once
#include "../Scene/Component.h"
#include <duktape/duktape.h>

namespace Urho3D
{
    /// @{
    /// helper class used to call js functions
    /// @}
    class JavaScriptComponentUtils {
    public:
        JavaScriptComponentUtils(Component* instance);
        virtual void SetProperty(const ea::string& key, const Variant& value);
        virtual Variant GetProperty(const ea::string& key);
        virtual bool HasProperty(const ea::string& key);
        virtual StringVector GetProperties();
        virtual Variant CallFunction(const ea::string& key);
        virtual Variant CallFunction(const ea::string& key, const Variant& value);
        virtual Variant CallFunction(const ea::string& key, const VariantVector& args);
        virtual Variant CallFunction(const ea::string& key, const ea::function<unsigned(duk_context*)>& argsCall);

        void OnSetEnabled();
        void GetDependencyNode(ea::vector<Node*>& nodes);
        void DrawDebugGeometry(DebugRenderer* debug, bool depthTest);
        void OnNodeSet(Node* previousNode, Node* currentNode);
        void OnSceneSet(Scene* scene);
        void OnMarkedDirty(Node* node);
        void OnNodeSetEnabled(Node* node);
    protected:
        Component* instance_;
    };

    class URHO3D_API JavaScriptComponent : public Component {
        URHO3D_OBJECT(JavaScriptComponent, Component);
    public:
        JavaScriptComponent(Context* context);
        virtual void OnSetEnabled();
        virtual void GetDependencyNode(ea::vector<Node*>& nodes);
        virtual void DrawDebugGeometry(DebugRenderer* debug, bool depthTest);
    protected:
        virtual void OnNodeSet(Node* previousNode, Node* currentNode);
        virtual void OnSceneSet(Scene* scene);
        virtual void OnMarkedDirty(Node* node);
        virtual void OnNodeSetEnabled(Node* node);
    private:
        ea::unique_ptr<JavaScriptComponentUtils> utils_;
    };

#define JS_DEFINE_PROXY_COMPONENT(typeName, baseTypeName) \
    public:\
        using ClassName = typeName; \
        using BaseClassName = baseTypeName; \
        ClassName(Context* context, TypeInfo* typeInfo) : BaseClassName(context), typeInfo_(typeInfo) {} \
        virtual StringHash GetType() const override { return typeInfo_->GetType(); } \
        virtual const TypeInfo* GetTypeInfo() const override { return typeInfo_; } \
        static StringHash GetTypeStatic() { return GetTypeInfoStatic()->GetType(); } \
        static const ea::string& GetTypeNameStatic() { return GetTypeInfoStatic()->GetTypeName(); } \
        static const TypeInfo* GetTypeInfoStatic() { return BaseClassName::GetTypeInfoStatic(); } \
    private: \
        TypeInfo* typeInfo_;

    class JavaScriptProxyComponent : public JavaScriptComponent
    {
        JS_DEFINE_PROXY_COMPONENT(JavaScriptProxyComponent, JavaScriptComponent)
    };
}
