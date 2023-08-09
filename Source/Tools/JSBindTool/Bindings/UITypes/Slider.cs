using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/Slider.h")]
    public class Slider : BorderImage
    {
        [PropertyMap("GetOrientation", "SetOrientation")]
        public Orientation Orientation { get; set; }
        [PropertyMap("GetRange", "SetRange")]
        public float Range { get; set; }
        [PropertyMap("GetValue", "SetValue")]
        public float Value { get; set; }
        [PropertyMap("GetRepeatRate", "SetRepeatRate")]
        public float RepeatRate { get; set; }
        [PropertyMap("GetKnob")]
        public BorderImage Knob { get => new BorderImage(); }

        public Slider() : base(typeof(Slider)) { }

        [Method]
        public void ChangeValue(float delta) { }
    }   
}
