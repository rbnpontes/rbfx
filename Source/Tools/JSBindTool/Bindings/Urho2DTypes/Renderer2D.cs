using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/Renderer2D.h")]
    public class Renderer2D : Drawable
    {
        public Renderer2D() : base(typeof(Renderer2D)) { }

        [Method]
        public void AddDrawable(Drawable2D drawable) { }
        [Method]
        public void RemoveDrawable(Drawable2D drawable) { }
        [Method]
        public Material GetMaterial(Texture2D texture, BlendMode blendMode) => new Material();
        [Method]
        public bool CheckVisibility(Drawable2D drawable) => false;
    }
}
