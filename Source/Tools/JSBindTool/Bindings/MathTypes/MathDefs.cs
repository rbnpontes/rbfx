using JSBindTool.Builders;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/MathDefs.h")]
    [Include("Urho3D/Math/StringHash.h")]
    [Namespace(Constants.ProjectName)]
    public class MathDefs : ModuleObject
    {
        public MathDefs() : base(typeof(MathDefs)) { }
        [Method("Equals")]
        public float Equals(float a, float b, float eps) => 0f;
        [Method("Lerp")]
        public float Lerp(float from, float to, float t) => 0f;
        [Method("InverseLerp")]
        public float InverseLerp(float from, float to, float t) => 0f;
        [Method("ToRadians")]
        public float ToRadians(float x) => 0;
        [Method("ToDegrees")]
        public float ToDegree(float x) => 0;
        [Method("FloatToRawIntBits")]
        public uint FloatToRawIntBits(float x) => 0;
        [Method("IsNaN")]
        public bool IsNaN(float x) => false;
        [Method("IsInf")]
        public bool IsInf(float x) => false;
        [Method("SmoothStep")]
        public float SmoothStep(float lhs, float rhs, float t) => 0f;
        [Method("ExpSmoothing")]
        public float ExpSmoothing(float constant, float timestep) => 0f;
        [Method("Ln")]
        public float Ln(float x) => 0f;
        [Method("AbsMod")]
        public float AbsMode(float x, float y) => 0f;
        [Method("Fract")]
        public float Fract(float x) => 0f;
        [Method("SnapFloor")]
        public float SnapFloor(float x, float y) => 0f;
        [Method("FloorToInt")]
        public int FloorToInt(float x) => 0;
        [Method("SnapRound")]
        public float SnapRound(float x, float y) => 0;
        [Method("RoundToInt")]
        public int RoundToInt(float x) => 0;
        [Method("SnapCeil")]
        public float SnapCeil(float x, float y) => 0f;
        [Method("IsPowerOfTwo")]
        public bool IsPowerOfTwo(uint x) => false;
        [Method("NextPowerOfTwo")]
        public uint NextPowerOfTwo(uint x) => 0;
        [Method("ClosestPowerOfTwo")]
        public uint ClosestPowerOfTwo(uint x) => 0;
        [Method("LogBaseTwo")]
        public uint LogBaseTwo(uint x) => 0;
        [Method("CountSetBits")]
        public uint CountSetBits(uint value) => 0;
        [Method("sdbmHash")]
        [CustomCode(typeof(StringHash), new Type[] { typeof(string) })]
        public void SDBMHash(CodeBuilder code)
        {
            code.Add("StringHash result(arg0);");
        }
        [Method("Random")]
        public float Random() => 0f;
        [Method("Random")]
        public float Random(float range) => 0f;
        [Method("Random")]
        public float Random(float min, float max) => 0f;
        [Method("RandomNormal")]
        public float RandomNormal(float meanValue, float variance) => 0f;
        [Method("FloatToHalf")]
        public ushort FloatToHalf(float value) => 0;
        [Method("HalfToFloat")]
        public float HalfToFloat(ushort value) => 0f;
        [Method("Wrap")]
        public float Wrap(float value, float min, float max) => 0f;
    }
}
