using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/Window.h")]
    public class Window : BorderImage
    {
        [PropertyMap("IsMovable", "SetMovable")]
        public bool IsMovable { get; set; }
        [PropertyMap("IsResizable", "SetResizable")]
        public bool IsResizable { get; set; }
        [PropertyMap("IsModal", "SetModal")]
        public bool IsModal { get; set; }
        [PropertyMap("GetFixedWidthResizing", "SetFixedWidthResizing")]
        public bool FixedWidthResizing { get; set; }
        [PropertyMap("GetFixedHeightResizing", "SetFixedHeightResizing")]
        public bool FixedHeightResizing { get; set; }
        [PropertyMap("GetResizeBorder", "SetResizeBorder")]
        public IntRect ResizeBorder { get => new IntRect(); set { } }
        [PropertyMap("GetModalShadeColor", "SetModalFrameColor")]
        public Color ModalShadeColor { get => new Color(); set { } }
        [PropertyMap("GetModalFrameColor", "SetModalFrameColor")]
        public Color ModalFrameColor { get => new Color(); set { } }
        [PropertyMap("GetModalFrameSize", "SetModalFrameSize")]
        public IntVector2 ModalFrameSize { get => new IntVector2(); set { } }
        [PropertyMap("GetModalAutoDismiss", "SetModalAutoDismiss")]
        public bool ModalAutoDismiss { get; set; }

        public Window() : base(typeof(Window)) { }
    }
}
