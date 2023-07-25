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

        [Variable("m30_")]
        public float M30;
        [Variable("m31_")]
        public float M31;
        [Variable("m32_")]
        public float M32;
        [Variable("m33_")]
        public float M33;
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
        [PropertyMap("Transpose")]
        public Matrix4 Transpose { get => new Matrix4(); }
        [PropertyMap("Inverse")]
        public Matrix4 Inverse { get => new Matrix4(); }
        [PropertyMap("IsNaN")]
        public bool IsNaN { get => false; }
        [PropertyMap("IsInf")]
        public bool IsInf { get => false; }
        #endregion
        public Matrix4() : base(typeof(Matrix4))
        {
        }

        #region Operators
        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(Matrix4 matrix) => false;
        [OperatorMethod(OperatorType.Equal)]
        [CustomCode(typeof(bool), new Type[] { typeof(Matrix4), typeof(float)})]
        public void EqualOperator(CodeBuilder code)
        {
            MathCodeUtils.EmitEqualOperator(code);
        }
        [OperatorMethod(OperatorType.Mul)]
        public Vector3 MulOperator(Vector3 vec) => new Vector3();
        [OperatorMethod(OperatorType.Mul)]
        public Vector4 MulOperator(Vector4 vec) => new Vector4();
        [OperatorMethod(OperatorType.Mul)]
        public Matrix4 MulOperator(float value)=> new Matrix4();
        [OperatorMethod(OperatorType.Mul)]
        public Matrix4 MulOperator(Matrix4 matrix) => new Matrix4();
        [OperatorMethod(OperatorType.Mul)]
        public Matrix4 MulOperator(Matrix3x4 matrix) => new Matrix4();
        [OperatorMethod(OperatorType.Add)]
        public Matrix4 AddOperator(Matrix4 matrix) => new Matrix4();
        [OperatorMethod(OperatorType.Sub)]
        public Matrix4 SubOperator(Matrix4 matrix) => new Matrix4();
        #endregion
        #region Methods
        [Method("ToMatrix3")]
        public Matrix3 ToMatrix3() => new Matrix3();
        [Method("SignedScale")]
        public Vector3 SignedScale(Matrix3 rotation) => new Vector3();
        [Method("Decompose")]
        [CustomCode(new Type[] { typeof(Vector3), typeof(Quaternion), typeof(Vector3)})]
        public void Decompose(CodeBuilder code)
        {
            MathCodeUtils.EmitMatrixDecompose(code);
        }
        [Method("Data")]
        [CustomCode(typeof(Vector<float>))]
        public void GetData(CodeBuilder code)
        {
            MathCodeUtils.EmitGetData(code, 4 * 4);
        }
        [Method("Element")]
        public float Element(uint i, uint j) => 0f;
        [Method("Row")]
        public Vector4 Row(uint i) => new Vector4();
        [Method("Column")]
        public Vector4 Column(uint j) => new Vector4();
        [Method("ToString")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() => string.Empty;
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        [Method("ToHash")]
        public uint ToHash() => 0;
        #endregion
        #region Static Methods
        [Method("BulkTranspose")]
        [CustomCode(new Type[] { typeof(Vector<float>), typeof(Vector<float>)})]
        public static void BulkTranspose(CodeBuilder code)
        {
            MathCodeUtils.EmitMatrixBulkTranspose("Matrix4", code);
        }
        #endregion
        #region Static Fields
        [Field("zero", "ZERO")]
        public static Matrix4 Zero = new Matrix4();
        [Field("identity", "IDENTITY")]
        public static Matrix4 Identity = new Matrix4();
        #endregion
    }
}
