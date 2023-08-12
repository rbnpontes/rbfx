using JSBindTool.Bindings.AudioTypes;
using JSBindTool.Bindings.EngineTypes;
using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/SplashScreen.h")]
    public class SplashScreen : ApplicationState
    {
        [PropertyMap("GetBackgroundImage", "SetBackgroundImage")]
        public Texture BackgroundImage { get => new Texture(); set { } }
        [PropertyMap("GetForegroundImage", "SetForegroundImage")]
        public Texture ForegroundImage { get => new Texture(); set { } }
        [PropertyMap("GetProgressImage", "SetProgressImage")]
        public Texture ProgressImage { get => new Texture(); set { } }
        [PropertyMap("GetProgressColor", "SetProgressColor")]
        public Color ProgressColor { get => new Color(); set { } }
        [PropertyMap("GetDuration", "SetDuration")]
        public float Duration { get; set; }
        [PropertyMap("IsSkippable", "SetSkippable")]
        public bool IsSkippable { get; set; }

        public SplashScreen() : base(typeof(SplashScreen)) { }

        [Method]
        public bool QueueSceneResourcesAsync(string fileName) => false;
        [Method]
        public bool QueueResource(StringHash type, string name) => false;
        [Method]
        public bool QueueResource(StringHash type, string name, bool sendEventOnFailure) => false;
        [Method]
        public void SetSound(Sound sound, float gain) { }
    }   
}
