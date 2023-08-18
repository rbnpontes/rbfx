using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/Drawable2D.h")]
    [Abstract]
    public class Drawable2D : Drawable
    {
        [PropertyMap("GetLayer", "SetLayer")]
        public int Layer { get; set; }
        [PropertyMap("GetOrderInLayer", "SetOrderInLayer")]
        public int OrderInLayer { get; set; }

        public Drawable2D() : base(typeof(Drawable2D)) { }
        public Drawable2D(Type type) : base(type)
        {
            ValidateInheritance<Drawable2D>();
        }
    }
}
