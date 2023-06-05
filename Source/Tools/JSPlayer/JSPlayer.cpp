#include "JSPlayer.h"
#include <Urho3D/Resource/ResourceCache.h>
#include <Urho3D/JavaScript/JavaScriptSystem.h>
#include <Urho3D/JavaScript/JavaScriptAsset.h>
#include <Urho3D/Engine/EngineDefs.h>
#include <Urho3D/Input/Input.h>
#include <Urho3D/Plugins/PluginManager.h>
#include <Urho3D/Plugins/ModulePlugin.h>
#include <Urho3D/IO/FileSystem.h>

namespace Urho3D
{
    JSPlayer::JSPlayer(Context* context) : Application(context)
    {
        engineParameters_[EP_WINDOW_TITLE] = "rbfx - Javascript Player";
        engineParameters_[EP_APPLICATION_NAME] = "JSPlayer";
        engineParameters_[EP_WINDOW_RESIZABLE] = true;
        engineParameters_[EP_WINDOW_WIDTH] = 500;
        engineParameters_[EP_WINDOW_HEIGHT] = 400;
        engineParameters_[EP_BORDERLESS] = false;
        engineParameters_[EP_HEADLESS] = false;
        engineParameters_[EP_FULL_SCREEN] = false;
        engineParameters_[EP_WINDOW_MAXIMIZE] = false;
        engineParameters_[EP_LOG_NAME] = "conf://JSPlayer.log";
        engineParameters_[EP_RESOURCE_PATHS] = "CoreData;Data";
        engineParameters_[EP_RESOURCE_PREFIX_PATHS] = ";..;../..";
        engineParameters_[EP_HIGH_DPI] = true;
        engineParameters_[EP_SOUND] = true;
    }

    void JSPlayer::Setup() {
        URHO3D_LOGDEBUG("Setup JSPlayer");
        FileSystem* fs = context_->GetSubsystem<FileSystem>();

        PluginApplication::RegisterStaticPlugins();

    }
    void JSPlayer::Start() {
        URHO3D_LOGDEBUG("Starting JSPlayer");
        FileSystem* fs = context_->GetSubsystem<FileSystem>();

        ModulePlugin* jsBindingsPlugin = new ModulePlugin(context_);
        jsBindingsPlugin->SetName("JSBindings");

        auto pluginManager = GetSubsystem<PluginManager>();
        pluginManager->AddDynamicPlugin(jsBindingsPlugin);
        pluginManager->StartApplication();

        Input* input = GetSubsystem<Input>();
        input->SetMouseMode(MM_FREE);
        input->SetMouseVisible(true);

        ResourceCache* cache = GetSubsystem<ResourceCache>();
        SharedPtr<JavaScriptAsset> file(cache->GetResource<JavaScriptAsset>("Scripts/TestScript.js"));

        URHO3D_ASSERTLOG(file, "Script file was not found!");

        URHO3D_LOGDEBUG("Running Script.");
        GetSubsystem<JavaScriptSystem>()->Run(file);
    }
}
