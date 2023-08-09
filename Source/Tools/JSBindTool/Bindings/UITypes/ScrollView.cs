using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/ScrollView.h")]
    public class ScrollView : UIElement
    {
        [PropertyMap("GetContentElement", "SetContentElement")]
        public UIElement ContentElement { get => new UIElement(); set { } }
        [PropertyMap("GetViewPosition", "SetViewPosition")]
        public IntVector2 ViewPosition { get => new IntVector2(); set { } }
        [PropertyMap("GetHorizontalScrollBarVisible", "SetHorizontalScrollBarVisible")]
        public bool HorizontalScrollBarVisible { get; set; }
        [PropertyMap("GetVerticalScrollBarVisible", "SetVerticalScrollBarVisible")]
        public bool VerticalScrollBarVisible { get; set; }
        [PropertyMap("GetScrollBarsAutoVisible", "SetScrollBarsAutoVisible")]
        public bool ScrollBarsAutoVisible { get; set; }
        [PropertyMap("GetScrollStep", "SetScrollStep")]
        public float ScrollStep { get; set; }
        [PropertyMap("GetPageStep", "SetPageStep")]
        public float PageStep { get; set; }
        [PropertyMap("GetScrollDeceleration", "SetScrollDeceleration")]
        public float ScrollDeceleration { get; set; }
        [PropertyMap("GetScrollSnapEpsilon", "SetScrollSnapEpsilon")]
        public float ScrollSnapEpsilon { get; set; }
        [PropertyMap("GetAutoDisableChildren", "SetAutoDisableChildren")]
        public bool AutoDisableChildren { get; set; }
        [PropertyMap("GetAutoDisableThreshold", "SetAutoDisableThreshold")]
        public float AutoDisableThreshold { get; set; }
        [PropertyMap("GetHorizontalScrollBar")]
        public ScrollBar HorizontalScrollBar { get => new ScrollBar(); }
        [PropertyMap("GetVerticalScrollBar")]
        public ScrollBar VerticalScrollBar { get => new ScrollBar(); }
        [PropertyMap("GetScrollPanel")]
        public BorderImage ScrollPanel { get => new BorderImage(); }

        public ScrollView() : base(typeof(ScrollView)) { }
        public ScrollView(Type type) : base(type) { }

        [Method]
        public void SetViewPosition(int x, int y) { }
        [Method]
        public void SetScrollBarsVisible(bool horizontal, bool vertical) { }
        [Method]
        public void SetViewPositionAttr(IntVector2 value) { }
    }
}
