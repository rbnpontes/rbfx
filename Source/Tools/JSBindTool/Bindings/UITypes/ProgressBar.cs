using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/ProgressBar.h")]
    public class ProgressBar : BorderImage
    {
        [PropertyMap("GetOrientation", "SetOrientation")]
        public Orientation Orientation { get; set; }
        [PropertyMap("GetRange", "SetRange")]
        public float Range { get; set; }
        [PropertyMap("GetValue", "SetValue")]
        public float Value { get; set; }
        [PropertyMap("GetKnob")]
        public BorderImage Knob { get => new BorderImage(); }
        [PropertyMap("GetLoadingPercentStyle", "SetLoadingPercentStyle")]
        public string LoadingPercentStyle { get; set; } = string.Empty;
        [PropertyMap("GetShowPercentText", "SetShowPercentText")]
        public bool ShowPercentText { get; set; }

        public ProgressBar() : base(typeof(ProgressBar)) { }

        [Method]
        public void ChangeValue(float delta) { }
    }   
}
