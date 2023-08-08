using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/Button.h")]
    public class Button : BorderImage
    {
        public Button() : base(typeof(Button)) { }
    }
}
