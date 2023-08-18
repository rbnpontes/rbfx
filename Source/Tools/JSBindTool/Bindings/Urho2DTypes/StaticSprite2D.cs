using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/StaticSprite2D.h")]
    public class StaticSprite2D : Drawable2D
    {
        [PropertyMap("GetSprite", "SetSprite")]
        public Sprite2D Sprite { get => new Sprite2D(); set { } }
        [PropertyMap("GetDrawRect", "SetDrawRect")]
        public Rect DrawRect { get => new Rect(); set { } }
        [PropertyMap("GetTextureRect", "SetTextureRect")]
        public Rect TextureRect { get => new Rect(); set { } }
        [PropertyMap("GetBlendMode", "SetBlendMode")]
        public BlendMode BlendMode { get; set; }
        [PropertyMap("GetFlipX", "SetFlipX")]
        public bool FlipX { get; set; }
        [PropertyMap("GetFlipY", "SetFlipY")]
        public bool FlipY { get; set; }
        [PropertyMap("GetSwapXY", "SetSwapXY")]
        public bool SwapXY { get; set; }
        [PropertyMap("GetColor", "SetColor")]
        public Color Color { get => new Color(); set { } }
        [PropertyMap("GetAlpha", "SetAlpha")]
        public float Alpha { get; set; }
        [PropertyMap("GetUseHotSpot", "SetUseHotSpot")]
        public bool UseHotSpot { get; set; }
        [PropertyMap("GetUseDrawRect", "SetUseDrawRect")]
        public bool UseDrawRect { get; set; }
        [PropertyMap("GetUseTextureRect", "SetUseTextureRect")]
        public bool UseTextureRect { get; set; }
        [PropertyMap("GetHotSpot", "SetHotSpot")]
        public Vector2 HotSpot { get => new Vector2(); set { } }
        [PropertyMap("GetCustomMaterial", "SetCustomMaterial")]
        public Material CustomMaterial { get => new Material(); set { } }
        [PropertyMap("GetSpriteAttr", "SetSpriteAttr")]
        public ResourceRef SpriteAttr { get => new ResourceRef(); set { } }
        [PropertyMap("GetCustomMaterialAttr", "SetCustomMaterialAttr")]
        public ResourceRef CustomMaterialAttr { get => new ResourceRef(); set { } }


        public StaticSprite2D() : base(typeof(StaticSprite2D)) { }
        public StaticSprite2D(Type type) : base(type)
        {
            ValidateInheritance<StaticSprite2D>();
        }

        [Method]
        public void SetFlip(bool flipX, bool flipY) { }
        [Method]
        public void SetFlip(bool flipX, bool flipY, bool swapXY) { }
    }
}
