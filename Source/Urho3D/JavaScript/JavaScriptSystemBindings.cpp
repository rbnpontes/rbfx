#include "JavaScriptSystemBindings.h"
#include "JavaScriptOperations.h"
#include "JavaScriptSystem.h"
#include "JavaScriptComponent.h"
#include "../Core/Context.h"

#define JS_HIDDEN_COMPONENT_TYPE DUK_HIDDEN_SYMBOL("__type")
#define JS_HIDDEN_COMPONENT_CTOR DUK_HIDDEN_SYMBOL("__ctor")
#define JS_OBJ_FACTORIES_PROP "__factories"

namespace Urho3D
{
    duk_ret_t js_get_subsystem_call(duk_context* ctx)
    {
        StringHash type = rbfx_require_string_hash(ctx, 0);
        Object* obj = JavaScriptSystem::GetContext()->GetSubsystem(type);
        rbfx_push_object(ctx, obj);
        return 1;
    }

    void js_setup_sys_bindings(duk_context* ctx)
    {
        duk_push_object(ctx);

        duk_push_c_lightfunc(ctx, js_get_subsystem_call, 2, 2, 0);
        duk_put_prop_string(ctx, -2, "getSubsystem");

        duk_put_global_string(ctx, "rbfx");

        // setup factories stash table
        duk_push_global_stash(ctx);
        duk_push_object(ctx);
        duk_put_prop_string(ctx, -2, JS_OBJ_FACTORIES_PROP);

        duk_pop(ctx);
    }
}
