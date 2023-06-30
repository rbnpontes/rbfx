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
    class JavaScriptAsset;
    typedef void (*js_fatal_error_function) (void *udata, const char *msg);

    class URHO3D_API JavaScriptSystem : public Object {
        friend class JavaScriptComponent;
        URHO3D_OBJECT(JavaScriptSystem, Object);
    public:
        JavaScriptSystem(Context* context);
        ~JavaScriptSystem();
        void Initialize();
        static void Run(const JavaScriptAsset* asset);
        static void Run(const ea::string& jsCode);
        static void Stop();
        static void RegisterObject(Context* context);
        /// this method is used by the RefCount object
        static void ReleaseHeapptr(void* heapptr);
        static Context* GetContext();
        static void* GetJSCtx();
        /// @{
        /// create a new js context. this pointer must be freed manually
        /// no bindings will be added to this context.
        /// only basic bindings like loggers, profilers and timers
        /// will be added to this context.
        /// memory allocation will be monitored by the profiler.
        /// this method is used by the workers to allocate heap contexts
        /// @}
        static void* CreateThreadContext(js_fatal_error_function fatalErrorFn, void* userdata = nullptr);
    private:
        static void* GetDukCtx() { return instance_->dukCtx_; }
        static void HandleFatalError(void* udata, const char* msg);
        void StopJS();
        void SetupJS();
        void RunCode(const ea::string& jsCode);
        void HandleUpdate(StringHash eventType, const VariantMap& eventArgs);

        static JavaScriptSystem* instance_;

        void* dukCtx_;
    };
}
