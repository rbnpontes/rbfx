using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Plane.h")]
    public class Plane : PrimitiveObject
    {
        public Plane() : base(typeof(Plane)) { }
    }
}
