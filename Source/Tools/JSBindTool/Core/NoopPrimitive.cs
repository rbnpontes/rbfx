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

        public override void EmitSourceIncludes(CodeBuilder code)
        {
            code.Add($"#include \"{Target.Name}{Constants.PrimitiveIncludeSuffix}.h\"");
        }
        public override void EmitHeaderSignatures(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetMethodPrefix(Target)}_setup(duk_context* ctx);");
            code.Add($"void {CodeUtils.GetSetupStaticSignature(Target)}(duk_context* ctx);");
        }
        public override void EmitSource(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetMethodPrefix(Target)}_setup(duk_context* ctx)").Scope(x => { });
            code.Add($"void {CodeUtils.GetSetupStaticSignature(Target)}(duk_context* ctx)").Scope(x => { });
        }
    }
}
