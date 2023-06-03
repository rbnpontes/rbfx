using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    [Ignore]
    public class NoopPrimitive : PrimitiveObject
    {
        public NoopPrimitive(Type type) : base(type)
        {
        }

        public override void EmitResolveSignature(CodeBuilder code)
        {
        }
        public override void EmitConstructorSignature(CodeBuilder code)
        {
        }
        public override void EmitSetupSignature(CodeBuilder code)
        {
            code.Add($"void {Target.Name}_setup(duk_context* ctx);");
        }

        public override void EmitConstructorSource(CodeBuilder code)
        {
        }
        public override void EmitResolveSource(CodeBuilder code)
        {
        }
        public override void EmitSetupSetupSource(CodeBuilder code)
        {
            code.Add($"void {Target.Name}_setup(duk_context* ctx)");
            code.Add("{").Add("}");
        }
    }
}
