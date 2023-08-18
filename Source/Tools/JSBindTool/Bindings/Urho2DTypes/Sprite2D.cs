using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/Sprite2D.h")]
    public class Sprite2D : Resource
    {
        [PropertyMap("GetTexture", "SetTexture")]
        public Texture2D Texture { get => new Texture2D(); set { } }
        [PropertyMap("GetRectangle", "SetRectangle")]
        public IntRect Rectangle { get => new IntRect(); set { } }
        [PropertyMap("GetHotSpot", "SetHotSpot")]
        public Vector2 HotSpot { get => new Vector2(); set { } }
        [PropertyMap("GetOffset", "SetOffset")]
        public IntVector2 Offset { get => new IntVector2(); set { } }
        [PropertyMap("GetSpriteSheet", "SetSpriteSheet")]
        public SpriteSheet2D SpriteSheet { get => new SpriteSheet2D(); set { } }
        [PropertyMap("GetTextureEdgeOffset")]
        public float TextureEdgeOffset { get; }

        public Sprite2D() : base(typeof(Sprite2D)) { }
        public Sprite2D(Type type) : base(type)
        {
            ValidateInheritance<Sprite2D>();
        }

        [Method]
        public bool GetDrawRectangle(Rect rect) => false;
        [Method]
        public bool GetDrawRectangle(Rect rect, bool flipX) => false;
        [Method]
        public bool GetDrawRectangle(Rect rect, bool flipX, bool flipY) => false;
        [Method]
        public bool GetDrawRectangle(Rect rect, Vector2 hotSpot) => false;
        [Method]
        public bool GetDrawRectangle(Rect rect, Vector2 hotSpot, bool flipX) => false;
        [Method]
        public bool GetDrawRectangle(Rect rect, Vector2 hotSpot, bool flipX, bool flipY) => false;
        [Method]
        public bool GetTextureRectangle(Rect rect) => false;
        [Method]
        public bool GetTextureRectangle(Rect rect, bool flipX) => false;
        [Method]
        public bool GetTextureRectangle(Rect rect, bool flipX, bool flipY) => false;

        [Method]
        public static ResourceRef SaveToResourceRef(Sprite2D sprite) => new ResourceRef();
        [Method]
        public static Sprite2D LoadFromResourceRef(EngineObjectRef obj, ResourceRef value) => new Sprite2D();
    }
}
