using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/Drawable2D.h")]
    [Namespace(Constants.ProjectName)]
    public class Urho2DModule : ModuleObject
    {
        public Urho2DModule() : base(typeof(Urho2DModule)) { }

        [Field("pixelSize", "PIXEL_SIZE")]
        public static float PixelSize;
    }
}
