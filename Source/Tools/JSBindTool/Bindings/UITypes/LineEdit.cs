using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/LineEdit.h")]
    public class LineEdit: BorderImage
    {
        [PropertyMap("GetText", "SetText")]
        public string Text { get; set; } = string.Empty;
        [PropertyMap("GetCursorPosition", "SetCursorPosition")]
        public uint CursorPosition { get; set; }
        [PropertyMap("GetCursorBlinkRate", "SetCursorBlinkRate")]
        public uint CursorBlinkRate { get; set; }
        [PropertyMap("GetMaxLength", "SetMaxLength")]
        public uint MaxLength { get; set; }
        [PropertyMap("GetEchoCharacter", "SetEchoCharacter")]
        public uint EchoCharacter { get; set; }
        [PropertyMap("IsCursorMovable", "SetCursorMovable")]
        public bool IsCursorMovable { get; set; }
        [PropertyMap("IsTextSelectable", "SetTextSelectable")]
        public bool IsTextSelectable { get; set; }
        [PropertyMap("IsTextCopyable", "SetTextCopyable")]
        public bool IsTextCopyable { get; set; }
        [PropertyMap("GetTextElement")]
        public Text TextElement { get => new Text(); }
        [PropertyMap("GetCursor")]
        public BorderImage Cursor { get => new BorderImage(); }

        public LineEdit() : base(typeof(LineEdit)) { }
    }
}
