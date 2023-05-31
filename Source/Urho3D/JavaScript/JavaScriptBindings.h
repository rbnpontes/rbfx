// # Generated Header File
#pragma once
#include <duktape/duktape.h>
#include <EASTL/functional.h>

#include "../JavaScript/JavaScriptSystem.h"
#include "../Math/StringHash.h"
#include "../IO/Log.h"
#include "../JavaScript/JavaScriptEventHandle.h"
#include <EASTL/queue.h>


#define JS_OBJECT_PTR_PROP DUK_HIDDEN_SYMBOL("__ptr")
#define JS_OBJECT_TYPE_PROP DUK_HIDDEN_SYMBOL("__type")
#define JS_OBJECT_HEAPPTR_PROP DUK_HIDDEN_SYMBOL("__heapptr")
#define JS_OBJECT_COMPONENT_PROP DUK_HIDDEN_SYMBOL("__ccall")
#define JS_EVENT_CALLBACK_ID_PROP DUK_HIDDEN_SYMBOL("__eventid")
#define JS_STASH_COMPONENT_PROP "components"

namespace Urho3D {
    typedef duk_ret_t(*duk_ctor_function)(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
    class JavaScriptBindings {
        public:
            static void Setup(Context* rbfxCtx);
            static void RunCode(const ea::string& code);
            static duk_context* GetJSCtx() { return dukCtx_; }
            static Context* GetContext() { return rbfxCtx_; }
            static void RequireType(duk_context* ctx, duk_idx_t obj_idx, StringHash typeName);
            static Object* GetObjectInstance(duk_context* ctx, duk_idx_t obj_idx, StringHash typeName);
            static Object* GetObjectInstance(duk_context* ctx, duk_idx_t obj_idx);
            static void Destroy();
        private:
            static void HandleFatalError(void* udata, const char* msg);

            static duk_context* dukCtx_;
            static Context* rbfxCtx_;
    };

    void Call_Event(unsigned callerId, StringHash eventType, const VariantMap& eventArgs);
    void Register_Event(duk_context* ctx, duk_idx_t callback_idx, StringHash eventType);
    void Remove_Event(duk_context* ctx, duk_idx_t callback_idx, StringHash eventType);

    void Call_RegisterComponent(duk_context* context, duk_idx_t ctor_idx, const char* typeName);
    void Console_Print(duk_context* ctx, unsigned argc, LogLevel logLvl);
    void Wrap_Object(duk_context* ctx, Object* instance);
    void Setup_Primitive(duk_context* ctx, duk_idx_t obj_idx, const char* primitiveTypeName);
    duk_ret_t Object_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
    duk_ret_t Object_Finalizer(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
}
