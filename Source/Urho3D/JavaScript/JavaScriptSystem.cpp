#include "JavaScriptSystem.h"
#include "JavaScriptAsset.h"
#include "../Core/Context.h"
#include "JavaScriptBindings.h"

#define JAVASCRIPT_REFLECTION_CATEGORY "Scripting"
namespace Urho3D
{
    JavaScriptSystem::JavaScriptSystem(Context* context) :
        Object(context),
        dukCtx_(nullptr){}
    JavaScriptSystem::~JavaScriptSystem()
    {
        ForceStop();
    }

    void JavaScriptSystem::Run(const JavaScriptAsset* asset) {
        Run(asset->GetSource());
    }
    void JavaScriptSystem::Run(const ea::string& jsCode) {
        Init();
        duk_eval_string_noresult(dukCtx_, jsCode.c_str());
    }

    void JavaScriptSystem::RegisterObject(Context* context)
    {
        context->RegisterSubsystem<JavaScriptSystem>();
        context->AddFactoryReflection<JavaScriptAsset>(JAVASCRIPT_REFLECTION_CATEGORY);
    }

    void JavaScriptSystem::Init() {
        if (dukCtx_)
            return;
        dukCtx_ = duk_create_heap(
            nullptr,
            nullptr,
            nullptr,
            this,
            HandleFatalError
        );

        JavaScript_SetupBindings(dukCtx_);
    }
    void JavaScriptSystem::ForceStop()
    {
        if (dukCtx_)
            duk_destroy_heap(dukCtx_);
        dukCtx_ = nullptr;
    }
    void JavaScriptSystem::HandleFatalError(void* udata, const char* msg)
    {
        ea::string errMsg(msg == nullptr ? "No message" : msg);
        URHO3D_LOGERROR("**FATAL ERROR** {}", errMsg);
        
        JavaScriptSystem* sys = static_cast<JavaScriptSystem*>(udata);
        if (sys)
            sys->ForceStop();
    }
}
