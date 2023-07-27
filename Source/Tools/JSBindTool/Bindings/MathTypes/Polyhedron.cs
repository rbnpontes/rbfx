using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Polyhedron.h")]
    public class Polyhedron : PrimitiveObject
    {
        public Polyhedron() : base(typeof(Polyhedron)) { }
    }
}
