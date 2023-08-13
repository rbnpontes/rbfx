using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/UISelectable.h")]
    public class UISelectable : UIElement
    {
        [PropertyMap("GetSelectionColor", "SetSelectionColor")]
        public Color SelectionColor { get => new Color(); set { } }
        [PropertyMap("GetHoverColor", "SetHoverColor")]
        public Color HoverColor { get => new Color(); set { } }

        public UISelectable() : base(typeof(UISelectable)) { }
        public UISelectable(Type type) : base(type) { }
    }
}
