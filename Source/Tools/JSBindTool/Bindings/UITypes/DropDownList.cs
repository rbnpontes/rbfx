using JSBindTool.Core;
using JSBindTool.Core.Annotations;
namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/DropDownList.h")]
    public class DropDownList : Menu
    {
        [PropertyMap("GetSelection", "SetSelection")]
        public uint Selection { get; set; }
        [PropertyMap("GetPlaceholderText", "SetPlaceholderText")]
        public string PlaceholderText { get; set; } = string.Empty;
        [PropertyMap("GetResizePopup", "SetResizePopup")]
        public bool ResizePopup { get; set; }
        [PropertyMap("GetNumItems")]
        public uint NumItems { get; }
        [PropertyMap("GetItems")]
        public Vector<UIElement> Items { get => new Vector<UIElement>(); }
        [PropertyMap("GetSelectedItem")]
        public UIElement SelectedItem { get => new UIElement(); }
        [PropertyMap("GetListView")]
        public ListView ListView { get => new ListView(); }
        [PropertyMap("GetPlaceholder")]
        public UIElement Placeholder { get => new UIElement(); }

        public DropDownList() : base(typeof(DropDownList)) { }
        public DropDownList(Type type) : base(type) { }

        [Method]
        public void AddItem(UIElement item) { }
        [Method]
        public void InsertItem(uint idx, UIElement item) { }
        [Method]
        public void RemoveItem(UIElement item) { }
        [Method]
        public void RemoveItem(uint idx) { }
        [Method]
        public void RemoveAllItems() { }
        [Method]
        public UIElement GetItem(uint idx) => new UIElement();
        [Method]
        public void SetSelectionAttr(uint idx) { }
    }
}
