using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/Button.h")]
    public class Button : BorderImage
    {
        [PropertyMap("GetPressedOffset", "SetPressedOffset")]
        public IntVector2 PressedOffset { get => new IntVector2(); set { } }
        [PropertyMap("GetPressedChildOffset", "SetPressedChildOffset")]
        public IntVector2 PressedChildOffset { get => new IntVector2(); set { } }
        [PropertyMap("GetRepeatDelay", "SetRepeatDelay")]
        public float RepeatDelay { get => 0f; }
        [PropertyMap("GetRepeatRate", "SetRepeatRate")]
        public float RepeatRate { get => 0f; }
        [PropertyMap("IsPressed")]
        public bool IsPressed { get => false; }

        public Button() : base(typeof(Button)) { }
        public Button(Type type) : base(type) { }

        [Method]
        public void SetPressedOffset(int x, int y) { }
        [Method]
        public void SetRepeat(float delay, float rate) { }
    }
}
