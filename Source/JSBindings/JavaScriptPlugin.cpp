#include "JavaScriptPlugin.h"
#include "Bindings.h"
#include <Urho3D/JavaScript/JavaScriptSystem.h>
extern "C" URHO3D_EXPORT_API PluginApplication * PluginApplicationMain(Context * context)
{
    PluginApplication* app = new JavaScriptPlugin(context);
    app->SetPluginName("JavaScriptBindingsPlugin");
    return app;
}
JavaScriptPlugin::JavaScriptPlugin(Context* context) : MainPluginApplication(context)
{
    JavaScriptSystem* system = GetSubsystem<JavaScriptSystem>();
    jsbind_bindings_setup(static_cast<duk_context*>(system->GetJSCtx()));
    URHO3D_LOGDEBUG("JavaScript Bindings has been initialized.");
}
