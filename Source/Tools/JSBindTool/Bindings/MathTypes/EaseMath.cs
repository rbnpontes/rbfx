using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Namespace(Constants.ProjectName+".EaseMath")]
    [Include("Urho3D/Math/EaseMath.h")]
    public class EaseMath : ModuleObject
    {
        public EaseMath() : base(typeof(EaseMath)) { }

        [Method("BackIn")]
        public float BackIn(float time) => 0f;
        [Method("BackOut")]
        public float BackOut(float time) => 0f;
        [Method("BackInOut")]
        public float BackInOut(float time) => 0f;

        [Method("BounceOut")]
        public float BounceOut(float time) => 0f;
        [Method("BounceIn")]
        public float BounceIn(float time) => 0f;
        [Method("BounceInOut")]
        public float BounceInOut(float time) => 0f;

        [Method("SineOut")]
        public float SineOut(float time) => 0f;
        [Method("SineIn")]
        public float SineIn(float time) => 0f;
        [Method("SineInOut")]
        public float SineInOut(float time) => 0f;

        [Method("ExponentialOut")]
        public float ExponentialOut(float time) => 0f;
        [Method("ExponentialIn")]
        public float ExponentialIn(float time) => 0f;
        [Method("ExponentialInOut")]
        public float ExponentialInOut(float time) => 0f;

        [Method("ElasticIn")]
        public float ElasticIn(float time, float period) => 0f;
        [Method("ElasticOut")]
        public float ElasticOut(float time, float period) => 0f;
        [Method("ElasticInOut")]
        public float ElasticInOut(float time, float period) => 0f;
    }
}
