using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/Sprite.h")]
    public class Sprite : UIElement
    {
        [PropertyMap("GetPosition", "SetPosition")]
        public Vector2 Position { get => new Vector2(); set { } }
        [PropertyMap("GetHotSpot", "SetHotSpot")]
        public IntVector2 HotSpot { get => new IntVector2(); set { } }
        [PropertyMap("GetScale", "SetScale")]
        public Vector2 Scale { get => new Vector2(); set { } }
        [PropertyMap("GetRotation", "SetRotation")]
        public float Rotation { get; set; }
        [PropertyMap("GetTexture", "SetTexture")]
        public Texture Texture { get => new Texture(); set { } }
        [PropertyMap("GetImageRect", "SetImageRect")]
        public IntRect ImageRect { get => new IntRect(); set { } }
        [PropertyMap("GetBlendMode", "SetBlendMode")]
        public BlendMode BlendMode { get; set; }
        [PropertyMap("GetTextureAttr", "SetTextureAttr")]
        public ResourceRef TextureAttr { get => new ResourceRef(); set { } }
        [PropertyMap("GetTransformMatrix")]
        public Matrix3x4 TransformMatrix { get => new Matrix3x4(); }

        public Sprite() : base(typeof(Sprite)) { }

        [Method]
        public void SetPosition(float x, float y) { }
        [Method]
        public void SetScale(float x, float y) { }
        [Method]
        public void SetScale(float scale) { }
        [Method]
        public void SetFullImageRect() { }
    }
}
