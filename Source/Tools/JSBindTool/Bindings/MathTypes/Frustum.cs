using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Frustum.h")]
    public class Frustum : PrimitiveObject
    {
        public Frustum() : base(typeof(Frustum)) { }
    }
}
