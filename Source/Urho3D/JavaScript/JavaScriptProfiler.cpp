#include "JavaScriptProfiler.h"
#include "../Math/StringHash.h"
#include "../IO/Log.h"
#include <tracy/TracyC.h>
#include <EASTL/vector.h>

namespace Urho3D
{
#if URHO3D_PROFILING
    static ea::unordered_map<StringHash, TracyCZoneCtx> g_zones;
#endif
    duk_idx_t js_console_profile_call(duk_context* ctx)
    {
#if URHO3D_PROFILING
        // insert tracy zone on unordered_map
        // profileEnd call will remove from map
        // and tracy will calculate the performance time
        // between these gaps.
        ea::string name = duk_require_string(ctx, 0);
        StringHash key = name;
        if (g_zones.contains(key))
        {
            URHO3D_LOGWARNING("A profile with name '{}' has been declared. Did you call console.profileEnd(name) ?", name);
            return 0;
        }

        TracyCZone(profile, true);
        // use custom color to identify JS calls
        TracyCZoneColor(profile, 0xFF0000FF);
        TracyCZoneName(profile, name.c_str(), name.size());
        g_zones[key] = profile;
#endif
        return 0;
    }
    duk_idx_t js_console_profile_end_call(duk_context* ctx)
    {
#if URHO3D_PROFILING
        // remove zone from map
        // and let tracy calculate performance
        // between profile and profileEnd
        ea::string name = duk_require_string(ctx, 0);

        auto profileIt = g_zones.find(name);
        if (profileIt == g_zones.end()) {
            URHO3D_LOGWARNING("This profile name '{}' has not been declared. Did you call console.profile(name) ?", name);
            return 0;
        }

        TracyCZoneCtx profile = profileIt->second;
        TracyCZoneEnd(profile);

        g_zones.erase(profileIt);
#endif
        return 0;
    }

    void js_setup_profiler_bindings(duk_context* ctx)
    {
        duk_get_global_string(ctx, "console");

        duk_push_c_lightfunc(ctx, js_console_profile_call, 1, 1, 0);
        duk_put_prop_string(ctx, -2, "profile");

        duk_push_c_lightfunc(ctx, js_console_profile_end_call, 1, 1, 0);
        duk_put_prop_string(ctx, -2, "profileEnd");

        duk_pop(ctx);
    }
}
