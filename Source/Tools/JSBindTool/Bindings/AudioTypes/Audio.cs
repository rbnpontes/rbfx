using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.AudioTypes
{
    [Include("Urho3D/Audio/Sound.h")]
    public class Sound : ResourceWithMetadata
    {
        public Sound() : base(typeof(Sound)) { }
    }
}
