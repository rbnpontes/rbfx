using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace JSBindTool.Core
{
    public class PrimitiveObject : BaseObject
    {
        public PrimitiveObject(Type type) : base(type)
        {
        }

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

        protected virtual void EmitResolveSignature(CodeBuilder code)
        {
            code.Add($"{Target.Name} {CodeUtils.GetMethodPrefix(Target)}_resolve(duk_context* ctx, duk_idx_t stack_idx);");
        }
        protected virtual void EmitSetSignature(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetMethodPrefix(Target)}_set(duk_context* ctx, duk_idx_t obj_idx, const {AnnotationUtils.GetTypeName(Target)}& target);");
        }
        protected virtual void EmitPushSignature(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetMethodPrefix(Target)}_push(duk_context* ctx, const {AnnotationUtils.GetTypeName(Target)}& value);");
        }

        public override void EmitHeaderSignatures(CodeBuilder code)
        {
            EmitResolveSignature(code);
            EmitSetSignature(code);
            EmitPushSignature(code);

            base.EmitHeaderSignatures(code);
        }
        protected override void EmitMethodSignatures(CodeBuilder code)
        {
            OperatorFlags opFlags = AnnotationUtils.GetOperatorFlags(Target);
            if (opFlags != OperatorFlags.None)
                code.Add("//@ operators definitions");

            if ((opFlags & OperatorFlags.Add) != 0)
                code.Add($"duk_idx_t {GetOpMethodSignature(OperatorType.Add)}(duk_context* ctx);");
            if ((opFlags & OperatorFlags.Sub) != 0)
                code.Add($"duk_idx_t {GetOpMethodSignature(OperatorType.Sub)}(duk_context* ctx);");
            if ((opFlags & OperatorFlags.Mul) != 0)
                code.Add($"duk_idx_t {GetOpMethodSignature(OperatorType.Mul)}(duk_context* ctx);");
            if ((opFlags & OperatorFlags.Div) != 0)
                code.Add($"duk_idx_t {GetOpMethodSignature(OperatorType.Div)}(duk_context* ctx);");
            if ((opFlags & OperatorFlags.Equal) != 0)
                code.Add($"duk_idx_t {GetOpMethodSignature(OperatorType.Equal)}(duk_context* ctx);");

            base.EmitMethodSignatures(code);
        }


        public override void EmitSource(CodeBuilder code)
        {
            base.EmitSource(code);
            EmitResolveSource(code);
            EmitSetSource(code);
            EmitPushSource(code);
        }

        public override void EmitSourceIncludes(CodeBuilder code)
        {
            code.Add($"#include \"{Target.Name}{Constants.PrimitiveIncludeSuffix}.h\"");
            code.Add($"#include <Urho3D/IO/Log.h>");
        }

        public virtual void EmitResolveSource(CodeBuilder code)
        {
            code.Add($"{Target.Name} {CodeUtils.GetMethodPrefix(Target)}_resolve(duk_context* ctx, duk_idx_t stack_idx)");
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
        public virtual void EmitSetSource(CodeBuilder code)
        {
            code
                .Add($"void {CodeUtils.GetMethodPrefix(Target)}_set(duk_context* ctx, duk_idx_t obj_idx, const {AnnotationUtils.GetTypeName(Target)}& target)")
                .Scope(scope =>
                {
                    CollectVariables().ForEach(vary =>
                    {
                        scope.Add($"// set '{vary.NativeName}' property.");
                        CodeUtils.EmitValueWrite(vary.Type, $"target.{vary.NativeName}", scope);
                        scope.Add($"duk_put_prop_string(ctx, obj_idx, \"{vary.JSName}\");");
                    });
                });
        }

        protected override void EmitSourceConstructor(CodeBuilder code)
        {
            code.Add($"duk_idx_t {CodeUtils.GetMethodPrefix(Target)}_ctor(duk_context* ctx)");
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

                ctorScope
                    .Add($"{CodeUtils.GetMethodPrefix(Target)}_set(ctx, this_idx, instance);");

                ctorScope.AddNewLine();

                ctorScope.Add($"duk_push_string(ctx, \"{AnnotationUtils.GetTypeName(Target)}\");");
                ctorScope.Add("duk_put_prop_string(ctx, this_idx, \"type\");");

                EmitMethods(ctorScope, "this_idx");
                ctorScope.AddNewLine();

                ctorScope.Add("duk_push_this(ctx);");
                ctorScope.Add("return 1;");
            });
        }
        protected override void EmitSourceMethods(CodeBuilder code)
        {
            OperatorFlags opFlags = AnnotationUtils.GetOperatorFlags(Target);
            if ((opFlags & OperatorFlags.Add) != 0)
                EmitOperatorMethodBody(OperatorType.Add, code);
            if ((opFlags & OperatorFlags.Sub) != 0)
                EmitOperatorMethodBody(OperatorType.Sub, code);
            if ((opFlags & OperatorFlags.Mul) != 0)
                EmitOperatorMethodBody(OperatorType.Mul, code);
            if ((opFlags & OperatorFlags.Div) != 0)
                EmitOperatorMethodBody(OperatorType.Div, code);
            if ((opFlags & OperatorFlags.Equal) != 0)
                EmitOperatorMethodBody(OperatorType.Equal, code);

            base.EmitSourceMethods(code);
        }

        public virtual void EmitPushSource(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetMethodPrefix(Target)}_push(duk_context* ctx, const {AnnotationUtils.GetTypeName(Target)}& value)");
            code.Scope(scope =>
            {
                scope.Add($"duk_get_global_string(ctx, \"{AnnotationUtils.GetTypeName(Target)}\");");
                var variables = CollectVariables();
                variables.ForEach(vary =>
                {
                    CodeUtils.EmitValueWrite(vary.Type, "value."+vary.NativeName, scope);
                });
                scope.Add($"duk_new(ctx, {variables.Count});");
            });
        }
        protected override void EmitSourceSetup(CodeBuilder code)
        {
            base.EmitSourceSetup(code);
            code.Scope(setupScope =>
            {
                setupScope.Add("duk_idx_t top = duk_get_top(ctx);");
                setupScope.Add($"duk_push_c_function(ctx, {CodeUtils.GetMethodPrefix(Target)}_ctor, DUK_VARARGS);");
                setupScope.Add("duk_dup(ctx, -1);");
                setupScope.Add($"duk_put_global_string(ctx, \"{AnnotationUtils.GetTypeName(Target)}\");");
                setupScope.Add("// setup static fields");
                EmitStaticFields(setupScope, "top");
            });
        }

        private void EmitStaticFields(CodeBuilder code, string accessor)
        {
            Target.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.GetCustomAttribute<FieldAttribute>() != null)
                .ToList()
                .ForEach(field => EmitStaticField(field, code, accessor));
        }
        private void EmitStaticField(FieldInfo field, CodeBuilder code, string accessor)
        {
            FieldAttribute fieldAttr = AnnotationUtils.GetFieldAttribute(field);

            code.Add($"{CodeUtils.GetMethodPrefix(Target)}_push(ctx, {AnnotationUtils.GetTypeName(field.FieldType)}::{fieldAttr.NativeName});");
            code.Add($"duk_put_prop_string(ctx, {accessor}, \"{fieldAttr.JSName}\");");
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
        protected override void EmitMethods(CodeBuilder code, string accessor)
        {
            OperatorFlags opFlags = AnnotationUtils.GetOperatorFlags(Target);
            if (opFlags != OperatorFlags.None)
                code.Add("// operators setup");
            if ((opFlags & OperatorFlags.Add) != 0)
                EmitOperatorMethod(OperatorType.Add, code);
            if ((opFlags & OperatorFlags.Sub) != 0)
                EmitOperatorMethod(OperatorType.Sub, code);
            if ((opFlags & OperatorFlags.Mul) != 0)
                EmitOperatorMethod(OperatorType.Mul, code);
            if ((opFlags & OperatorFlags.Div) != 0)
                EmitOperatorMethod(OperatorType.Div, code);
            if ((opFlags & OperatorFlags.Equal) != 0)
                EmitOperatorMethod(OperatorType.Equal, code);

            base.EmitMethods(code, accessor);
        }
        private void EmitOperatorMethod(OperatorType operatorType, CodeBuilder code)
        {
            code
                .Add($"duk_push_c_lightfunc(ctx, {GetOpMethodSignature(operatorType)}, 1, 1, 0);")
                .Add($"duk_put_prop_string(ctx, this_idx, \"{GetOpName(operatorType)}\");");
        }

        protected override void EmitMethodBody(MethodInfo methodInfo, CodeBuilder code, bool emitValidations = true)
        {
            base.EmitMethodBody(methodInfo, code, emitValidations);

            string nativeName = AnnotationUtils.GetMethodNativeName(methodInfo);

            code.Add("duk_push_this(ctx);");
            code.Add($"{AnnotationUtils.GetTypeName(Target)} value = {CodeUtils.GetMethodPrefix(Target)}_resolve(ctx, -1);");
            code.Add("duk_pop(ctx);");

            var parameters = methodInfo.GetParameters();

            StringBuilder argsCall = new StringBuilder();
            for (int i = 0; i < parameters.Length; ++i)
            {
                CodeUtils.EmitValueRead(parameters[i].ParameterType, $"arg{i}", i.ToString(), code);
                argsCall.Append($"arg{i}");
                if (i < parameters.Length - 1)
                    argsCall.Append(", ");
            }

            if (methodInfo.ReturnType == typeof(void))
            {
                code.Add($"value.{nativeName}({argsCall});");
                // when return type is void, must update JS properties instead.
                code.Add("duk_push_this(ctx);");
                code.Add($"{CodeUtils.GetMethodPrefix(Target)}_set(ctx, duk_get_top(ctx) - 1, value);");
                code.Add("return 0;");
            }
            else
            {
                code.Add($"{CodeUtils.GetNativeDeclaration(methodInfo.ReturnType)} result = value.{nativeName}({argsCall});");
                CodeUtils.EmitValueWrite(methodInfo.ReturnType, "result", code);
                code.Add("return 1;");
            }
        }
        private void EmitOperatorMethodBody(OperatorType opType, CodeBuilder code)
        {
            string operatorSignal = GetOpSignal(opType);
            code.Add($"duk_idx_t {GetOpMethodSignature(opType)}(duk_context* ctx)");
            code.Scope(scope =>
            {
                scope.Add("duk_push_this(ctx);");
                scope.Add($"{AnnotationUtils.GetTypeName(Target)} value ={CodeUtils.GetMethodPrefix(Target)}_resolve(ctx, 0);");
                if (string.Equals(operatorSignal, "=="))
                {
                    scope.Add($"duk_push_boolean(ctx, {CodeUtils.GetMethodPrefix(Target)}_resolve(ctx, 1) == value);");
                    scope.Add("return 1;");
                }
                else
                {
                    scope.Add($"{AnnotationUtils.GetTypeName(Target)} result = {CodeUtils.GetMethodPrefix(Target)}_resolve(ctx, 1) {operatorSignal} value;").AddNewLine();

                    scope.Add($"duk_get_global_string(ctx, \"{Target.Name}\");");
                    var variables = CollectVariables();
                    variables.ForEach(vary =>
                    {
                        CodeUtils.EmitValueWrite(vary.Type, $"result.{vary.NativeName}", scope);
                    });

                    scope.Add($"duk_new(ctx, {variables.Count});").AddNewLine();
                    scope.Add("return 1;");
                }
            });
        }

        private List<Variable> CollectVariables()
        {
            List<Variable> variables = new List<Variable>();
            Target.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).Where(x => !string.Equals(x.Name, "Target")).ToList().ForEach(prop =>
            {
                Variable vary = new Variable(prop.PropertyType);
                vary.NativeName = AnnotationUtils.GetVariableName(prop);
                vary.JSName = CodeUtils.ToCamelCase(vary.NativeName);
                variables.Add(vary);
            });
            Target.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).ToList().ForEach(field =>
            {
                Variable vary = new Variable(field.FieldType);
                vary.NativeName = AnnotationUtils.GetVariableName(field);
                vary.JSName = CodeUtils.ToCamelCase(field.Name);
                variables.Add(vary);
            });

            return variables;
        }


        private string GetOpMethodSignature(OperatorType opType)
        {
            return $"{CodeUtils.GetMethodPrefix(Target)}_op_{GetOpName(opType)}_call";
        }
        private string GetOpName(OperatorType opType)
        {
            string op = string.Empty;
            switch (opType)
            {
                case OperatorType.Add:
                    op = "add";
                    break;
                case OperatorType.Sub:
                    op = "sub";
                    break;
                case OperatorType.Mul:
                    op = "mul";
                    break;
                case OperatorType.Div:
                    op = "div";
                    break;
                case OperatorType.Equal:
                    op = "equal";
                    break;
            }
            return op;
        }
        private string GetOpSignal(OperatorType opType)
        {
            string op = string.Empty;
            switch (opType)
            {
                case OperatorType.Add:
                    op = "+";
                    break;
                case OperatorType.Sub:
                    op = "-";
                    break;
                case OperatorType.Mul:
                    op = "*";
                    break;
                case OperatorType.Div:
                    op = "/";
                    break;
                case OperatorType.Equal:
                    op = "==";
                    break;
            }
            return op;
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
