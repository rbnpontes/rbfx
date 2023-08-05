using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    [Include("Urho3D/Math/RandomEngine.h")]
    public class RandomEngine : PrimitiveObject
    {
        public RandomEngine() : base(typeof(RandomEngine)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(uint seed) { }
        [Constructor]
        public void Constructor(string state) { }

        [Method]
        public void Load(string state) { }
        [Method]
        public string Save() => string.Empty;

        [Method]
        public uint GetUInt() => 0;
        [Method]
        public uint GetUInt(uint range) => 0;
        [Method]
        public uint GetUInt(uint min, uint max) => 0;
        [Method]
        public int GetInt(int min, int max) => 0;
        [Method("Shuffle")]
        [CustomCode(new Type[] { typeof(Array) })]
        public void Shuffle(CodeBuilder code)
        {
            code
                .Add("duk_size_t length = duk_get_length(ctx, arg0);")
                .Add("for (unsigned i = length - 1; i > 0; --i)")
                .Scope(code =>
                {
                    code
                        .Add("const unsigned j = instance->GetUInt(i + 1);")
                        .Add("duk_idx_t a_idx, b_idx;")
                        .AddNewLine()
                        .Add("duk_get_prop_index(ctx, arg0, i);")
                        .Add("a_idx = duk_get_top_index(ctx);")
                        .Add("duk_get_prop_index(ctx, arg0, j);")
                        .Add("b_idx = duk_get_top_index(ctx);")
                        .AddNewLine()
                        .Add("// shuflle indices")
                        .Add("std::swap(a_idx, b_idx);")
                        .Add("// now insert again values into array")
                        .Add("duk_dup(ctx, a_idx);")
                        .Add("duk_put_prop_index(ctx, arg0, i);")
                        .Add("duk_dup(ctx, b_idx);")
                        .Add("duk_put_prop_index(ctx, arg0, j);")
                        .AddNewLine()
                        .Add("duk_pop_2(ctx);");
                });
        }
        [Method]
        public double GetDouble() => 0.0;
        [Method]
        public double GetDouble(double min, double max) => 0.0;
        [Method]
        public bool GetBool(float probability) => false;
        [Method]
        public float GetFloat() => 0.0f;
        [Method]
        public float GetFloat(float min, float max) => 0.0f;
        [Method("GetStandardNormalFloatPair")]
        [CustomCode]
        public void GetStandardNormalFloatPair(CodeBuilder code)
        {
            code
                .Add("ea::pair<float, float> result = instance->GetStandardNormalFloatPair();")
                .Add("duk_push_array(ctx);")
                .Add("duk_push_number(ctx, result.first);")
                .Add("duk_put_prop_index(ctx, -2, 0);")
                .Add("duk_push_number(ctx, result.second);")
                .Add("duk_put_prop_index(ctx, -2, 1);")
                .Add("result_code = 1;");
        }
        [Method]
        public float GetStandardNormalFloat() => 0.0f;
        [Method]
        public float GetNormalFloat(float mean, float sigma) => 0f;

        [Method]
        public Vector2 GetDirectionVector2() => new Vector2();
        [Method]
        public Vector3 GetDirectionVector3() => new Vector3();
        [Method]
        public Quaternion GetQuaternion() => new Quaternion();

        [Method]
        public Vector2 GetVector2(Vector2 min, Vector2 max) => new Vector2();
        [Method]
        public Vector3 GetVector3(Vector3 min, Vector3 max) => new Vector3();
        [Method]
        public Vector3 GetVector3(BoundingBox bbox) => new Vector3();

        [Method]
        public static RandomEngine GetDefaultEngine() => new RandomEngine();
        [Method]
        public static uint MaxRange() => 0;
        [CustomField("maxIterations")]
        public static void MaxIterations(CodeBuilder code)
        {
            code.Add("duk_push_uint(ctx, RandomEngine::MaxIterations);");
        }
    }
}
