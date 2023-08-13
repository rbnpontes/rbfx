using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/ToolTip.h")]
    public class ToolTip : UIElement
    {
        [PropertyMap("GetDelay", "SetDelay")]
        public float Delay { get; set; }

        public ToolTip() : base(typeof(ToolTip)) { }
        [Method]
        public void Reset() { }
        [Method]
        public void AddAltTarget(UIElement target) { }
    }
}
