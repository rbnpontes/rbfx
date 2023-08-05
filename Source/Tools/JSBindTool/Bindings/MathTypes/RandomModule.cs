using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Random.h")]
    [Namespace("rbfx.random")]
    public class RandomModule : ModuleObject
    {
        public RandomModule() : base(typeof(RandomModule))
        {
        }

        [Method]
        public void SetRandomSeed(uint seed) { }
        [Method]
        public uint GetRandomSeed() => 0;
        [Method]
        public int Rand() => 0;
        [Method]
        public float RandStandardNormal() => 0f;
    }
}
