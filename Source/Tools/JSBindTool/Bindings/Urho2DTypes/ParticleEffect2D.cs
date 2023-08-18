using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/ParticleEffect2D.h")]
    public class ParticleEffect2D : Resource
    {
        [PropertyMap("GetSprite", "SetSprite")]
        public Sprite2D Sprite { get => new Sprite2D(); set { } }
        [PropertyMap("GetSourcePositionVariance", "SetSourcePositionVariance")]
        public Vector2 SourcePositionVariance { get => new Vector2(); set { } }
        [PropertyMap("GetSpeed", "SetSpeed")]
        public float Speed { get; set; }
        [PropertyMap("GetSpeedVariance", "SetSpeedVariance")]
        public float SpeedVariance { get; set; }
        [PropertyMap("GetParticleLifeSpan", "SetParticleLifeSpan")]
        public float ParticleLifeSpan { get; set; }
        [PropertyMap("GetParticleLifespanVariance", "SetParticleLifespanVariance")]
        public float ParticleLifeSpanVariance { get; set; }
        [PropertyMap("GetAngle", "SetAngle")]
        public float Angle { get; set; }
        [PropertyMap("GetAngleVariance", "SetAngleVariance")]
        public float AngleVariance { get; set; }
        [PropertyMap("GetGravity", "SetGravity")]
        public Vector2 Gravity { get => new Vector2(); set { } }
        [PropertyMap("GetRadialAcceleration", "SetRadialAcceleration")]
        public float RadialAcceleration { get; set; }
        [PropertyMap("GetTangentialAcceleration", "SetTangentialAcceleration")]
        public float TangentialAcceleration { get; set; }
        [PropertyMap("GetRadialAccelVariance", "SetRadialAccelVariance")]
        public float RadialAccelVariance { get; set; }
        [PropertyMap("GetTangentialAccelVariance", "SetTangentialAccelVariance")]
        public float TangentialAccelVariance { get; set; }
        [PropertyMap("GetStartColor", "SetStartColor")]
        public Color StartColor { get => new Color(); set { } }
        [PropertyMap("GetStartColorVariance", "SetStartColorVariance")]
        public Color StartColorVariance { get => new Color(); set { } }
        [PropertyMap("GetFinishColor", "SetFinishColor")]
        public Color FinishColor { get => new Color(); set { } }
        [PropertyMap("GetFinishColorVariance", "SetFinishColorVariance")]
        public Color FinishColorVariance { get => new Color(); set { } }
        [PropertyMap("GetMaxParticles", "SetMaxParticles")]
        public int MaxParticles { get; set; }
        [PropertyMap("GetStartParticleSize", "SetStartParticleSize")]
        public float StartParticleSize { get; set; }
        [PropertyMap("GetStartParticleSizeVariance", "SetStartParticleSizeVariance")]
        public float StartParticleSizeVariance { get; set; }
        [PropertyMap("GetFinishParticleSize", "SetFinishParticleSize")]
        public float FinishParticleSize { get; set; }
        [PropertyMap("GetFinishParticleSizeVariance", "SetFinishParticleSizeVariance")]
        public float FinishParticleSizeVariance { get; set; }
        [PropertyMap("GetDuration", "SetDuration")]
        public float Duration { get; set; }
        [PropertyMap("GetEmitterType", "SetEmitterType")]
        public EmitterType2D EmitterType { get; set; }
        [PropertyMap("GetMaxRadius", "SetMaxRadius")]
        public float MaxRadius { get; set; }
        [PropertyMap("GetMaxRadiusVariance", "SetMaxRadiusVariance")]
        public float MaxRadiusVariance { get; set; }
        [PropertyMap("GetMinRadius", "SetMinRadius")]
        public float MinRadius { get; set; }
        [PropertyMap("GetMinRadiusVariance", "SetMinRadiusVariance")]
        public float MinRadiusVariance { get; set; }
        [PropertyMap("GetRotatePerSecond", "SetRotatePerSecond")]
        public float RotatePerSecond { get; set; }
        [PropertyMap("GetRotatePerSecondVariance", "SetRotatePerSecondVariance")]
        public float RotatePerSecondVariance { get; set; }
        [PropertyMap("GetBlendMode", "SetBlendMode")]
        public BlendMode BlendMode { get; set; }
        [PropertyMap("GetRotationStart", "SetRotationStart")]
        public float RotationStart { get; set; }
        [PropertyMap("GetRotationStartVariance", "SetRotationStartVariance")]
        public float RotationStartVariance { get; set; }
        [PropertyMap("GetRotationEnd", "SetRotationEnd")]
        public float RotationEnd { get; set; }
        [PropertyMap("GetRotationEndVariance", "SetRotationEndVariance")]
        public float RotationEndVariance { get; set; }

        public ParticleEffect2D() : base(typeof(ParticleEffect2D)) { }
        public ParticleEffect2D(Type type) : base(type)
        {
            ValidateInheritance<ParticleEffect2D>();
        }

        [Method]
        public SharedPtr<ParticleEffect2D> Clone() => new SharedPtr<ParticleEffect2D>();
        [Method]
        public SharedPtr<ParticleEffect2D> Clone(string name) => new SharedPtr<ParticleEffect2D>();
    }   
}
