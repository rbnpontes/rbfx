using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Sphere.h")]
    public class Sphere : PrimitiveObject
    {
        public Sphere() : base(typeof(Sphere))
        {
        }
    }
}
