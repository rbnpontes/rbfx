using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/UI.h")]
    public class UI : ClassObject
    {
        [PropertyMap("GetRoot")]
        public UIElement Root { get; } = new UIElement();
        public UI() : base(typeof(UI)) { }
        public UI(Type type) : base(type) { }
    }
}
