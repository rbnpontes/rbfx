using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.SceneTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/UIComponent.h")]
    [Abstract]
    public class UIComponent : Component
    {
        [PropertyMap("GetRoot")]
        public UIElement Root { get => new UIElement(); }
        [PropertyMap("GetMaterial")]
        public Material Material { get => new Material(); }
        [PropertyMap("GetTexture")]
        public Texture2D Texture { get => new Texture2D(); }

        public UIComponent() : base(typeof(UIComponent)) { }

        [Method]
        public void SetViewportIndex(uint index) { }
    }
}
