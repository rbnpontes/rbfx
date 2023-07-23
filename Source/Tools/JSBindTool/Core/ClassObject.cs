using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace JSBindTool.Core
{
    [Abstract]
    [Include("Urho3D/Core/Object.h")]
    public class ClassObject : BaseObject
    {
        public ClassObject(Type type) : base(type)
        {
        }

        public override void EmitHeaderSignatures(CodeBuilder code)
        {
            base.EmitHeaderSignatures(code);
            code.Add($"void {CodeUtils.GetMethodPrefix(Target)}_wrap(duk_context* ctx, duk_idx_t obj_idx, {AnnotationUtils.GetTypeName(Target)}* instance);");
        }

        public override void EmitSource(CodeBuilder code)
        {
            EmitSourceWrap(code);
            base.EmitSource(code);
        }

        protected override void EmitSourceSetup(CodeBuilder code)
        {
            base.EmitSourceSetup(code);
            code.Scope(scope =>
            {
                // Abstract methods must generate as wrapper instead of constructor like
                if(AnnotationUtils.IsAbstract(Target))
                {
                    scope
                        .Add($"duk_push_c_function(ctx, {CodeUtils.GetMethodPrefix(Target)}_ctor, 1);")
                        .Add($"duk_put_global_string(ctx, \"{AnnotationUtils.GetTypeName(Target)}\");");
                }
                else
                {
                    scope
                        .Add($"duk_push_c_function(ctx, {CodeUtils.GetMethodPrefix(Target)}_ctor, DUK_VARARGS);")
                        .Add($"duk_put_global_string(ctx, \"{AnnotationUtils.GetTypeName(Target)}\");");
                }
            });
        }
        protected override void EmitSourceConstructor(CodeBuilder code)
        {
            code.Add($"duk_idx_t {CodeUtils.GetMethodPrefix(Target)}_ctor(duk_context* ctx)");
            code.Scope(scope =>
            {
                if (AnnotationUtils.IsAbstract(Target))
                {
                    // generate wrapper code for abstract classes
                    scope
                        .Add($"{AnnotationUtils.GetTypeName(Target)}* instance = static_cast<{AnnotationUtils.GetTypeName(Target)}*>(duk_require_pointer(ctx, 0));")
                        .Add($"duk_push_this(ctx);")
                        .Add($"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, duk_get_top(ctx) - 1, instance);")
                        .Add("return 1;");
                }
                else
                {
                    scope.Add("if (!duk_is_constructor_call(ctx))");
                    scope.Scope(ifScope =>
                    {
                        ifScope.Add("duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid constructor call. must call with 'new' keyword.\");");
                        ifScope.Add("return duk_throw(ctx);");
                    }).AddNewLine();

                    scope.Add($"{AnnotationUtils.GetTypeName(Target)}* instance = nullptr;");
                    scope.Add("duk_idx_t obj_idx = duk_get_top(ctx);");
                    scope.Add("if(obj_idx > 1)");
                    scope.Scope(ifScope =>
                    {
                        ifScope.Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid '{AnnotationUtils.GetTypeName(Target)}' constructor call.\");");
                        ifScope.Add("return duk_throw(ctx);");
                    }).AddNewLine();


                    scope.Add("duk_push_this(ctx);");
                    scope.Add("// if has argument, the first argument must be a pointer to Object*");
                    scope.Add("if(obj_idx == 0)");
                    scope.Scope(ifScope => ifScope.Add($"instance = new {AnnotationUtils.GetTypeName(Target)}(JavaScriptSystem::GetContext());"));
                    scope.Add("else");
                    scope.Scope(ifScope =>
                    {
                        ifScope.Add($"instance = static_cast<{AnnotationUtils.GetTypeName(Target)}*>(duk_require_pointer(ctx, 0));");
                        ifScope.Add("if(!instance)");
                        ifScope.Scope(nestedIfScope =>
                        {
                            nestedIfScope.Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"pointer argument is null\");");
                            nestedIfScope.Add("return duk_throw(ctx);");
                        });
                    }).AddNewLine();

                    scope.Add("void* heapptr = instance->GetJSHeapptr();");
                    scope.Add("if(rbfx_is_valid_heapptr(ctx, heapptr))");
                    scope.Scope(ifScope =>
                    {
                        ifScope.Add("duk_push_heapptr(ctx, heapptr);");
                        ifScope.Add($"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, duk_get_top(ctx) - 1, instance);");
                        ifScope.Add("return 1;");
                    }).AddNewLine();

                    scope.Add(
                        "heapptr = duk_get_heapptr(ctx, obj_idx);",
                        "instance->SetJSHeapptr(heapptr);",
                        "instance->AddRef();",
                        "rbfx_lock_heapptr(ctx, heapptr);"
                    ).AddNewLine();

                    scope.Add(
                        $"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, obj_idx, instance);",
                        "rbfx_set_finalizer(ctx, obj_idx, instance);"
                    ).AddNewLine();

                    scope.Add("duk_push_this(ctx);");
                    scope.Add("return 1;");
                }
            });
        }
        protected virtual void EmitSourceWrap(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetMethodPrefix(Target)}_wrap(duk_context* ctx, duk_idx_t obj_idx, {AnnotationUtils.GetTypeName(Target)}* instance)");
            code.Scope(scope =>
            {
                if (Target.BaseType == typeof(ClassObject))
                    scope.Add("rbfx_object_wrap(ctx, obj_idx, instance);");
                else if(Target.BaseType != null)
                    scope.Add($"{CodeUtils.GetMethodPrefix(Target.BaseType)}_wrap(ctx, obj_idx, instance);");
                EmitProperties(scope, "obj_idx");
                EmitMethods(scope, "obj_idx");
            });
        }
        
        protected override void EmitProperty(PropertyInfo prop, string accessor, CodeBuilder code)
        {
            List<string> enumFlags = new List<string>();
            enumFlags.Add("DUK_DEFPROP_HAVE_ENUMERABLE");

            code.AddNewLine();
            code.Add($"duk_push_string(ctx, \"{CodeUtils.ToCamelCase(AnnotationUtils.GetJSPropertyName(prop))}\");");

            var attr = AnnotationUtils.GetPropertyMap(prop);
            if(prop.GetMethod != null && !string.IsNullOrEmpty(attr.GetterName))
            {
                code.LightFunction(getterScope =>
                {
                    getterScope.Add(
                        "duk_push_this(ctx);",
                        $"{AnnotationUtils.GetTypeName(Target)}* instance = static_cast<{AnnotationUtils.GetTypeName(Target)}*>(rbfx_get_instance(ctx, -1));",
                        "duk_pop(ctx);",
                        $"{CodeUtils.GetNativeDeclaration(prop.PropertyType)} result = instance->{AnnotationUtils.GetGetterName(prop)}();"
                    );
                    CodeUtils.EmitValueWrite(prop.PropertyType, "result", getterScope);

                    getterScope.Add("return 1;");
                }, "0");

                enumFlags.Add("DUK_DEFPROP_HAVE_GETTER");
            }
            if(prop.SetMethod != null && !string.IsNullOrEmpty(attr.SetterName))
            {
                code.LightFunction(setterScope =>
                {
                    setterScope.Add(
                        "duk_push_this(ctx);",
                        $"{AnnotationUtils.GetTypeName(Target)}* instance = static_cast<{AnnotationUtils.GetTypeName(Target)}*>(rbfx_get_instance(ctx, -1));",
                        "duk_pop(ctx);"
                    );
                    CodeUtils.EmitValueRead(prop.PropertyType, "result", "0", setterScope);

                    setterScope.AddNewLine().Add($"instance->{AnnotationUtils.GetSetterName(prop)}(result);");
                    setterScope.Add("return 0;");
                }, "1");

                enumFlags.Add("DUK_DEFPROP_HAVE_SETTER");
            }

            StringBuilder propFlags = new StringBuilder();
            for(int i =0; i < enumFlags.Count; ++i)
            {
                propFlags.Append(enumFlags[i]);
                if (i < enumFlags.Count - 1)
                    propFlags.Append(" | ");
            }

            code.Add($"duk_def_prop(ctx, {accessor}, {propFlags});");
        }

        protected override void EmitMethodBody(MethodInfo methodInfo, CodeBuilder code, bool emitValidations = true)
        {
            base.EmitMethodBody(methodInfo, code, emitValidations);

            string nativeName = AnnotationUtils.GetMethodNativeName(methodInfo);

            var parameters = methodInfo.GetParameters();

            code
                .Add("duk_push_this(ctx);")
                .Add($"{AnnotationUtils.GetTypeName(Target)}* instance = static_cast<{AnnotationUtils.GetTypeName(Target)}*>(rbfx_get_instance(ctx, -1));")
                .Add("duk_pop(ctx);");

            CustomCodeAttribute? customCodeAttr = methodInfo.GetCustomAttribute<CustomCodeAttribute>();
            if(customCodeAttr != null)
            {
                EmitCustomCodeMethodBody(customCodeAttr, methodInfo, code);
                return;
            }

            StringBuilder argsCall = new StringBuilder();
            for(int i =0; i < parameters.Length; ++i)
            {
                CodeUtils.EmitValueRead(parameters[i].ParameterType, $"arg{i}", i.ToString(), code);
                argsCall.Append($"arg{i}");
                if (i < parameters.Length - 1)
                    argsCall.Append(", ");
            }

            if(methodInfo.ReturnType == typeof(void))
            {
                code
                    .Add($"instance->{nativeName}({argsCall});")
                    .Add("return 0;");
            }
            else
            {
                code.Add($"{CodeUtils.GetNativeDeclaration(methodInfo.ReturnType)} result = instance->{nativeName}({argsCall});");
                CodeUtils.EmitValueWrite(methodInfo.ReturnType, "result", code);
                code.Add("return 1;");
            }
        }

        public static ClassObject Create(Type type)
        {
            if (!type.IsSubclassOf(typeof(ClassObject)))
                throw new Exception("invalid engine object derived type.");
            var engineObject = Activator.CreateInstance(type) as ClassObject;
            if (engineObject is null)
                throw new Exception("cannot instantiate engine object derived type.");
            return engineObject;
        }
    }
}
