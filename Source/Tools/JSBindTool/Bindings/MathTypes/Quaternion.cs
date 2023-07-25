using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Quaternion.h")]
    public class Quaternion : PrimitiveObject
    {
        #region Variables
        [Variable("x_")]
        public float X;
        [Variable("y_")]
        public float Y;
        [Variable("z_")]
        public float Z;
        [Variable("w_")]
        public float W;
        #endregion
        #region Properties
        [PropertyMap("Normalized")]
        public Quaternion Normalized { get => new Quaternion(); }
        [PropertyMap("Inverse")]
        public Quaternion Inverse { get => new Quaternion(); }
        [PropertyMap("LengthSquared")]
        public float LengthSquared { get => 0f; }
        [PropertyMap("IsNaN")]
        public bool IsNaN { get => false; }
        [PropertyMap("IsInf")]
        public bool IsInf { get => false; }
        [PropertyMap("Conjugate")]
        public Quaternion Conjugate { get => new Quaternion(); }
        [PropertyMap("EulerAngles")]
        public Vector3 EulerAngles { get => new Vector3(); }
        [PropertyMap("YawAngle")]
        public float YawAngle { get => 0f; }
        [PropertyMap("PitchAngle")]
        public float PitchAngle { get => 0f; }
        [PropertyMap("RollAngle")]
        public float RollAngle { get => 0f; }
        [PropertyMap("Axis")]
        public Vector3 Axis { get => new Vector3(); }
        [PropertyMap("Angle")]
        public float Angle { get => 0f; }
        [PropertyMap("AngularVelocity")]
        public Vector3 AngularVelocity { get => new Vector3(); }
        [PropertyMap("RotationMatrix")]
        public Matrix3 RotationMatrix { get => new Matrix3(); }
        #endregion
        public Quaternion() : base(typeof(Quaternion))
        {
        }

        #region Operators
        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(Quaternion rot) => false;
        [OperatorMethod(OperatorType.Equal)]
        [CustomCode(typeof(bool), new Type[] { typeof(Quaternion), typeof(float) })]
        public void EqualOperator(CodeBuilder code)
        {
            MathCodeUtils.EmitEqualOperator(code);
        }
        [OperatorMethod(OperatorType.Add)]
        public Quaternion AddOperator(Quaternion rhs) => new Quaternion();
        [OperatorMethod(OperatorType.Sub)]
        public Quaternion SubOperator(Quaternion rhs) => new Quaternion();
        [OperatorMethod(OperatorType.Mul)]
        public Quaternion MulOperator(Quaternion rot) => new Quaternion();
        [OperatorMethod(OperatorType.Mul)]
        public Quaternion MulOperator(float value) => new Quaternion();
        [OperatorMethod(OperatorType.Mul)]
        public Vector3 MulOperator(Vector3 value) => new Vector3();
        #endregion
        #region Methods
        [Method("FromAngleAxis")]
        public void FromAngleAxis(float angle, Vector3 axis) { }
        [Method("FromEulerAngles")]
        public void FromEulerAngles(float x, float y, float z) { }
        [Method("FromRotationTo")]
        public void FromRotationTo(Vector3 start, Vector3 end) { }
        [Method("FromAxes")]
        public void FromAxes(Vector3 xAxis, Vector3 yAxis, Vector3 zAxis) { }
        [Method("FromRotationMatrix")]
        public void FromRotationMatrix(Matrix3 matrix) { }
        [Method("FromLookRotation")]
        public void FromLookRotation(Vector3 direction) { }
        [Method("FromLookRotation")]
        public void FromLookRotation(Vector3 direction, Vector3 up) { }
        [Method("Normalize")]
        public void Normalize() { }
        [Method("DotProduct")]
        public float DotProduct(Quaternion rhs) => 0f;
        [Method("Equivalent")]
        public bool Equivalent(Quaternion rot) => false;
        [Method("Equivalent")]
        public bool Equivalent(Quaternion rot, float eps) => false;
        [Method("Slerp")]
        public Quaternion Slerp(Quaternion rot, float t) => new Quaternion();
        [Method("Nlerp")]
        public Quaternion Nlerp(Quaternion rot, float t) => new Quaternion();
        [Method("Nlerp")]
        public Quaternion Nlerp(Quaternion rot, float t, bool shortestPath) => new Quaternion();
        [Method("ToSwingTwist")]
        [CustomCode(new Type[] { typeof(Vector3) })]
        public void ToSwingTwist(CodeBuilder code)
        {
            code
                .Add("auto result = instance.ToSwingTwist(arg0);")
                .Add("duk_push_array(ctx);")
                .Add("jsbind_quaternion_push(ctx, result.first);")
                .Add("duk_put_prop_index(ctx, -2, 0);")
                .Add("jsbind_quaternion_push(ctx, result.second);")
                .Add("duk_put_prop_index(ctx, -2, 1);")
                .Add("result_code = 1;");
        }
        [Method("Data")]
        [CustomCode(new Type[] { typeof(Vector<float>) })]
        public void GetData(CodeBuilder code)
        {
            MathCodeUtils.EmitGetData(code, 4);
        }
        [Method("ToString")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() => string.Empty;
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        [Method("ToHash")]
        public uint ToHash() => 0;
        #endregion
        #region Static Methods
        [Method("FromAngularVelocity")]
        public static Quaternion FromAngularVelocity(Vector3 angularVelocity) { return new Quaternion(); }
        #endregion
        #region Static Fields
        [Field("identity", "IDENTITY")]
        public static Quaternion Identity = new Quaternion();
        [Field("zero", "ZERO")]
        public static Quaternion Zero = new Quaternion();
        #endregion
    }
}
