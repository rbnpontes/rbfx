using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/View3D.h")]
    public class View3D : Window
    {
        [PropertyMap("GetFormat", "SetFormat")]
        public uint Format { get; set; }
        [PropertyMap("GetAutoUpdate", "SetAutoUpdate")]
        public bool AutoUpdate { get; set; }
        [PropertyMap("GetScene")]
        public Scene Scene { get => new Scene(); set { } }
        [PropertyMap("GetCameraNode")]
        public Node CameraNode { get => new Node(); }
        [PropertyMap("GetRenderTexture")]
        public Texture2D RenderTexture { get => new Texture2D(); }
        [PropertyMap("GetDepthTexture")]
        public Texture2D DepthTexture { get => new Texture2D(); }
        [PropertyMap("GetViewport")]
        public Viewport Viewport { get => new Viewport(); }

        public View3D() : base(typeof(View3D)) { }

        [Method]
        public void SetView(Scene scene, Camera camera) { }
        [Method]
        public void SetView(Scene scene, Camera camera, bool ownScene) { }
        [Method]
        public void QueueUpdate() { }
    }   
}
