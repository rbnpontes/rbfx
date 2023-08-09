using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/Menu.h")]
    public class Menu : Button
    {
        [PropertyMap("GetPopup", "SetPopup")]
        public UIElement Popup { get => new UIElement(); set { } }
        [PropertyMap("GetPopupOffset", "SetPopupOffset")]
        public IntVector2 PopupOffset { get => new IntVector2(); set { } }
        [PropertyMap("GetAcceleratorKey")]
        public int AcceleratorKey { get; }
        [PropertyMap("GetAcceleratorQualifiers")]
        public int AcceleratorQualifiers { get; }

        public Menu() : base(typeof(Menu)){ }
        public Menu(Type type) : base(type) { }

        [Method]
        public void OnShowPopup() { }
        [Method]
        public void OnHidePopup() { }
        [Method]
        public void SetPopupOffset(int x, int y) { }
        [Method]
        public void ShowPopup(bool enable) { }
        [Method]
        public void SetAccelerator(int key, int qualifiers) { }
        [Method]
        public bool GetShowPopup() => false;
    }
}
