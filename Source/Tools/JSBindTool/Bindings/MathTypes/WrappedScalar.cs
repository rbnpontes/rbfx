using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/WrappedScalar.h")]
    [Define("#define NumberScalarRange Urho3D::WrappedScalarRange<double>")]
    [TypeName("NumberScalarRange", "WrappedScalarRange")]
    public class NumberScalarRange : PrimitiveObject
    {
        [PropertyMap("Begin")]
        public double Begin { get => 0; }
        [PropertyMap("End")]
        public double End { get => 0; }
        [PropertyMap("Min")]
        public double Min { get => 0; }
        [PropertyMap("Max")]
        public double Max { get => 0; }
        [PropertyMap("IsEmpty")]
        public bool IsEmpty { get => false; }
        public NumberScalarRange() : base(typeof(NumberScalarRange)) { }

        [Constructor]
        public void Constructor(double value, double minValue, double maxValue) { }
        [Constructor]
        public void Constructor(double beginValue, double endValue, double minValue, double maxValue, int numWraps) { }

        [Method]
        public bool ContainsInclusive(double value) => false;
        [Method]
        public bool ContainsExclusive(double value) => false;
        [Method]
        public bool ContainsExcludingBegin(double value) => false;
        [Method]
        public bool ContainsExcludingEnd(double value) => false;

        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(NumberScalarRange range) => false;
    }
    [Include("Urho3D/Math/WrappedScalar.h")]
    [Define("#define NumberScalar Urho3D::WrappedScalar<double>")]
    [TypeName("NumberScalar", "WrappedScalar")]
    public class NumberScalar : PrimitiveObject
    {
        [PropertyMap("Value")]
        public double Value { get; }
        [PropertyMap("Min")]
        public double Min { get; }
        [PropertyMap("Max")]
        public double Max { get; }
        public NumberScalar() : base(typeof(NumberScalar)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(double value, double minValue, double maxValue) { }

        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(NumberScalar value) => false;

        [Method]
        public void Set(double value) { }
        [Method]
        public NumberScalarRange UpdateWrapped(double delta) => new NumberScalarRange();
        [Method]
        public NumberScalarRange UpdateClamped(double delta) => new NumberScalarRange();
        [Method]
        public NumberScalarRange UpdateClamped(double delta, bool returnOutOfBounds) => new NumberScalarRange();
        [Method]
        public NumberScalar MinMaxClamped(double minValue, double maxValue) => new NumberScalar();
    }
}
