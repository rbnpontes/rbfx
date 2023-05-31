#include "JavaScriptDummy.h"

namespace Urho3D
{
    TypeInfo* JavaScriptDummy::currentType_ = nullptr;
    JavaScriptDummy::JavaScriptDummy(Context* context) : Object(context){}

    void JavaScriptDummy::SetTypeInfoStatic(TypeInfo* type) {
        currentType_ = type;
    }
}
