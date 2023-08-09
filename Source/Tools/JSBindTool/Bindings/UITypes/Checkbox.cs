using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/CheckBox.h")]
    public class CheckBox : BorderImage
    {
        [PropertyMap("IsChecked", "SetChecked")]
        public bool IsChecked { get; set; }
        [PropertyMap("GetCheckedOffset", "SetCheckedOffset")]
        public IntVector2 CheckedOffset { get => new IntVector2(); set { } }

        public CheckBox() : base(typeof(CheckBox)) { }
        public CheckBox(Type type) : base(type) { }

        [Method]
        public void SetCheckedOffset(int x, int y) { }
    }
}
