using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/SpriteSheet2D.h")]
    public class SpriteSheet2D : Resource
    {
        [PropertyMap("GetTexture", "SetTexture")]
        public Texture2D Texture { get => new Texture2D(); set { } }
        [PropertyMap("GetSpriteMapping")]
        public StringMap<SharedPtr<Sprite2D>> SpriteMapping { get => new StringMap<SharedPtr<Sprite2D>>(); }

        public SpriteSheet2D() : base(typeof(SpriteSheet2D)) { }
        public SpriteSheet2D(Type type) : base(type)
        {
            ValidateInheritance<SpriteSheet2D>();
        }

        [Method]
        public void DefineSprite(string name, IntRect rectangle) { }
        [Method]
        public void DefineSprite(string name, IntRect rectangle, Vector2 hotSpot) { }
        [Method]
        public void DefineSprite(string name, IntRect rectangle, Vector2 hotSpot, IntVector2 offset) { }
        [Method]
        public Sprite2D GetSprite(string name) => new Sprite2D();
    }
}
