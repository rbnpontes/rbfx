using JSBindTool.Bindings.SceneTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.GraphicsTypes
{
    [Include("Urho3D/Graphics/Drawable.h")]
    [Abstract]
    public class Drawable : Component, IPipelineStateTracker
    {
        [PropertyMap("GetPipelineStateHash")]
        public uint PipelineStateHash { get; set; }

        public Drawable() : base(typeof(Drawable)) { }
        public Drawable(Type type) : base(type) { }

        [Method]
        public void MarkPipelineStateHashDirty() { }
    }
}
