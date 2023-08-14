using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class WeakPtr<T> : TemplateObject, ICodeEmitter
    {
        public WeakPtr() : base(typeof(T), TemplateType.WeakPtr) { }

        public void EmitRead(string varName, string accessor, CodeBuilder code)
        {
            code
                .Add($"{GetNativeDeclaration()} {varName};")
                .Add($"{varName}.StaticCast(rbfx_get_weak_ptr(ctx, {accessor}));");
        }

        public void EmitWrite(string accessor, CodeBuilder code)
        {
            code.Add($"rbfx_push_weak_ptr(ctx, {accessor});");
        }

        public string GetNativeDeclaration()
        {
            return $"WeakPtr<{AnnotationUtils.GetTypeName(TargetType)}>";
        }
    }
}
