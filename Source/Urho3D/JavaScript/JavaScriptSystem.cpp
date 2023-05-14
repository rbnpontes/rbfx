#include "JavaScriptSystem.h"
#include "JavaScriptAsset.h"
#include "../Core/Context.h"

#define JAVASCRIPT_REFLECTION_CATEGORY "Scripting"
namespace Urho3D
{
    JavaScriptSystem::JavaScriptSystem(Context* context) :
        Object(context),
        dukCtx_(nullptr){}
    JavaScriptSystem::~JavaScriptSystem()
    {
        duk_destroy_heap(dukCtx_);
        dukCtx_ = nullptr;
    }

    void JavaScriptSystem::Run(const JavaScriptAsset* asset) {
        Run(asset->GetSource());
    }
    void JavaScriptSystem::Run(const ea::string& jsCode) {

    }

    void JavaScriptSystem::RegisterObject(Context* context)
    {
        context->RegisterSubsystem<JavaScriptSystem>();
        context->AddFactoryReflection<JavaScriptAsset>(JAVASCRIPT_REFLECTION_CATEGORY);
    }
}
