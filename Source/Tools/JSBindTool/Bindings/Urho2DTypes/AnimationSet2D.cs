using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.Urho2DTypes
{
    [Include("Urho3D/Urho2D/AnimationSet2D.h")]
    public class AnimationSet2D : Resource
    {
        [PropertyMap("GetNumAnimations")]
        public uint NumAnimations { get; }
        [PropertyMap("GetSprite")]
        public Sprite2D Sprite { get => new Sprite2D(); }

        public AnimationSet2D() : base(typeof(AnimationSet2D)) { }

        [Method]
        public string GetAnimation(uint index) => string.Empty;
        [Method]
        public bool HasAnimation(string animationName) => false;
        [Method]
        public Sprite2D GetSpriterFileSprite(int folderId, int fileId) => new Sprite2D();
    }
}
