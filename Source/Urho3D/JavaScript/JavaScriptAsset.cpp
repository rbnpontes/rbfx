#include "JavaScriptAsset.h"
#include "../IO/Deserializer.h"
#include "../IO/Serializer.h"
namespace Urho3D
{
    JavaScriptAsset::JavaScriptAsset(Context* context) : Resource(context) {}
    JavaScriptAsset::~JavaScriptAsset()
    {
    }
    bool JavaScriptAsset::BeginLoad(Deserializer& source)
    {
        sourceCode_ = "(function() {\ntry {\n";
        sourceCode_ += source.ReadString();
        sourceCode_ += "\n} catch(e) { console.error(e); }\n})();";
        return true;
    }
    bool JavaScriptAsset::Save(Serializer& desc) const
    {
        desc.WriteString(sourceCode_);
        return true;
    }
}
