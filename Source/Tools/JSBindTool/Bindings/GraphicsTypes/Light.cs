using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.GraphicsTypes
{
    [Include("Urho3D/Graphics/Light.h")]
    public class BiasParameters : PrimitiveObject
    {
        [Variable("constantBias_")]
        public float ConstantBias;
        [Variable("slopeScaledBias_")]
        public float SlopeScaledBias;
        [Variable("normalOffset_")]
        public float NormalOffset;
        public BiasParameters() : base(typeof(BiasParameters)) { }

        [Method]
        public void Validate() { }
    }
}
