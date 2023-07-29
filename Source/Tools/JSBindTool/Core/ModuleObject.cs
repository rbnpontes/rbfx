using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public abstract class ModuleObject : BaseObject
    {
        public ModuleObject(Type type) : base(type) { }

        public override void EmitHeaderSignatures(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetMethodPrefix(Target)}_setup(duk_context* ctx);");
            EmitMethodSignatures(code);
        }
        public override void EmitSource(CodeBuilder code)
        {
            EmitSourceMethodLookupTable(code);
            EmitSourceSetup(code);
            EmitSourceMethods(code);
        }
        protected override void EmitSourceSetup(CodeBuilder code)
        {
            base.EmitSourceSetup(code);
            code.Scope(code =>
            {
                NamespaceAttribute? namespaceAttr = Target.GetCustomAttribute<NamespaceAttribute>();

                code.Add("duk_push_global_object(ctx);");

                if (namespaceAttr != null)
                {
                    string[] parts = namespaceAttr.Namespace.Split('.');
                    foreach (string part in parts)
                    {
                        code
                            .Add($"// search namespace part: {part}")
                            .Add($"if(!duk_get_prop_string(ctx, duk_get_top_index(ctx), \"{part}\"))")
                            .Scope(code =>
                            {
                                code
                                    .Add("duk_pop(ctx);")
                                    .Add("duk_push_object(ctx);")
                                    .Add("duk_dup(ctx, -1);")
                                    .Add($"duk_put_prop_string(ctx, -3, \"{part}\");");
                            });
                    }
                }

                code.Add("duk_idx_t top = duk_get_top_index(ctx);");
                EmitMethods(code, "top");
                code.Add("duk_pop(ctx);");
            });
        }

        protected override void EmitMethodBody(MethodInfo methodInfo, CodeBuilder code, bool emitValidations = true)
        {
            var parameters = methodInfo.GetParameters().ToList();
            if (emitValidations)
                EmitArgumentValidation(parameters, methodInfo.GetCustomAttribute<CustomCodeAttribute>(), code);

            string nativeName = AnnotationUtils.GetMethodNativeName(methodInfo);

            CustomCodeAttribute? customCodeAttr = methodInfo.GetCustomAttribute<CustomCodeAttribute>();
            if(customCodeAttr != null)
            {
                EmitCustomCodeMethodBody(customCodeAttr, methodInfo, code);
                return;
            }


            StringBuilder argsCall = new StringBuilder();
            for (int i = 0; i < parameters.Count; ++i)
            {
                CodeUtils.EmitValueRead(parameters[i].ParameterType, $"arg{i}", i.ToString(), code);
                argsCall.Append($"arg{i}");
                if (i < parameters.Count - 1)
                    argsCall.Append(", ");
            }

            if (methodInfo.ReturnType == typeof(void))
            {
                code.Add($"{nativeName}({argsCall});");
                code.Add("return 0;");
                return;
            }

            code.Add($"{CodeUtils.GetNativeDeclaration(methodInfo.ReturnType)} result = {nativeName}({argsCall});");
            CodeUtils.EmitValueWrite(methodInfo.ReturnType, "result", code);
            code.Add("return 1;");
        }

        #region Skipped Methods
        protected override void EmitSourceGetRef(CodeBuilder code) { }
        protected override void EmitSourceProperties(CodeBuilder code) { }
        protected override void EmitSourceConstructor(CodeBuilder code)
        {
        }
        #endregion

        public static ModuleObject Create(Type type)
        {
            if (!type.IsSubclassOf(typeof(ModuleObject)))
                throw new Exception("invalid module object derived type.");
            ModuleObject? result = Activator.CreateInstance(type) as ModuleObject;
            if (result is null)
                throw new Exception("could not possible to instantiate this primitive object derived type.");
            return result;
        }
    }
}
