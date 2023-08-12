using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.GraphicsTypes
{
    [Include("Urho3D/Graphics/Viewport.h")]
    public class Viewport : EngineObject
    {
        public Viewport() : base(typeof(Viewport)) { }
    }

}
