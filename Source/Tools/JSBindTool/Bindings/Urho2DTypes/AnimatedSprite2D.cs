using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/AnimatedSprite2D.h")]
    public class AnimatedSprite2D : StaticSprite2D
    {
        [PropertyMap("GetAnimationSet", "SetAnimationSet")]
        public AnimationSet2D AnimationSet { get => new AnimationSet2D(); set { } }
        [PropertyMap("GetEntity", "SetEntity")]
        public string Entity { get; set; } = string.Empty;
        [PropertyMap("GetAnimation", "SetAnimation")]
        public string Animation { get; set; } = string.Empty;
        [PropertyMap("GetLoopMode", "SetLoopMode")]
        public LoopMode2D LoopMode { get; set; }
        [PropertyMap("GetSpeed", "SetSpeed")]
        public float Speed { get; set; }
        [PropertyMap("GetAnimationSetAttr", "SetAnimationSetAttr")]
        public ResourceRef AnimationSetAttr { get => new ResourceRef(); set { } }

        public AnimatedSprite2D() : base(typeof(AnimatedSprite2D)) { }

        [Method]
        public void SetAnimation(string name, LoopMode2D loopMode) { }
    }
}
