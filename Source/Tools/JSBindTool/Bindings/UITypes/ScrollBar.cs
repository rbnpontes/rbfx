using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/ScrollBar.h")]
    public class ScrollBar : BorderImage
    {
        [PropertyMap("GetOrientation", "SetOrientation")]
        public Orientation Orientation { get; set; }
        [PropertyMap("GetRange", "SetRange")]
        public float Range { get; set; }
        [PropertyMap("GetValue", "SetValue")]
        public float Value { get; set; }
        [PropertyMap("GetScrollStep", "SetScrollStep")]
        public float ScrollStep { get; set; }
        [PropertyMap("GetStepFactor", "SetStepFactor")]
        public float StepFactor { get; }
        [PropertyMap("GetEffectiveScrollStep")]
        public float EffectiveScrollStep { get; }
        [PropertyMap("GetBackButton")]
        public Button BackButton { get => new Button(); }
        [PropertyMap("GetForwardButton")]
        public Button ForwardButton { get => new Button(); }
        [PropertyMap("GetSlider")]
        public Slider Slider { get => new Slider(); }
        public ScrollBar() : base(typeof(ScrollBar)) { }

        [Method]
        public void ChangeValue(float delta) { }
        [Method]
        public void StepBack() { }
        [Method]
        public void StepForward() { }
    }   
}
