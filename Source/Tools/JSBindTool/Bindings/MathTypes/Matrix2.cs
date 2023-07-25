using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Matrix2.h")]
    public class Matrix2 : PrimitiveObject
    {
        [Variable("m00_")]
        public float M00;
        [Variable("m01_")]
        public float M01;
        [Variable("m10_")]
        public float M10;
        [Variable("m11_")]
        public float M11;

        #region Properties
        [PropertyMap("Scale", "SetScale")]
        public Vector2 Scale { get => new Vector2(); set { } }
        [PropertyMap("Transpose")]
        public Matrix2 Transpose { get => new Matrix2(); }
        [PropertyMap("Inverse")]
        public Matrix2 Inverse { get => new Matrix2(); }
        [PropertyMap("IsNaN")]
        public bool IsNaN { get; }
        [PropertyMap("IsInf")]
        public bool IsInf { get; }
        #endregion

        public Matrix2() : base(typeof(Matrix2))
        {
        }

        #region Operator Methods
        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(Matrix2 m) => true;
        [OperatorMethod(OperatorType.Equal)]
        [CustomCode(typeof(bool), new Type[] { typeof(Matrix2), typeof(float) })]
        public void EqualOperator(CodeBuilder code)
        {
            MathCodeUtils.EmitEqualOperator(code);
        }
        [OperatorMethod(OperatorType.Add)]
        public Matrix2 AddOperator(Matrix2 m) => new Matrix2();
        [OperatorMethod(OperatorType.Sub)]
        public Matrix2 SubOperator(Matrix2 m) => new Matrix2();
        [OperatorMethod(OperatorType.Mul)]
        public Matrix2 MulOperator(Matrix2 m) => new Matrix2();
        [OperatorMethod(OperatorType.Mul)]
        public Matrix2 MulOperator(float v) => new Matrix2();
        #endregion
        #region Methods
        [Method("SetScale")]
        public void SetScale(float scale) { }
        [Method("Scaled")]
        public Matrix2 Scaled(Vector2 scale) => new Matrix2();
        [Method("Data")]
        [CustomCode(typeof(Vector<float>))]
        public void GetData(CodeBuilder code)
        {
            MathCodeUtils.EmitGetData(code, 2 * 2);
        }
        [Method("ToString")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() => string.Empty;
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        #endregion

        [Field("zero", "ZERO")]
        public static Matrix2 Zero = new Matrix2();
        [Field("identity", "IDENTITY")]
        public static Matrix2 Identity = new Matrix2();

        [Method("BulkTranspose")]
        [CustomCode(new Type[] { typeof(Vector<float>), typeof(Vector<float>)})]
        public static void BulkTranspose(CodeBuilder code)
        {
            MathCodeUtils.EmitMatrixBulkTranspose("Matrix2", code);
        }
    }
}
