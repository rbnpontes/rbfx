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
#include <duktape/duktape.h>

namespace Urho3D
{
    class JavaScriptAsset;
    class URHO3D_API JavaScriptSystem : public Object {
        friend class JavaScriptComponent;
        URHO3D_OBJECT(JavaScriptSystem, Object);
    public:
        JavaScriptSystem(Context* context);
        ~JavaScriptSystem();
        void Run(const JavaScriptAsset* asset);
        void Run(const ea::string& jsCode);
        static void RegisterObject(Context* context);
    private:
        static void HandleFatalError(void* udata, const char* msg);
        void Init();
        void ForceStop();
        duk_context* dukCtx_;
    };
}
