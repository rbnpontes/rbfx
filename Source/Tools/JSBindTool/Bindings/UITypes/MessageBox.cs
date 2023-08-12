using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/MessageBox.h")]
    public class MessageBox : EngineObject
    {
        [PropertyMap("GetTitle", "SetTitle")]
        public string Text { get; set; } = string.Empty;
        [PropertyMap("GetMessage", "SetMessage")]
        public string Message { get; set; } = string.Empty;
        [PropertyMap("GetWindow")]
        public UIElement Window { get => new UIElement(); }

        public MessageBox() : base(typeof(MessageBox)) { }
    }   
}
