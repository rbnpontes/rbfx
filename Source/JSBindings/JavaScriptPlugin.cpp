#include "JavaScriptPlugin.h"

extern "C" URHO3D_EXPORT_API PluginApplication * PluginApplicationMain(Context * context)
{
    PluginApplication* app = new JavaScriptPlugin(context);
    app->SetPluginName("JavaScriptBindingsPlugin");
    return app;
}
JavaScriptPlugin::JavaScriptPlugin(Context* context) : MainPluginApplication(context){}
