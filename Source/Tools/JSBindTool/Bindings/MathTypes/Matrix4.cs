using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Matrix4.h")]
    public class Matrix4 : PrimitiveObject
    {
        public Matrix4() : base(typeof(Matrix4))
        {
        }
    }
}
