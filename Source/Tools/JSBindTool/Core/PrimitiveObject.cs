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
            if (type == typeof(PrimitiveObject))
                throw new Exception("Invalid Implementation. Inherited primitive object is using AbstractClass instead of Implementation.");
        }

        protected virtual void EmitResolveSignature(CodeBuilder code)
        {
            code.Add($"{AnnotationUtils.GetTypeName(Target)}& {CodeUtils.GetMethodPrefix(Target)}_resolve(duk_context* ctx, duk_idx_t stack_idx);");
        }
        protected virtual void EmitSetSignature(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetMethodPrefix(Target)}_set(duk_context* ctx, duk_idx_t obj_idx, {AnnotationUtils.GetTypeName(Target)}* target);");
        }
        protected virtual void EmitPushSignature(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetPushSignature(Target)}(duk_context* ctx, const {AnnotationUtils.GetTypeName(Target)}& value);");
        }
        protected virtual void EmitPushRefSignature(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetPushRefSignature(Target)}(duk_context* ctx, {AnnotationUtils.GetTypeName(Target)}* instance);");
        }
        protected virtual void EmitFinalizerSignature(CodeBuilder code)
        {
            code
                .Add($"void {CodeUtils.GetMethodPrefix(Target)}_set_finalizer(duk_context* ctx, duk_idx_t obj_idx, {AnnotationUtils.GetTypeName(Target)}* value);")
                .Add($"duk_idx_t {CodeUtils.GetMethodPrefix(Target)}_finalizer(duk_context* ctx);");
        }
        public override void EmitHeaderSignatures(CodeBuilder code)
        {
            EmitResolveSignature(code);
            EmitSetSignature(code);
            EmitPushSignature(code);
            EmitPushRefSignature(code);
            EmitFinalizerSignature(code);

            base.EmitHeaderSignatures(code);
        }

        public override void EmitSource(CodeBuilder code)
        {
            base.EmitSource(code);
            EmitResolveSource(code);
            EmitSetSource(code);
            EmitPushSource(code);
            EmitPushRefSource(code);
            EmitFinalizerSource(code);
        }

        public virtual void EmitResolveSource(CodeBuilder code)
        {
            code.Add($"{AnnotationUtils.GetTypeName(Target)}& {CodeUtils.GetMethodPrefix(Target)}_resolve(duk_context* ctx, duk_idx_t stack_idx)");
            code.Scope(resolveScope =>
            {
                resolveScope.Add($"{AnnotationUtils.GetTypeName(Target)}* result = nullptr;").AddNewLine();
                resolveScope.Add("if(!duk_get_prop_string(ctx, stack_idx, \"type\"))");
                resolveScope.Scope(ifScope =>
                {
                    ifScope
                        .Add("duk_pop(ctx);")
                        .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"can't resolve object '{AnnotationUtils.GetTypeName(Target)}'. Object type property is not present.\");")
                        .Add("duk_throw(ctx);");
                }).AddNewLine();
                resolveScope.Add("duk_pop(ctx);");
                resolveScope.Add("if(!duk_get_prop_string(ctx, stack_idx, JS_OBJ_HIDDEN_PTR))");
                resolveScope.Scope(code =>
                {
                    code
                        .Add("duk_pop(ctx);")
                        .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"can't resolve object '{AnnotationUtils.GetTypeName(Target)}'. Object internal pointer is null.\");")
                        .Add("duk_throw(ctx);");
                });
                resolveScope.Add($"result = static_cast<{AnnotationUtils.GetTypeName(Target)}*>(duk_get_pointer(ctx, -1));");
                resolveScope.Add("duk_pop(ctx);");
                resolveScope.Add("return *result;");
            });
        }
        public virtual void EmitSetSource(CodeBuilder code)
        {
            code
                .Add($"void {CodeUtils.GetMethodPrefix(Target)}_set(duk_context* ctx, duk_idx_t obj_idx, {AnnotationUtils.GetTypeName(Target)}* target)")
                .Scope(scope =>
                {
                    var baseType = Target.BaseType;
                    if (baseType != typeof(PrimitiveObject) && baseType != null)
                    {
                        scope
                            .Add("// set parent definition first.")
                            .Add($"{CodeUtils.GetMethodPrefix(baseType)}_set(ctx, obj_idx, target);");
                    }
                    GetVariables().ForEach(vary =>
                    {
                        scope.Add($"// set '{vary.NativeName}' property.");
                        CodeUtils.EmitValueWrite(vary.Type, $"target->{vary.NativeName}", scope);
                        scope.Add($"duk_put_prop_string(ctx, obj_idx, \"{vary.JSName}\");");
                    });
                });
        }

        protected virtual void EmitFinalizerSource(CodeBuilder code)
        {
            code
                .Add($"void {CodeUtils.GetMethodPrefix(Target)}_set_finalizer(duk_context* ctx, duk_idx_t obj_idx, {AnnotationUtils.GetTypeName(Target)}* value)")
                .Scope(code =>
                {
                    code
                        .Add($"duk_push_c_function(ctx, {CodeUtils.GetMethodPrefix(Target)}_finalizer, 0);")
                        .Add("duk_push_pointer(ctx, value);")
                        .Add("duk_put_prop_string(ctx, -2, JS_OBJ_HIDDEN_PTR);")
                        .Add("duk_set_finalizer(ctx, obj_idx);");
                })
                .Add($"duk_idx_t {CodeUtils.GetMethodPrefix(Target)}_finalizer(duk_context* ctx)")
                .Scope(code =>
                {
                    code
                        .Add("duk_push_current_function(ctx);")
                        .Add("if(duk_get_prop_string(ctx, -1, JS_OBJ_HIDDEN_PTR))")
                        .Scope(code =>
                        {
                            code
                                .Add($"{AnnotationUtils.GetTypeName(Target)}* ptr = static_cast<{AnnotationUtils.GetTypeName(Target)}*>(duk_get_pointer_default(ctx, -1, nullptr));")
                                .Add("if (ptr) delete ptr;");
                        })
                        .Add("return 0;");
                });
        }
        protected virtual void EmitPushRefSource(CodeBuilder code)
        {
            code
                .Add($"void {CodeUtils.GetPushRefSignature(Target)}(duk_context* ctx, {AnnotationUtils.GetTypeName(Target)}* instance)")
                .Scope(code =>
                {
                    code
                        .Add("if(!instance)")
                        .Scope(code =>
                        {
                            code
                                .Add("duk_push_null(ctx);")
                                .Add("return;");
                        })
                        .AddNewLine()
                        .Add("duk_push_object(ctx);")
                        .Add("duk_idx_t obj_idx = duk_get_top_index(ctx);")
                        .Add("duk_push_pointer(ctx, instance);")
                        .Add("duk_put_prop_string(ctx, obj_idx, JS_OBJ_HIDDEN_PTR);")
                        .Add($"duk_push_string(ctx, \"{AnnotationUtils.GetTypeName(Target)}\");")
                        .Add("duk_put_prop_string(ctx, obj_idx, \"type\");")
                        .Add($"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, obj_idx, instance);")
                        .Add("duk_dup(ctx, obj_idx);");
                });
        }

        protected override void EmitSourceConstructor(CodeBuilder code)
        {
            code.Add($"duk_idx_t {CodeUtils.GetMethodPrefix(Target)}_ctor(duk_context* ctx)");
            code.Scope(ctorScope =>
            {
                ctorScope.Add("duk_idx_t argc = duk_get_top(ctx);");
                ctorScope.Add($"{AnnotationUtils.GetTypeName(Target)}* instance = nullptr;").AddNewLine();
                ctorScope.Add("if(argc == 1 && duk_is_pointer(ctx, 0))");
                ctorScope.Add($"\tinstance = static_cast<{AnnotationUtils.GetTypeName(Target)}*>(duk_get_pointer(ctx, 0));");
                ctorScope.Add("else");
                ctorScope.Scope(ctorScope =>
                {
                    ctorScope.Add($"instance = new {AnnotationUtils.GetTypeName(Target)}();");
                    int idx = 0;
                    GetVariables().ForEach(vary =>
                    {
                        ctorScope.Add($"if(argc > {idx})");
                        ctorScope.Scope(ifScope =>
                        {
                            CodeUtils.EmitValueRead(vary.Type, "value", idx.ToString(), ifScope);
                            ifScope.Add($"instance->{vary.NativeName} = value;");
                        });
                        ++idx;
                    });
                });

                ctorScope.AddNewLine().Add(
                    "duk_push_this(ctx);",
                    "duk_idx_t this_idx = duk_get_top(ctx) - 1;"
                ).AddNewLine();


                ctorScope.Add($"duk_push_string(ctx, \"{AnnotationUtils.GetTypeName(Target)}\");");
                ctorScope.Add("duk_put_prop_string(ctx, this_idx, \"type\");");
                ctorScope
                    .Add("duk_push_pointer(ctx, instance);")
                    .Add("duk_put_prop_string(ctx, this_idx, JS_OBJ_HIDDEN_PTR);")
                    .Add("// setup finalizer");
                ctorScope.Add($"{CodeUtils.GetMethodPrefix(Target)}_set_finalizer(ctx, this_idx, instance);");

                ctorScope.Add($"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, this_idx, instance);");

                ctorScope.AddNewLine();

                ctorScope.Add("duk_push_this(ctx);");
                ctorScope.Add("return 1;");
            });
        }
        public virtual void EmitPushSource(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetPushSignature(Target)}(duk_context* ctx, const {AnnotationUtils.GetTypeName(Target)}& value)");
            code.Scope(scope =>
            {
                scope.Add($"duk_get_global_string(ctx, \"{AnnotationUtils.GetTypeName(Target)}\");");
                var variables = GetVariables();
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
                EmitStaticMethods(setupScope, "top");
            });
        }

        private void EmitStaticFields(CodeBuilder code, string accessor)
        {
            Target.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.GetCustomAttribute<FieldAttribute>() != null)
                .ToList()
                .ForEach(field => EmitStaticField(field, code, accessor));

            Target
                .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(x => x.GetCustomAttribute<CustomFieldAttribute>() != null)
                .ToList()
                .ForEach(method => EmitCustomStaticField(method, code, accessor));
        }
        private void EmitStaticField(FieldInfo field, CodeBuilder code, string accessor)
        {
            FieldAttribute fieldAttr = AnnotationUtils.GetFieldAttribute(field);
            code.Add($"{CodeUtils.GetMethodPrefix(Target)}_push(ctx, {AnnotationUtils.GetTypeName(field.FieldType)}::{fieldAttr.NativeName});");
            code.Add($"duk_put_prop_string(ctx, {accessor}, \"{fieldAttr.JSName}\");");
        }
        private void EmitCustomStaticField(MethodInfo method, CodeBuilder code, string accessor)
        {
            CustomFieldAttribute attr = AnnotationUtils.GetCustomFieldAttribute(method);
            method.Invoke(this, new object[] { code });
            code.Add($"duk_put_prop_string(ctx, {accessor}, \"{attr.JSName}\");");
        }

        //protected override void EmitMethodBody(MethodInfo methodInfo, CodeBuilder code, bool emitValidations = true)
        //{
        //    base.EmitMethodBody(methodInfo, code, emitValidations);

        //    string nativeName = AnnotationUtils.GetMethodNativeName(methodInfo);

        //    code.Add("duk_push_this(ctx);");
        //    code.Add($"{AnnotationUtils.GetTypeName(Target)}* instance = {GetRefSignature()}(ctx, duk_get_top_index(ctx));");
        //    code.Add("duk_pop(ctx);");

        //    CustomCodeAttribute? customCodeAttr = methodInfo.GetCustomAttribute<CustomCodeAttribute>();
        //    if (customCodeAttr != null)
        //    {
        //        EmitCustomCodeMethodBody(customCodeAttr, methodInfo, code);
        //        return;
        //    }

        //    var parameters = methodInfo.GetParameters();

        //    StringBuilder argsCall = new StringBuilder();
        //    for (int i = 0; i < parameters.Length; ++i)
        //    {
        //        CodeUtils.EmitValueRead(parameters[i].ParameterType, $"arg{i}", i.ToString(), code);
        //        argsCall.Append($"arg{i}");
        //        if (i < parameters.Length - 1)
        //            argsCall.Append(", ");
        //    }

        //    if (methodInfo.ReturnType == typeof(void))
        //    {
        //        code.Add($"instance->{nativeName}({argsCall});");
        //        code.Add("return 0;");
        //    }
        //    else
        //    {
        //        code.Add($"{CodeUtils.GetNativeDeclaration(methodInfo.ReturnType)} result = instance->{nativeName}({argsCall});");
        //        CodeUtils.EmitValueWrite(methodInfo.ReturnType, "result", code);
        //        code.Add("return 1;");
        //    }
        //}
        //protected override void EmitOperatorMethodBody(OperatorType opType, MethodInfo method, CodeBuilder code)
        //{
        //    base.EmitOperatorMethodBody(opType, method, code);

        //    string operatorSignal = GetOperatorSignal(opType);

        //    code
        //        .Add("duk_push_this(ctx);")
        //        .Add($"{AnnotationUtils.GetTypeName(Target)} instance = {CodeUtils.GetMethodPrefix(Target)}_resolve(ctx, -1);")
        //        .Add("duk_pop(ctx);")
        //        .AddNewLine();

        //    CustomCodeAttribute? customCodeAttr = method.GetCustomAttribute<CustomCodeAttribute>();
        //    if (customCodeAttr != null)
        //    {
        //        EmitCustomCodeMethodBody(customCodeAttr, method, code);
        //        return;
        //    }

        //    var parameters = method.GetParameters();

        //    if (parameters.Length > 1)
        //        throw new Exception($"Operators that contains more than 1 argument must add custom code attribute. Error Class: {Target.FullName}");

        //    CodeUtils.EmitValueRead(parameters[0].ParameterType, $"value", "0", code);

        //    if (opType == OperatorType.Equal)
        //    {
        //        code
        //            .Add($"duk_push_boolean(ctx, instance == value);")
        //            .Add("return 1;");
        //        return;
        //    }

        //    code.Add($"{CodeUtils.GetNativeDeclaration(method.ReturnType)} result = instance {operatorSignal} value;");
        //    CodeUtils.EmitValueWrite(method.ReturnType, "result", code);
        //    code.Add("return 1;");
        //}

        //protected override void EmitProperty(PropertyInfo prop, string accessor, CodeBuilder code)
        //{
        //    List<string> enumFlags = new List<string>();
        //    enumFlags.Add("DUK_DEFPROP_HAVE_ENUMERABLE");

        //    code.Add($"duk_push_string(ctx, \"{CodeUtils.ToCamelCase(AnnotationUtils.GetJSPropertyName(prop))}\");");

        //    var attr = AnnotationUtils.GetPropertyMap(prop);
        //    if (prop.GetMethod != null && !string.IsNullOrEmpty(attr.GetterName))
        //    {
        //        code.LightFunction(getterScope =>
        //        {
        //            getterScope.Add(
        //                "duk_push_this(ctx);",
        //                $"{AnnotationUtils.GetTypeName(Target)} instance = {CodeUtils.GetMethodPrefix(Target)}_resolve(ctx, -1);",
        //                "duk_pop(ctx);",
        //                $"{CodeUtils.GetNativeDeclaration(prop.PropertyType)} result = instance.{AnnotationUtils.GetGetterName(prop)}();"
        //            );
        //            CodeUtils.EmitValueWrite(prop.PropertyType, "result", getterScope);

        //            getterScope.Add("return 1;");
        //        }, "0");

        //        enumFlags.Add("DUK_DEFPROP_HAVE_GETTER");
        //    }
        //    if (prop.SetMethod != null && !string.IsNullOrEmpty(attr.SetterName))
        //    {
        //        code.LightFunction(setterScope =>
        //        {
        //            setterScope.Add(
        //                "duk_push_this(ctx);",
        //                $"{AnnotationUtils.GetTypeName(Target)} instance = {CodeUtils.GetMethodPrefix(Target)}_resolve(ctx, -1);",
        //                "duk_pop(ctx);"
        //            );
        //            CodeUtils.EmitValueRead(prop.PropertyType, "result", "0", setterScope);

        //            setterScope.AddNewLine().Add($"instance.{AnnotationUtils.GetSetterName(prop)}(result);");
        //            setterScope.Add("return 0;");
        //        }, "1");

        //        enumFlags.Add("DUK_DEFPROP_HAVE_SETTER");
        //    }

        //    StringBuilder propFlags = new StringBuilder();
        //    for (int i = 0; i < enumFlags.Count; ++i)
        //    {
        //        propFlags.Append(enumFlags[i]);
        //        if (i < enumFlags.Count - 1)
        //            propFlags.Append(" | ");
        //    }

        //    code.Add($"duk_def_prop(ctx, {accessor}, {propFlags});");
        //}

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
