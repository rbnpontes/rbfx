#include "JavaScriptSystem.h"
#include "JavaScriptAsset.h"
#include "../Core/Context.h"
#include "../Core/ObjectCategory.h"
#include "JavaScriptLogging.h"
#include "JavaScriptSystemBindings.h"
#include "JavaScriptEvents.h"
#include "JavaScriptOperations.h"
#include "JavaScriptProfiler.h"
#include "JavaScriptTimers.h"
#include "JavaScriptWorker.h"
#include "../Core/CoreEvents.h"

#include <duktape/duktape.h>
#include <duktape/duk_internal.h>

#if URHO3D_PROFILING
#include <tracy/Tracy.hpp>
#endif

namespace Urho3D
{
#if URHO3D_PROFILING
    void* js_alloc(void* udata, duk_size_t size)
    {
        void* data = duk_default_alloc_function(udata, size);
        TracyAlloc(data, size);
        return data;
    }
    void* js_realloc(void* udata, void* ptr, duk_size_t size)
    {
        TracyFree(ptr);
        void* data = duk_default_realloc_function(udata, ptr, size);
        TracyAlloc(data, size);
        return data;
    }
    void js_free(void* udata, void* ptr)
    {
        duk_default_free_function(udata, ptr);
        TracyFree(ptr);
    }
#endif
    duk_context* js_create_context(js_fatal_error_function fatal_error_fn, void* userdata = nullptr) {
                duk_alloc_function allocFn = nullptr;
        duk_realloc_function reallocFn = nullptr;
        duk_free_function freeFn = nullptr;

#if URHO3D_PROFILING
        allocFn = js_alloc;
        reallocFn = js_realloc;
        freeFn = js_free;
#endif

        duk_context* ctx = duk_create_heap(allocFn, reallocFn, freeFn, userdata, fatal_error_fn);

        duk_push_global_stash(ctx);
        // heapptr table
        duk_push_object(ctx);
        duk_put_prop_string(ctx, -2, JS_OBJECT_HEAPPTR_PROP);
        // strong ref table
        duk_push_object(ctx);
        duk_put_prop_string(ctx, -2, JS_PROP_STRONG_REFS);
        duk_pop(ctx);

        js_setup_logger(ctx);
        js_setup_sys_bindings(ctx);
        js_setup_profiler_bindings(ctx);
        js_setup_timer_bindings(ctx);
        js_setup_worker_bindings(ctx);

        return ctx;
    }

    JavaScriptSystem* JavaScriptSystem::instance_ = nullptr;
    JavaScriptSystem::JavaScriptSystem(Context* context) :
        Object(context),
        dukCtx_(nullptr)
    {
        SubscribeToEvent(E_UPDATE, URHO3D_HANDLER(JavaScriptSystem, HandleUpdate));
    }
    JavaScriptSystem::~JavaScriptSystem()
    {
    }

    void JavaScriptSystem::Initialize() {
        if (instance_)
            return;

        SetupJS();
        instance_ = this;
    }
    void JavaScriptSystem::Run(const JavaScriptAsset* asset) {
        Run(asset->GetSource());
    }
    void JavaScriptSystem::Run(const ea::string& jsCode) {
        URHO3D_ASSERTLOG(instance_, "must initialize JavaScriptSystem first.");
        instance_->RunCode(jsCode);
    }
    void JavaScriptSystem::ReleaseHeapptr(void* heapptr) {
        if (!heapptr)
            return;
        URHO3D_ASSERTLOG(instance_, "must initialize JavaScriptSystem first.");
        duk_context* ctx = static_cast<duk_context*>(instance_->dukCtx_);
        if (rbfx_is_valid_heapptr(ctx, heapptr))
            rbfx_unlock_heapptr(ctx, heapptr);
    }

    void* JavaScriptSystem::CreateThreadContext(js_fatal_error_function fatalErrFn, void* userdata) {
        return js_create_context(fatalErrFn, userdata);
    }

    void JavaScriptSystem::Stop() {
        URHO3D_ASSERTLOG(instance_, "must initializes JavaScriptSystem first.");

        js_clear_internal_timers(static_cast<duk_context*>(instance_->dukCtx_));
        instance_->StopJS();
    }
    Context* JavaScriptSystem::GetContext() {
        URHO3D_ASSERTLOG(instance_, "must initializes JavaScriptSystem first.");
        return instance_->context_;
    }
    void* JavaScriptSystem::GetJSCtx()
    {
        URHO3D_ASSERTLOG(instance_, "must initializes JavaScriptSystem first.");
        return GetDukCtx();
    }
    void JavaScriptSystem::RegisterObject(Context* context)
    {
        context->RegisterSubsystem<JavaScriptSystem>();
        context->AddFactoryReflection<JavaScriptAsset>();
    }
    void JavaScriptSystem::HandleFatalError(void* udata, const char* msg)
    {
        URHO3D_ASSERTLOG(instance_, "must initializes JavaScriptSystem first.");
        ea::string errMsg(msg == nullptr ? "no message" : msg);
        URHO3D_LOGERROR("**FATAL ERROR** {}", errMsg);

        instance_->StopJS();
    }
    void JavaScriptSystem::SetupJS()
    {
        if (dukCtx_) return;

        dukCtx_ = js_create_context(HandleFatalError);

        VariantMap args;
        args[JavaScriptSetup::P_DUKCTX] = dukCtx_;
        SendEvent(E_JAVASCRIPT_SETUP, args);
    }
    void JavaScriptSystem::RunCode(const ea::string& jsCode)
    {
        if (!dukCtx_)
            SetupJS();
        duk_context* ctx = static_cast<duk_context*>(dukCtx_);
        duk_eval_string_noresult(ctx, jsCode.c_str());
    }
    void JavaScriptSystem::HandleUpdate(StringHash eventType, const VariantMap& eventArgs)
    {
        URHO3D_PROFILE("JavaScriptSystem->HandleUpdate");
        duk_context* ctx = static_cast<duk_context*>(dukCtx_);

        if (!ctx)
            return;

        js_resolve_timers(ctx);
    }
    void JavaScriptSystem::StopJS()
    {
        SendEvent(E_JAVASCRIPT_STOP);

        duk_destroy_heap(static_cast<duk_context*>(dukCtx_));
        dukCtx_ = nullptr;

        js_clear_all_timers();
    }
}
