using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Color.h")]
    [Operator(OperatorFlags.Add | OperatorFlags.Sub | OperatorFlags.Mul | OperatorFlags.Equal)]
    public class Color : PrimitiveObject
    {
        [Variable("r_")]
        public float R;
        [Variable("g_")]
        public float G;
        [Variable("b_")]
        public float B;
        [Variable("a_")]
        public float A;

        public Color() : base(typeof(Color)) { }
    }
}
