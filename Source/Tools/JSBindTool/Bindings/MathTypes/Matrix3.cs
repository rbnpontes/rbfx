using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Vector3.h")]
    public class Matrix3 : PrimitiveObject
    {
        #region Variables
        [Variable("m00_")]
        public float M00;
        [Variable("m01_")]
        public float M01;
        [Variable("m02_")]
        public float M02;

        [Variable("m10_")]
        public float M10;
        [Variable("m11_")]
        public float M11;
        [Variable("m12_")]
        public float M12;

        [Variable("m20_")]
        public float M20;
        [Variable("m21_")]
        public float M21;
        [Variable("m22_")]
        public float M22;
        #endregion

        #region Properties
        [PropertyMap("Scale", "SetScale")]
        public Vector3 Scale { get => new Vector3(); set { } }
        [PropertyMap("Transpose")]
        public Matrix3 Transpose { get => new Matrix3(); }
        [PropertyMap("Determinant")]
        public float Determinant { get; }
        [PropertyMap("Inverse")]
        public Matrix3 Inverse { get => new Matrix3(); }
        [PropertyMap("IsNaN")]
        public bool IsNaN { get; }
        [PropertyMap("IsInf")]
        public bool IsInf { get; }
        #endregion
        public Matrix3() : base(typeof(Matrix3))
        {
        }

        #region Operators
        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(Matrix3 m) => true;
        [OperatorMethod(OperatorType.Equal)]
        [CustomCode(typeof(bool), new Type[] { typeof(Matrix3), typeof(float) })]
        public void EqualOperator(CodeBuilder code)
        {
            MathCodeUtils.EmitEqualOperator(code);
        }
        [OperatorMethod(OperatorType.Add)]
        public Matrix3 AddOperator(Matrix3 m) => new Matrix3();
        [OperatorMethod(OperatorType.Sub)]
        public Matrix3 SubOperator(Matrix3 m) => new Matrix3();
        [OperatorMethod(OperatorType.Mul)]
        public Matrix3 MulOperator(Matrix3 m) => new Matrix3();
        [OperatorMethod(OperatorType.Mul)]
        public Matrix3 MulOperator(float v) => new Matrix3();
        #endregion
        #region Methods
        [Method("FromAngleAxis")]
        public void FromAngleAxis(float angle, Vector3 axis) { }
        [Method("SetScale")]
        public void SetScale(float scale) { }
        [Method("SignedScale")]
        public Vector3 SignedScale(Matrix3 rot) => new Vector3();
        [Method("Scaled")]
        public Matrix3 Scaled(Vector3 scale) => new Matrix3();
        [Method("Data")]
        [CustomCode(typeof(Vector<float>))]
        public void GetData(CodeBuilder code)
        {
            MathCodeUtils.EmitGetData(code, 3 * 3);
        }
        [Method("Element")]
        public float Element(uint i, uint j) => 0f;
        [Method("Row")]
        public Vector3 Row(uint i) => new Vector3();
        [Method("Column")]
        public Vector3 Column(uint j) => new Vector3();

        [Method("ToString")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() => string.Empty;
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        [Method("ToHash")]
        public uint ToHash() => 0;
        #endregion
        [Field("zero", "ZERO")]
        public static Matrix3 Zero = new Matrix3();
        [Field("identity", "IDENTITY")]
        public static Matrix3 Identity = new Matrix3();

        [Method("BulkTranspose")]
        [CustomCode(new Type[] { typeof(Vector<float>), typeof(Vector<float>) })]
        public static void BulkTranspose(CodeBuilder code)
        {
            MathCodeUtils.EmitMatrixBulkTranspose("Matrix3", code);
        }
    }
}
