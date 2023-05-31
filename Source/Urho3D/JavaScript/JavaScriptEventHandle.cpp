#include "JavaScriptEventHandle.h"
#include "JavaScriptBindingUtils.h"

namespace Urho3D
{
    JavaScriptEventHandle::JavaScriptEventHandle(Context* context) : Object(context),
        id_(0u),
        connectedEvents_(0u)
    {

    }

    void JavaScriptEventHandle::Connect(StringHash eventType)
    {
        ++connectedEvents_;
        SubscribeToEvent(eventType, URHO3D_HANDLER(JavaScriptEventHandle, HandleEventCall));
    }
    void JavaScriptEventHandle::Disconnect(StringHash eventType)
    {
        --connectedEvents_;
        UnsubscribeFromEvent(eventType);
    }
    void JavaScriptEventHandle::HandleEventCall(StringHash eventType, const VariantMap& eventArgs)
    {
        duk_context* ctx = JavaScriptBindings::GetJSCtx();

        duk_idx_t globalStashIdx = duk_get_top(ctx);
        duk_push_global_stash(ctx);
        duk_get_prop_index(ctx, -1, id_);

        if (duk_is_null_or_undefined(ctx, -1)) {
            URHO3D_LOGWARNING("JavaScript Event Handle #{:d} does not exists.", id_);
            duk_pop_n(ctx, 2);
            return;
        }

#ifdef URHO3D_DEBUG
        if (!duk_is_function(ctx, -1)) {
            URHO3D_LOGWARNING("JavaScript Event Handle #{:d} entry is not a function, this should never occurs. Skipping!!!", id_);
            duk_pop_n(ctx, 2);
            return;
        }
#endif

        // Setup Args
        duk_push_uint(ctx, eventType.Value());
        push_variant(ctx, eventArgs);

        duk_call(ctx, 2);

        // pop elements until heap top is equal to stash idx
        while (globalStashIdx != duk_get_top(ctx)) {
            duk_pop(ctx);
        }
    }
}
