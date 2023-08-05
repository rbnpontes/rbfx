using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/PerlinNoise.h")]
    public class PerlinNoise : PrimitiveObject
    {
        public PerlinNoise() : base(typeof(PerlinNoise))
        {
        }

        [Constructor]
        public void Constructor(RandomEngine engine) { }

        [Method]
        public double GetDouble(double x, double y, double z) => 0.0;
        [Method]
        public double GetDouble(double x, double y, double z, int repeat) => 0.0;
        [Method]
        public float Get(float x, float y, float z) => 0f;
        [Method]
        public float Get(float x, float y, float z, int repeat) => 0f;

        [CustomField("numPer")]
        public static void NumPer(CodeBuilder code)
        {
            code.Add("duk_push_uint(ctx, PerlinNoise::NumPer);");
        }
    }
}
