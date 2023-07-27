using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Rect.h")]
    public class Rect : PrimitiveObject
    {
        public Rect() : base(typeof(Rect))
        {
        }
    }
}
