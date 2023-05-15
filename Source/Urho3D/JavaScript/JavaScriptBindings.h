#include <duktape/duktape.h>
#include "../JavaScript/JavaScriptSystem.h"
#include <EASTL/functional.h>
namespace Urho3D
{
    void JavaScript_SetupBindings(duk_context* ctx, JavaScriptSystem* system);
}
