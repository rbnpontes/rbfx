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
#include <duktape.h>
namespace Urho3D
{
    class JavaScriptComponent : public Component {
    public:
        JavaScriptComponent(Context* context, TypeInfo* typeInfo);
        ~JavaScriptComponent();
        void SetHeapPointer(void* ptr) { heapptr_ = ptr; }
        void* GetHeapPointer() { return heapptr_; }

        void OnSetEnabled() override;
        void DrawDebugGeometry(DebugRenderer* debug, bool depthTest) override;

        void SetupBindings(duk_context* ctx, duk_idx_t this_idx);
        void ReleaseHeapptr(duk_context* ctx);

        virtual StringHash GetType() const override { return typeInfo_->GetType(); }
        virtual const ea::string& GetTypeName() const override { return typeInfo_->GetTypeName(); }
        virtual const TypeInfo* GetTypeInfo() const override { return typeInfo_; }

        static StringHash GetTypeStatic() { return GetTypeInfoStatic()->GetType(); }
        static const ea::string& GetTypeNameStatic() { return GetTypeInfoStatic()->GetTypeName(); }
        static const TypeInfo* GetTypeInfoStatic() {
            static const TypeInfo typeInfoStatic("JavaScriptComponent", Component::GetTypeInfoStatic());
            return &typeInfoStatic;
        }
        static void SetTypeInfoStatic(TypeInfo* currentType);
    protected:
        void OnNodeSet(Node* previousNode, Node* currentNode) override;
        void OnSceneSet(Scene* scene) override;
        void OnMarkedDirty(Node* node) override;
        void OnNodeSetEnabled(Node* node) override;

    private:
        void SetupEventHandler(duk_context* ctx, duk_idx_t this_idx);
        void HandleEventCall(StringHash eventType, const VariantMap& eventArgs);
        TypeInfo* typeInfo_;
        void* heapptr_;
    };


}
