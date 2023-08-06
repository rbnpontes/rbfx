using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/SphericalHarmonics.h")]
    public class SphericalHarmonics9 : PrimitiveObject
    {
        public SphericalHarmonics9() : base(typeof(SphericalHarmonics9)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(Vector3 dir) { }

        [Method]
        [CustomCode(typeof(float), new Type[] { typeof(uint)} )]
        public void GetValue(CodeBuilder code)
        {
            MathCodeUtils.EmitGetProp("float", "values_", 9, code);
        }
        [Method]
        [CustomCode(new Type[] { typeof(uint), typeof(float) })]
        public void SetValue(CodeBuilder code)
        {
            MathCodeUtils.EmitSetProp("values_", 9, code);
        }

        [CustomField("factors")]
        public static void GetFactors(CodeBuilder code)
        {
            code
                .Add("duk_push_array(ctx);")
                .Scope(code =>
                {
                    for(int i =0; i < 9; ++i)
                    {
                        code
                            .Add($"duk_push_number(ctx, SphericalHarmonics9::factors[{i}]);")
                            .Add($"duk_put_prop_index(ctx, -2, {i});");
                    }
                });
        }
        [CustomField("cosines")]
        public static void GetCosines(CodeBuilder code)
        {
            code
                .Add("duk_push_array(ctx);")
                .Scope(code =>
                {
                    for (int i = 0; i < 9; ++i)
                    {
                        code
                            .Add($"duk_push_number(ctx, SphericalHarmonics9::cosines[{i}]);")
                            .Add($"duk_put_prop_index(ctx, -2, {i});");
                    }
                });
        }
    }
    [Include("Urho3D/Math/SphericalHarmonics.h")]
    public class SphericalHarmonicsColor9 : PrimitiveObject
    {
        public SphericalHarmonicsColor9() : base(typeof(SphericalHarmonicsColor9)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(Color color) { }
        [Constructor]
        public void Constructor(Vector3 dir, Vector3 color) { }
        [Constructor]
        public void Constructor(Vector3 dir, Color color) { }

        [OperatorMethod(OperatorType.Add)]
        [CustomCode(new Type[] { typeof(SphericalHarmonicsColor9) })]
        public void AddOperator(CodeBuilder code)
        {
            code
                .Add($"instance += arg0;")
                .Add($"duk_push_this(ctx);")
                .Add($"result_code = 1;");
            
        }
        [OperatorMethod(OperatorType.Mul)]
        public SphericalHarmonicsColor9 MulOperator(float value) => new SphericalHarmonicsColor9();

        [Method]
        [CustomCode(typeof(Vector3), new Type[] { typeof(uint) })]
        public void GetValue(CodeBuilder code)
        {
            MathCodeUtils.EmitGetProp("Vector3", "values_", 9, code);
        }
        [Method]
        [CustomCode(new Type[] { typeof(uint), typeof(Vector3) })]
        public void SetValue(CodeBuilder code)
        {
            MathCodeUtils.EmitSetProp("values_", 9, code);
        }

        [Method]
        public Vector3 Evaluate(Vector3 dir) => new Vector3();
        [Method]
        public Vector3 EvaluateAverage() => new Vector3();
    }
    [Include("Urho3D/Math/SphericalHarmonics.h")]
    public class SphericalHarmonicsDot9 : PrimitiveObject
    {
        [Variable("Ar_")]
        public Vector4 Ar = new Vector4();
        [Variable("Ag_")]
        public Vector4 Ag = new Vector4();
        [Variable("Ab_")]
        public Vector4 Ab = new Vector4();
        [Variable("Br_")]
        public Vector4 Br = new Vector4();
        [Variable("Bg_")]
        public Vector4 Bg = new Vector4();
        [Variable("Bb_")]
        public Vector4 Bb = new Vector4();
        [Variable("C_")]
        public Vector4 C = new Vector4();
        [PropertyMap("GetDebugColor")]
        public Color DebugColor { get => new Color(); }

        public SphericalHarmonicsDot9() : base(typeof(SphericalHarmonicsDot9)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(Vector3 color) { }
        [Constructor]
        public void Constructor(Color color) { }
        [Constructor]
        public void Constructor(SphericalHarmonicsDot9 color) { }

        [Method]
        public Vector3 Evaluate(Vector3 vec) => new Vector3();
        [Method]
        public Vector3 EvaluateAverage() => new Vector3();

        [OperatorMethod(OperatorType.Add)]
        [CustomCode(new Type[] { typeof(Vector3) })]
        public void AddOperator(CodeBuilder code)
        {
            code
                .Add($"instance += arg0;")
                .Add($"duk_push_this(ctx);")
                .Add($"result_code = 1;");
        }
        [OperatorMethod(OperatorType.Add)]
        [CustomCode(new Type[] { typeof(SphericalHarmonicsDot9) })]
        public void AddOperator1(CodeBuilder code)
        {
            AddOperator(code);
        }
        [OperatorMethod(OperatorType.Mul)]
        public SphericalHarmonicsDot9 MulOperator(float value) => new SphericalHarmonicsDot9();
    }
}
