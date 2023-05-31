#include "JavaScriptSystem.h"
#include "JavaScriptAsset.h"
#include "../Core/Context.h"
#include "../Core/ObjectCategory.h"
#include "JavaScriptLogging.h"
#include "JavaScriptEvents.h"

#include <duktape/duktape.h>

namespace Urho3D
{
    JavaScriptSystem* JavaScriptSystem::instance_ = nullptr;
    JavaScriptSystem::JavaScriptSystem(Context* context) :
        Object(context),
        dukCtx_(nullptr) {}
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

        // Add wrap method with exception treatment
        ea::string scopedCode = "(function() {\n";
        scopedCode += "try {\n";
        scopedCode += jsCode;
        scopedCode += "} catch (e) { console.error(e); }\n";
        scopedCode += "})();";

        instance_->RunCode(scopedCode);
    }

    void JavaScriptSystem::Stop() {
        URHO3D_ASSERTLOG(instance_, "must initializes JavaScriptSystem first.");
        instance_->StopJS();
    }

    void JavaScriptSystem::RegisterObject(Context* context)
    {
        context->RegisterSubsystem<JavaScriptSystem>();
        context->AddFactoryReflection<JavaScriptAsset>();
    }
    void JavaScriptSystem::HandleFatalError(void* udata, const char* msg)
    {
        ea::string errMsg(msg == nullptr ? "No message" : msg);
        URHO3D_LOGERROR("**FATAL ERROR** {}", errMsg);
        instance_->StopJS();
    }
    void JavaScriptSystem::SetupJS()
    {
        if (dukCtx_) return;

        duk_context* ctx = duk_create_heap(nullptr, nullptr, nullptr, nullptr, HandleFatalError);
        dukCtx_ = ctx;

        JavaScript_SetupLogger(ctx);

        VariantMap args;
        args[JavaScriptSetup::P_DUKCTX] = ctx;
        SendEvent(E_JAVASCRIPT_SETUP, args);
    }
    void JavaScriptSystem::RunCode(const ea::string& jsCode)
    {
        if (!dukCtx_)
            SetupJS();
        duk_context* ctx = static_cast<duk_context*>(dukCtx_);
        duk_eval_string_noresult(ctx, jsCode.c_str());
    }
    void JavaScriptSystem::StopJS()
    {
        SendEvent(E_JAVASCRIPT_STOP);

        duk_destroy_heap(static_cast<duk_context*>(dukCtx_));
        dukCtx_ = nullptr;
    }
}
