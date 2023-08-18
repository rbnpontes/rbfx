using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/StretchableSprite2D.h")]
    public class StretchableSprite2D : StaticSprite2D
    {
        [PropertyMap("GetBorder", "SetBorder")]
        public IntRect Border { get => new IntRect(); set { } }
        public StretchableSprite2D() : base(typeof(StretchableSprite2D)) { }
    }
}
