using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public abstract class StructObject : BaseObject
    {
        public StructObject(Type type) : base(type) { }

        protected override string GetSelfHeader()
        {
            return $"{Target.Name}{Constants.StructIncludeSuffix}.h";
        }
        public override void EmitHeaderSignatures(CodeBuilder code)
        {
            code
                .Add($"void {CodeUtils.GetPushSignature(Target)}(duk_context* ctx, {AnnotationUtils.GetTypeName(Target)}& instance);")
                .Add($"{AnnotationUtils.GetTypeName(Target)} {CodeUtils.GetResolveSignature(Target)}(duk_context* ctx, duk_idx_t obj_idx);");
        }
        public override void EmitSource(CodeBuilder code)
        {
            code
                .Add($"void {CodeUtils.GetPushSignature(Target)}(duk_context* ctx, {AnnotationUtils.GetTypeName(Target)}& instance)")
                .Scope(EmitPushSource)
                .Add($"{AnnotationUtils.GetTypeName(Target)} {CodeUtils.GetResolveSignature(Target)}(duk_context* ctx, duk_idx_t obj_idx)")
                .Scope(EmitResolveSource);
        }

        protected virtual void EmitPushSource(CodeBuilder code)
        {
            code.Add("duk_push_object(ctx);")
                .Add("duk_idx_t top = duk_get_top_index(ctx);");
            AnnotationUtils.GetVariables(Target).ForEach(x =>
            {
                code.Add($"// @ write {x.NativeName}");
                CodeUtils.EmitValueWrite(x.Type, $"instance.{x.NativeName}", code);
                code.Add($"duk_put_prop_string(ctx, -2, \"{x.JSName}\");");
            });
        }
        protected virtual void EmitResolveSource(CodeBuilder code)
        {
            code.Add($"{AnnotationUtils.GetTypeName(Target)} result;");

            AnnotationUtils.GetVariables(Target).ForEach(x =>
            {
                code.Add($"// @ read {x.NativeName}")
                    .Add($"if(duk_get_prop_string(ctx, obj_idx, \"{x.JSName}\"))")
                    .Scope(code =>
                    {
                        CodeUtils.EmitValueRead(x.Type, x.NativeName, "duk_get_top_index(ctx)", code);
                        code.Add($"result.{x.NativeName} = {x.NativeName};");
                    })
                    .Add("duk_pop(ctx);");
            });

            code.Add("return result;");
        }

        public static StructObject Create(Type type)
        {
            if (!type.IsSubclassOf(typeof(StructObject)))
                throw new Exception("invalid struct object derived type.");
            StructObject? result = Activator.CreateInstance(type) as StructObject;
            if(result is null)
                throw new Exception("could not possible to instantiate this struct object derived type.");
            return result;
        }

        protected override void EmitGenericConstructorBody(ConstructorData ctor, CodeBuilder code)
        {
        }
        protected override void EmitConstructorBody(ConstructorData ctor, CodeBuilder code)
        {
        }
    }
}
