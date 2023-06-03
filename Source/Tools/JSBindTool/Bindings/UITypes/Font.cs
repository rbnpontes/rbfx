using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/Font.h")]
    public class Font : Resource
    {
        public Font() : base(typeof(Font)) { }
        public Font(Type type) : base(type) { }
    }
}
