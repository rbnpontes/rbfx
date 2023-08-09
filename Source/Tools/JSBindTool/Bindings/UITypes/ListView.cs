using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/ListView.h")]
    public enum HighlightMode
    {
        Never,
        Focus,
        Always
    }
    [Include("Urho3D/UI/ListView.h")]
    public class ListView : ScrollView
    {
        [PropertyMap("GetSelection", "SetSelection")]
        public uint Selection { get; set; }
        [PropertyMap("GetSelections", "SetSelections")]
        public Vector<uint> Selections { get => new Vector<uint>(); set { } }
        [PropertyMap("GetHighlightMode", "SetHighlightMode")]
        public HighlightMode HighlightMode { get; set; }
        [PropertyMap("GetMultiselect", "SetMultiselect")]
        public bool Multiselect { get; set; }
        [PropertyMap("GetHierarchyMode", "SetHierarchyMode")]
        public bool HierarchyMode { get; set; }
        [PropertyMap("GetBaseIndent", "SetBaseIndent")]
        public int BaseIndent { get; set; }
        [PropertyMap("GetClearSelectionOnDefocus", "SetClearSelectionOnDefocus")]
        public bool ClearSelectionOnDefocus { get; set; }
        [PropertyMap("GetSelectOnClickEnd", "SetSelectOnClickEnd")]
        public bool SelectOnClickEnd { get; set; }
        [PropertyMap("GetNumItems")]
        public uint NumItems { get; }
        [PropertyMap("GetItems")]
        public Vector<UIElement> Items { get => new Vector<UIElement>(); }
        [PropertyMap("GetSelectedItem")]
        public UIElement SelectedItem { get => new UIElement(); }
        [PropertyMap("GetSelectedItems")]
        public Vector<UIElement> SelectedItems { get => new Vector<UIElement>(); }

        public ListView() : base(typeof(ListView)) { }

        [Method]
        public void UpdateInternalLayout() { }
        [Method]
        public void DisableInternalLayoutUpdate() { }
        [Method]
        public void EnableInternalLayoutUpdate() { }
        [Method]
        public void AddItem(UIElement item) { }
        [Method]
        public void InsertItem(uint index, UIElement item) { }
        [Method]
        public void InsertItem(uint index, UIElement item, UIElement parentItem) { }
        [Method]
        public void RemoveItem(UIElement item) { }
        [Method]
        public void RemoveItem(UIElement item, uint index) { }
        [Method]
        public void RemoveItem(uint index) { }
        [Method]
        public void RemoveAllItems() { }
        [Method]
        public void AddSelection(uint index) { }
        [Method]
        public void RemoveSelection(uint index) { }
        [Method]
        public void ToggleSelection(uint index) { }
        [Method]
        public void ChangeSelection(int delta) { }
        [Method]
        public void ChangeSelection(int delta, bool additive) { }
        [Method]
        public void ClearSelection() { }
        [Method]
        public void Expand(uint index, bool enable) { }
        [Method]
        public void Expand(uint index, bool enable, bool recursive) { }
        [Method]
        public void ToggleExpand(uint index) { }
        [Method]
        public void ToggleExpand(uint index, bool recursive) { }
        [Method]
        public UIElement GetItem(uint index) => new UIElement();
        [Method]
        public uint FindItem(UIElement item) => 0;
        [Method]
        public void CopySelectedItemsToClipboard() { }
        [Method]
        public bool IsExpanded(uint index) => false;
        [Method]
        public void EnsureItemVisibility(uint index) { }
        [Method]
        public void EnsureItemVisibility(UIElement item) { }
    }
}
