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
#include "../Core/Object.h"
namespace Urho3D
{
    //@{
    // This dummy is used to inject js objects into reflection registry
    // and must not be used to any other purposes.
    //@}
    class JavaScriptDummy : public Object {
    public:
        JavaScriptDummy(Context* context);

        virtual StringHash GetType() const override { return currentType_->GetType(); }
        virtual const ea::string& GetTypeName() const override { return currentType_->GetTypeName(); }
        virtual const TypeInfo* GetTypeInfo() const override { return currentType_; }

        static StringHash GetTypeStatic() { return currentType_->GetType(); }
        static const ea::string& GetTypeNameStatic() { return currentType_->GetTypeName(); }
        static const TypeInfo* GetTypeInfoStatic() { return currentType_; }
        static void SetTypeInfoStatic(TypeInfo* currentType);
    private:
        static TypeInfo* currentType_;
    };
}
