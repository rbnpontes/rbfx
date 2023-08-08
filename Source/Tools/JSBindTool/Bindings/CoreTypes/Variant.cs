using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.CoreTypes
{
    [Include("Urho3D/Core/Variant.h")]
    public class ResourceRef : PrimitiveObject
    {
        public ResourceRef() : base(typeof(ResourceRef)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(StringHash type) { }
        [Constructor]
        public void Constructor(string type, string name) { }

        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(ResourceRef resRef) => false;

        [Method]
        public uint ToHash() => 0;
        [Method]
        [CustomCode(typeof(StringHash))]
        public void GetType(CodeBuilder code)
        {
            code.Add("StringHash result = instance->type_;");
        }
        [Method]
        [CustomCode(new Type[] { typeof(StringHash) })]
        public void SetType(CodeBuilder code)
        {
            code.Add($"instance->type_ = arg0;");
        }
        [Method]
        [CustomCode(typeof(string))]
        public void GetName(CodeBuilder code)
        {
            code.Add("ea::string result = instance->name_;");
        }
        [Method]
        [CustomCode(new Type[] { typeof(string) })]
        public void SetName(CodeBuilder code)
        {
            code.Add("instance->name_ = arg0;");
        }
    }

    public class Variant : NoopPrimitive
    {
        public Variant() : base(typeof(Variant)) { }
    }
}
