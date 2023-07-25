using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Matrix3x4.h")]
    public class Matrix3x4 : PrimitiveObject
    {
        #region Variables
        [Variable("m00_")]
        public float M00;
        [Variable("m01_")]
        public float M01;
        [Variable("m02_")]
        public float M02;
        [Variable("m03_")]
        public float M03;

        [Variable("m10_")]
        public float M10;
        [Variable("m11_")]
        public float M11;
        [Variable("m12_")]
        public float M12;
        [Variable("m13_")]
        public float M13;

        [Variable("m20_")]
        public float M20;
        [Variable("m21_")]
        public float M21;
        [Variable("m22_")]
        public float M22;
        [Variable("m23_")]
        public float M23;
        #endregion
        #region Properties
        [PropertyMap("Translation", "SetTranslation")]
        public Vector3 Translation { get => new Vector3(); set { } }
        [PropertyMap("Rotation")]
        public Quaternion Rotation { get => new Quaternion(); }
        [PropertyMap("RotationMatrix", "SetRotation")]
        public Matrix3 RotationMatrix { get => new Matrix3(); set { } }
        [PropertyMap("Scale", "SetScale")]
        public Vector3 Scale { get => new Vector3(); set { } }
        [PropertyMap("Determinant")]
        public float Determinant { get => 0f; }
        [PropertyMap("Inverse")]
        public Matrix3x4 Inverse { get => new Matrix3x4(); }
        [PropertyMap("IsNaN")]
        public bool IsNaN { get => false; }
        [PropertyMap("IsInf")]
        public bool IsInf { get => false; }
        #endregion
        public Matrix3x4() : base(typeof(Matrix3x4)) { }

        #region Operators
        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(Matrix3x4 m) => false;
        [OperatorMethod(OperatorType.Equal)]
        [CustomCode(typeof(bool), new Type[] { typeof(Matrix3x4), typeof(float) })]
        public void EqualOperator(CodeBuilder code)
        {
            MathCodeUtils.EmitEqualOperator(code);
        }
        [OperatorMethod(OperatorType.Mul)]
        public Vector3 MulOperator(Vector3 vec) => new Vector3();
        [OperatorMethod(OperatorType.Mul)]
        public Vector3 MulOperator(Vector4 vec) => new Vector3();
        [OperatorMethod(OperatorType.Mul)]
        public Matrix3x4 MulOperator(float vec) => new Matrix3x4();
        [OperatorMethod(OperatorType.Mul)]
        public Matrix3x4 MulOperator(Matrix3x4 m) => new Matrix3x4();
        [OperatorMethod(OperatorType.Mul)]
        public Matrix4 MulOperator(Matrix4 m) => new Matrix4();
        [OperatorMethod(OperatorType.Add)]
        public Matrix3x4 AddOperator(Matrix3x4 m) => new Matrix3x4();
        [OperatorMethod(OperatorType.Sub)]
        public Matrix3x4 SubOperator(Matrix3x4 m) => new Matrix3x4();
        #endregion
        #region Methods
        [Method("SetRotation")]
        public void SetRotation(Matrix3 rotation) { }
        [Method("SetScale")]
        public void SetScale(float scale) { }
        [Method("ToMatrix3")]
        public Matrix3 ToMatrix3() => new Matrix3();
        [Method("ToMatrix4")]
        public Matrix4 ToMatrix4() => new Matrix4();
        [Method("SignedScale")]
        public Vector3 SignedScale(Matrix3 rotation) => new Vector3();
        [Method("Decompose")]
        [CustomCode(new Type[] { typeof(Vector3), typeof(Quaternion), typeof(Vector3) })]
        public void Decompose(CodeBuilder code)
        {
            MathCodeUtils.EmitMatrixDecompose(code);
        }
        [Method("Data")]
        [CustomCode(typeof(Vector<float>))]
        public void GetData(CodeBuilder code)
        {
            MathCodeUtils.EmitGetData(code, 3 * 4);
        }
        [Method("Element")]
        public float Element(uint i, uint j) => 0f;
        [Method("Row")]
        public Vector4 Row(uint i) => new Vector4();
        [Method("Column")]
        public Vector3 Column(uint j) => new Vector3();
        [Method("ToString")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() => string.Empty;
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        [Method("ToHash")]
        public uint ToHash() => 0;
        #endregion
        #region Static Methods
        [Method("FromTranslation")]
        public static Matrix3x4 FromTranslation(Vector3 translation) => new Matrix3x4();
        [Method("FromRotation")]
        public static Matrix3x4 FromRotation(Quaternion rotation) => new Matrix3x4();
        [Method("FromScale")]
        public static Matrix3x4 FromScale(float scale) => new Matrix3x4();
        [Method("FromScale")]
        public static Matrix3x4 FromScale(Vector3 scale) => new Matrix3x4();
        #endregion
        #region Static Fields
        [Field("zero", "ZERO")]
        public static Matrix3x4 Zero = new Matrix3x4();
        [Field("identity", "IDENTITY")]
        public static Matrix3x4 Identity = new Matrix3x4();
        #endregion
    }
}
