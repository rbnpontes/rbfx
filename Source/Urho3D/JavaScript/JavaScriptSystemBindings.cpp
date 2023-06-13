#include "JavaScriptSystemBindings.h"
#include "JavaScriptOperations.h"
#include "JavaScriptSystem.h"
#include "../Core/Context.h"

namespace Urho3D
{
    duk_ret_t GetSubsystem_Call(duk_context* ctx)
    {
        StringHash type = rbfx_require_string_hash(ctx, 0);
        Object* obj = JavaScriptSystem::GetContext()->GetSubsystem(type);
        rbfx_push_object(ctx, obj);
        return 1;
    }

    void JavaScript_SetupSystemBindings(duk_context* ctx)
    {
        duk_push_object(ctx);

        duk_push_c_function(ctx, GetSubsystem_Call, 1);
        duk_put_prop_string(ctx, -2, "getSubsystem");

        duk_put_global_string(ctx, "rbfx");
    }
}
