using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    [Abstract]
    [Include("Urho3D/Core/Object.h")]
    public class ClassObject
    {
        public Type Target { get; private set; }
        public ClassObject(Type target)
        {
            Target = target;
        }

        public virtual void EmitHeaderIncludes(CodeBuilder code)
        {
            code.Add(AnnotationUtils.GetIncludes(Target).Select(x => $"#include <{x}>"));
            code.Add("#include <duktape/duktape.h>");
            code.Add("#include <Urho3D/JavaScript/JavaScriptOperations.h>");
        }
        public virtual void EmitHeaderSignatures(CodeBuilder code)
        {
            code.Add($"void {Target.Name}_setup(duk_context* ctx);");
            if (!AnnotationUtils.IsAbstract(Target))
                code.Add($"duk_idx_t {Target.Name}_ctor(duk_context* ctx);");
            code.Add($"void {Target.Name}_wrap(duk_context* ctx, duk_idx_t obj_idx, {AnnotationUtils.GetTypeName(Target)}* instance);");
        }

        public virtual void EmitSourceIncludes(CodeBuilder code)
        {
            string typeName = Target.Name;
            HashSet<string> includes = new HashSet<string>();
            includes.Add($"{Target.Name}_Class.h");
            if (Target.BaseType != null && Target.BaseType != typeof(ClassObject))
                includes.Add($"{Target.BaseType.Name}_Class.h");
            includes.Add("Urho3D/JavaScript/JavaScriptSystem.h");
            includes.Add("Urho3D/JavaScript/JavaScriptOperations.h");
            Action<Type> addInclude = (type) =>
            {
                AnnotationUtils
                    .GetIncludes(type)
                    .ForEach(x =>
                    {
                        if (!includes.Contains(x))
                            includes.Add(x);
                    });
                if(type.IsSubclassOf(typeof(ClassObject)))
                {
                    string include = $"{type.Name}_Class.h";
                    if (!includes.Contains(include))
                        includes.Add(include);
                }
                else if (type.IsSubclassOf(typeof(PrimitiveObject)))
                {
                    string include = $"{type.Name}_Primitive.h";
                    if (!includes.Contains(include))
                        includes.Add(include);
                }
            };
            // collect binding dependency
            Target.GetProperties(BindingFlags.DeclaredOnly)
                .ToList()
                .ForEach(prop =>
                {
                    addInclude(prop.PropertyType);
                });
            Target.GetFields(BindingFlags.DeclaredOnly)
                .Where(x => !AnnotationUtils.IsIgnored(x))
                .ToList()
                .ForEach(field =>
                {
                    addInclude(field.FieldType);
                });
            Target.GetMethods(BindingFlags.DeclaredOnly)
                .Where(x => AnnotationUtils.IsValidMethod(x))
                .ToList()
                .ForEach(method =>
                {
                    method.GetParameters().ToList().ForEach(x => addInclude(x.ParameterType));
                    addInclude(method.ReturnType);
                });

            includes.ToList().ForEach(include =>
            {
                if (include.EndsWith("_Primitive.h") || include.EndsWith("_Class.h"))
                    code.Add($"#include \"{include}\"");
                else
                    code.Add($"#include <{include}>");
            });
        }

        public virtual void EmitSource(CodeBuilder code)
        {
            EmitSourceSetup(code);
            if(!AnnotationUtils.IsAbstract(Target))
                EmitSourceConstructor(code);
            EmitSourceWrap(code);
        }

        protected virtual void EmitSourceSetup(CodeBuilder code)
        {
            code.Add($"void {Target.Name}_setup(duk_context* ctx)");
            code.Scope(scope =>
            {
            });
        }
        protected virtual void EmitSourceConstructor(CodeBuilder code)
        {
            code.Add($"duk_idx_t {Target.Name}_ctor(duk_context* ctx)");
            code.Scope(scope =>
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
                    ifScope.Add($"{Target.Name}_wrap(ctx, duk_get_top(ctx) - 1, instance);");
                    ifScope.Add("return 1;");
                }).AddNewLine();

                scope.Add(
                    "heapptr = duk_get_heapptr(ctx, obj_idx);",
                    "instance->SetJSHeapptr(heapptr);",
                    "instance->AddRef();",
                    "rbfx_lock_heapptr(ctx, heapptr);"
                ).AddNewLine();

                scope.Add(
                    $"{Target.Name}_wrap(ctx, obj_idx, instance);",
                    "rbfx_set_finalizer(ctx, obj_idx, instance);"
                ).AddNewLine();

                scope.Add("duk_push_this(ctx);");
                scope.Add("return 1;");
            });
        }
        protected virtual void EmitSourceWrap(CodeBuilder code)
        {
            code.Add($"void {Target.Name}_wrap(duk_context* ctx, duk_idx_t obj_idx, {AnnotationUtils.GetTypeName(Target)}* instance)");
            code.Scope(scope =>
            {
                if (Target.BaseType == typeof(ClassObject))
                    scope.Add("rbfx_object_wrap(ctx, obj_idx, instance);");
                else if(Target.BaseType != null)
                    scope.Add($"{Target.BaseType.Name}_wrap(ctx, obj_idx, instance);");
                EmitProperties(scope);
            });
        }

        protected virtual void EmitProperties(CodeBuilder code)
        {
            Target.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Where(x => AnnotationUtils.IsValidProperty(x))
                .ToList()
                .ForEach(prop => EmitProperty(prop, code));
        }
        protected virtual void EmitProperty(PropertyInfo prop, CodeBuilder code)
        {
            List<string> enumFlags = new List<string>();
            enumFlags.Add("DUK_DEFPROP_HAVE_ENUMERABLE");

            code.AddNewLine();
            code.Add($"duk_push_string(ctx, \"{CodeUtils.ToCamelCase(AnnotationUtils.GetJSPropertyName(prop))}\");");

            var attr = AnnotationUtils.GetPropertyMap(prop);
            if(prop.GetMethod != null && !string.IsNullOrEmpty(attr.GetterName))
            {
                code.Function(getterScope =>
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
                code.Function(setterScope =>
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

            code.Add($"duk_def_prop(ctx, obj_idx, {propFlags});");
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
