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
        public UISelectable() : base(typeof(UISelectable)) { }
        public UISelectable(Type type) : base(type) { }
    }
}
