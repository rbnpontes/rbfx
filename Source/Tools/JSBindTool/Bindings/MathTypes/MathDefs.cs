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

        [Method("Random")]
        public float Random() => 0f;
        [Method("Random")]
        public float Random(float range) => 0f;
        [Method("Random")]
        public float Random(float min, float max) => 0f;
        [Method("RandomNormal")]
        public float RandomNormal(float meanValue, float variance) => 0f;
    }
}
