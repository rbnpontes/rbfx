using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Transform.h")]
    public class Transform : PrimitiveObject
    {
        [Variable("position_")]
        public Vector3 Position = new Vector3();
        [Variable("rotation_")]
        public Quaternion Rotation = new Quaternion();
        [Variable("scale_")]
        public Vector3 Scale = new Vector3();

        [PropertyMap("Inverse")]
        public Transform Inverse { get => new Transform(); }

        public Transform() : base(typeof(Transform))
        {
        }

        [Constructor]
        public void Constructor() { }

        [OperatorMethod(OperatorType.Mul)]
        public Transform MulOperator(Transform transform) => new Transform();
        [OperatorMethod(OperatorType.Mul)]
        public Vector3 MulOperator(Vector3 point) => new Vector3();
        [OperatorMethod(OperatorType.Mul)]
        public Quaternion MulOperator(Quaternion rotation) => new Quaternion();

        [Method]
        public Matrix3x4 ToMatrix3x4() => new Matrix3x4();
        [Method]
        public Transform Lerp(Transform transform, float t) => new Transform();

        public static Transform FromMatrix3x4(Matrix3x4 matrix) => new Transform();
    }
}
