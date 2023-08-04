using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/NumericRange.h")]
    [Define("using DoubleRange = Urho3D::NumericRange<double>;")]
    [TypeName("DoubleRange", "NumericRange")]
    public class DoubleRange : PrimitiveObject
    {
        [Variable("first")]
        public double first;
        [Variable("second")]
        public double second;

        public DoubleRange() : base(typeof(DoubleRange)) { }

        protected override string GetJSTypeIdentifier()
        {
            return "NumericRange";
        }

        [Method]
        bool IsValid() => false;
        [Method]
        bool Intersect(DoubleRange other) => false;
        [Method]
        bool ContainsInclusive(double value) => false;
        [Method]
        bool ContainsExclusive(double value) => false;
    }
}
