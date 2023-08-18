using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/ParticleEmitter2D.h")]
    public class ParticleEmitter2D : Drawable2D
    {
        [PropertyMap("GetEffect", "SetEffect")]
        public ParticleEffect2D Effect { get => new ParticleEffect2D(); set { } }
        [PropertyMap("GetSprite", "SetSprite")]
        public Sprite2D Sprite { get => new Sprite2D(); set { } }
        [PropertyMap("GetBlendMode", "SetBlendMode")]
        public BlendMode BlendMode { get; set; }
        [PropertyMap("GetMaxParticles", "SetMaxParticles")]
        public uint MaxParticles { get; set; }
        [PropertyMap("IsEmitting", "SetEmitting")]
        public bool IsEmitting { get; set; }
        [PropertyMap("GetParticleEffectAttr", "SetParticleEffectAttr")]
        public ResourceRef ParticleEffectAttr { get => new ResourceRef(); set { } }
        [PropertyMap("GetSpriteAttr", "SetSpriteAttr")]
        public ResourceRef SpriteAttr { get => new ResourceRef(); set { } }

        public ParticleEmitter2D() : base(typeof(ParticleEmitter2D)) { }
    }
}
