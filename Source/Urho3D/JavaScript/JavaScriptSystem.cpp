#include "JavaScriptSystem.h"
#include "JavaScriptAsset.h"
#include "../Core/Context.h"
#include "JavaScriptBindings.h"
#include "../Core/ObjectCategory.h"

namespace Urho3D
{
    JavaScriptSystem::JavaScriptSystem(Context* context) :
        Object(context) {}
    JavaScriptSystem::~JavaScriptSystem()
    {
        JavaScriptBindings::Destroy();
    }

    void JavaScriptSystem::Run(const JavaScriptAsset* asset) {
        Run(asset->GetSource());
    }
    void JavaScriptSystem::Run(const ea::string& jsCode) {
        JavaScriptBindings::Setup(context_);
        JavaScriptBindings::RunCode(jsCode);
    }

    void JavaScriptSystem::RegisterObject(Context* context)
    {
        context->RegisterSubsystem<JavaScriptSystem>();
        context->AddFactoryReflection<JavaScriptAsset>(Category_JavaScript);
    }
}
