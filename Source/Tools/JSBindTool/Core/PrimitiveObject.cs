using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class PrimitiveObject
    {
        class Variable
        {
            public string NativeName { get; set; } = string.Empty;
            public string JSName { get; set; } = string.Empty;
            public Type Type { get; set; }
            public Variable(Type type)
            {
                Type = type;
            }
        }

        public Type Target { get; private set; }
        public PrimitiveObject(Type type)
        {
            Target = type;
        }

        public virtual void EmitHeaderIncludes(CodeBuilder code)
        {
            code.Add(AnnotationUtils.GetIncludes(Target).Select(x => $"#include <{x}>"));
            code.Add("#include <duktape/duktape.h>");
            code.Add("#include <Urho3D/JavaScript/JavaScriptOperations.h>");
        }
        public virtual void EmitResolveSignature(CodeBuilder code)
        {
            code.Add($"{Target.Name} {Target.Name}_resolve(duk_context* ctx, duk_idx_t stack_idx);");
        }
        public virtual void EmitConstructorSignature(CodeBuilder code)
        {
            code.Add($"duk_idx_t {Target.Name}_constructor(duk_context* ctx);");
        }
        public virtual void EmitSetupSignature(CodeBuilder code)
        {
            code.Add($"void {Target.Name}_setup(duk_context* ctx);");
        }

        public virtual void EmitSourceIncludes(CodeBuilder code)
        {
            code.Add($"#include \"{Target.Name}_Primitive.h\"");
            code.Add($"#include <Urho3D/IO/Log.h>");
        }
        public virtual void EmitResolveSource(CodeBuilder code)
        {
            code.Add($"{Target.Name} {Target.Name}_resolve(duk_context* ctx, duk_idx_t stack_idx)");
            code.Scope(resolveScope =>
            {
                resolveScope.Add($"{Target.Name} result;").AddNewLine();
                resolveScope.Add("if(!duk_get_prop_string(ctx, stack_idx, \"type\"))");
                resolveScope.Scope(ifScope =>
                {
                    ifScope.Add("duk_pop(ctx);");
                    ifScope.Add($"URHO3D_LOGWARNING(\"invalid '{Target.Name}' type. resolving to default!\");");
                    ifScope.Add("return result;");
                }).AddNewLine();
                resolveScope.Add("StringHash currType = rbfx_get_string_hash(ctx, -1);");
                resolveScope.Add("duk_pop(ctx);");
                resolveScope.Add($"if(currType != StringHash(\"{Target.Name}\"))");
                resolveScope.Scope(ifScope =>
                {
                    ifScope.Add($"URHO3D_LOGWARNING(\"invalid '{Target.Name}' type. resolving to default!\");");
                    ifScope.Add("return result;");
                }).AddNewLine();

                EmitObjectRead(resolveScope, "stack_idx");

                resolveScope.Add("return result;");
            });
        }
        public virtual void EmitConstructorSource(CodeBuilder code)
        {
            code.Add($"duk_idx_t {Target.Name}_constructor(duk_context* ctx)");
            code.Scope(ctorScope =>
            {
                ctorScope.Add("duk_idx_t argc = duk_get_top(ctx);");
                ctorScope.Add($"{Target.Name} instance;").AddNewLine();
                int idx = 0;
                CollectVariables().ForEach(vary =>
                {
                    ctorScope.Add("if(argc > 0)");
                    ctorScope.Scope(ifScope =>
                    {
                        CodeUtils.EmitValueRead(vary.Type, "value", idx.ToString(), ifScope);
                        ifScope.Add($"instance.{vary.NativeName} = value;");
                    });
                    ++idx;
                });

                ctorScope.AddNewLine().Add(
                    "duk_push_this(ctx);",
                    "duk_idx_t this_idx = duk_get_top(ctx) - 1;"
                ).AddNewLine();
                CollectVariables().ForEach(vary =>
                {
                    ctorScope.Scope(writeScope =>
                    {
                        CodeUtils.EmitValueWrite(vary.Type, $"instance.{vary.NativeName}", writeScope);
                        writeScope.Add($"duk_put_prop_string(ctx, this_idx, \"{vary.JSName}\");");
                    });
                });

                ctorScope.AddNewLine();

                ctorScope.Add($"duk_push_string(ctx, \"{AnnotationUtils.GetClassName(Target)}\");");
                ctorScope.Add("duk_put_prop_string(ctx, this_idx, \"type\");");

                EmitMethods(ctorScope);
                ctorScope.AddNewLine();

                ctorScope.Add("duk_push_this(ctx);");
                ctorScope.Add("return 1;");
            });
        }
        public virtual void EmitSetupSetupSource(CodeBuilder code)
        {
            code.Add($"void {Target.Name}_setup(duk_context* ctx)");
            code.Scope(setupScope =>
            {
                setupScope.Add($"duk_push_c_function(ctx, {Target.Name}_constructor, DUK_VARARGS);");
                setupScope.Add($"duk_put_global_string(ctx, \"{Target.Name}\");");
            });
        }

        private void EmitObjectRead(CodeBuilder code, string accessor)
        {
            CollectVariables().ForEach(vary =>
            {
                code.Add($"// variable {vary.NativeName} read");
                code.Scope(scope =>
                {
                    scope.Add($"duk_get_prop_string(ctx, {accessor}, \"{vary.JSName}\");");
                    CodeUtils.EmitValueRead(vary.Type, "value", "-1", scope);
                    scope.Add("duk_pop(ctx);");
                    scope.Add($"result.{vary.NativeName} = value;");
                });
            });
        }
        private void EmitMethods(CodeBuilder code)
        {
            OperatorFlags opFlags = AnnotationUtils.GetOperatorFlags(Target);
            if ((opFlags & OperatorFlags.Add) != 0)
                EmitOperatorMethod("add", "+", code);
            if ((opFlags & OperatorFlags.Sub) != 0)
                EmitOperatorMethod("sub", "-", code);
            if ((opFlags & OperatorFlags.Mul) != 0)
                EmitOperatorMethod("mul", "*", code);
            if ((opFlags & OperatorFlags.Div) != 0)
                EmitOperatorMethod("div", "/", code);
            if ((opFlags & OperatorFlags.Equal) != 0)
                EmitOperatorMethod("equal", "==", code);

            //var methods = Target.GetMethods().ToList();
            //methods.ForEach(method =>
            //{
            //});
        }
        private void EmitOperatorMethod(string methodName, string operatorSignal, CodeBuilder code)
        {
            code.Function(scope =>
            {
                scope.Add("duk_push_this(ctx);");
                scope.Add($"{Target.Name} value = {Target.Name}_resolve(ctx, 0);");
                if (string.Equals(operatorSignal, "=="))
                {
                    scope.Add($"duk_push_boolean(ctx, {Target.Name}_resolve(ctx, 1) == value);");
                    scope.Add("return 1;");
                }
                else
                {
                    scope.Add($"{Target.Name} result = {Target.Name}_resolve(ctx, 1) {operatorSignal} value;").AddNewLine();

                    scope.Add($"duk_get_global_string(ctx, \"{Target.Name}\");");
                    var variables = CollectVariables();
                    variables.ForEach(vary =>
                    {
                        CodeUtils.EmitValueWrite(vary.Type, $"result.{vary.NativeName}", scope);
                    });

                    scope.Add($"duk_new(ctx, {variables.Count});").AddNewLine();
                    scope.Add("return 1;");
                }
            }, "1");
            code.Add($"duk_put_prop_string(ctx, this_idx, \"{methodName}\");");
        }
        private List<Variable> CollectVariables()
        {
            List<Variable> variables = new List<Variable>();
            Target.GetProperties().Where(x => !string.Equals(x.Name, "Target")).ToList().ForEach(prop =>
            {
                Variable vary = new Variable(prop.PropertyType);
                vary.NativeName = AnnotationUtils.GetVariableName(prop);
                vary.JSName = CodeUtils.ToCamelCase(vary.NativeName);
                variables.Add(vary);
            });
            Target.GetFields().ToList().ForEach(field =>
            {
                Variable vary = new Variable(field.FieldType);
                vary.NativeName = AnnotationUtils.GetVariableName(field);
                vary.JSName = CodeUtils.ToCamelCase(field.Name);
                variables.Add(vary);
            });

            return variables;
        }

        public static PrimitiveObject Create(Type type)
        {
            if (!type.IsSubclassOf(typeof(PrimitiveObject)))
                throw new Exception("invalid primitive object derived type.");
            PrimitiveObject? result = Activator.CreateInstance(type) as PrimitiveObject;
            if (result is null)
                throw new Exception("could not possible to instantiate this primitive object derived type.");
            return result;
        }
    }
}
