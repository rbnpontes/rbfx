using JSBindTool.Bindings;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public class ClassGen : CodeGen
    {
        public ClassGen(Type type) : base(type)
        {
        }

        public override CodeBuilder BuildHeader()
        {
            CodeBuilder header = new CodeBuilder();
            header.IndentationSize = 0;
            HeaderUtils.EmitNotice(header);
            header.Add("#pragma once");
            header.Add(GetIncludes()).AddNewLine();

            header.Add("namespace Urho3D");
            header.Add("{");
            {
                CodeBuilder signatures = new CodeBuilder();
                if (!IsAbstract())
                {
                    signatures.Add($"void {Target.Name}_setup(duk_context* ctx);");
                    signatures.Add($"void {Target.Name}_ctor(duk_context* ctx);");
                }
                signatures.Add($"void {Target.Name}_wrap(duk_context* ctx, duk_idx_t obj_idx, {Target.Name}* instance);");
                header.Add(signatures);
            }
            header.Add("}");

            return header;
        }

        public override CodeBuilder BuildSource()
        {
            CodeBuilder source = new CodeBuilder();
            source.IndentationSize = 0;
            HeaderUtils.EmitNotice(source);
            source.Add($"#include \"{Target.Name}.h\"").AddNewLine();
            source.Add("namespace Urho3D");
            source.Add("{");

            CodeBuilder scope = new CodeBuilder();
            if (!IsAbstract())
            {
                BuildSetup(scope);
                BuildConstructor(scope);
            }
            BuildWrap(source);

            source.Add(scope);
            source.Add("}");
            return source;
        }

        private void BuildSetup(CodeBuilder code)
        {
            code.Add($"void {Target.Name}_setup(duk_context* ctx)");
            code.Add("{");
            {
                CodeBuilder scope = new CodeBuilder();
                scope.Add($"duk_push_c_function(ctx, {Target.Name}_ctor, DUK_VARARGS);");
                scope.Add($"duk_push_c_function(ctx, [](duk_context* ctx)");
                scope.Add("{");
                {
                    CodeBuilder wrapScope = new CodeBuilder();
                    wrapScope.Add($"{Target.Name}_wrap(ctx, 0, static_cast<{Target.Name}*>(rbfx_get_instance(ctx, 0)));");
                    wrapScope.Add("return 0;");

                    scope.Add(wrapScope);
                }
                scope.Add("}, 1);");
                scope.Add("duk_put_prop_string(ctx, -2, JS_OBJ_HIDDEN_WRAP_CALL);");
                scope.Add($"duk_put_global_string(ctx, \"{Target.Name}\");");

                code.Add(scope);
            }
            code.Add("}");
        }
        private void BuildConstructor(CodeBuilder code)
        {
            code.Add($"void {Target.Name}_ctor(duk_context* ctx)");
            code.Add("{");
            {
                CodeBuilder scope = new CodeBuilder();
                scope.Add("if (!duk_is_constructor_call(ctx))");
                scope.Add("{");
                {
                    CodeBuilder ctorValidationScope = new CodeBuilder();
                    ctorValidationScope.Add("duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid constructor call. must call with 'new' keyword.\");");
                    ctorValidationScope.Add("return duk_throw(ctx);");
                    scope.Add(ctorValidationScope);
                }
                scope.Add("}");

                scope.AddNewLine();

                scope.Add($"{Target.Name}* instance = nullptr;");
                scope.Add("duk_idx_t obj_idx = duk_get_top(ctx);");
                scope.Add("if(obj_idx > 1)");
                scope.Add("{");
                {
                    CodeBuilder ctorValidationScope = new CodeBuilder();
                    ctorValidationScope.Add("duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid constructor call.\");");
                    ctorValidationScope.Add("return duk_throw(ctx);");
                    scope.Add(ctorValidationScope);
                }
                scope.Add("}");

                scope.AddNewLine();

                scope.Add("duk_push_this(ctx);");
                scope.Add("// if arguments has been used, the first argument must be a pointer to Object*");
                scope.Add($"if(obj_idx == 0)");
                scope.Add($"\tinstance = new {Target.Name}(JavaScriptSystem::GetContext());");
                scope.Add("else");
                scope.Add("{");
                {
                    CodeBuilder instanceScope = new CodeBuilder();
                    instanceScope.Add($"instance = static_cast<{Target.Name}*>(duk_require_pointer(ctx, 0));");
                    instanceScope.Add("if(!instance)");
                    instanceScope.Add("{");
                    instanceScope.Add($"\tduk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"pointer argument is null: \");");
                    instanceScope.Add("\treturn duk_throw(ctx);");
                    instanceScope.Add("}");
                    scope.Add(instanceScope);
                }
                scope.Add("}");

                scope.AddNewLine();
                scope.Add("void* heapptr = instance->GetJSHeapptr();");
                scope.Add("if(rbfx_is_valid_heapptr(ctx, heapptr))");
                scope.Add("{");
                {
                    CodeBuilder heapptrScope = new CodeBuilder();
                    heapptrScope.Add("duk_push_heapptr(ctx, heapptr);");
                    heapptrScope.Add($"{Target.Name}_wrap(ctx, obj_idx + 1, instance);");
                    heapptrScope.Add("return 1;");
                    scope.Add(heapptrScope);
                }
                scope.Add("}");

                scope.AddNewLine();

                scope.Add(
                    "heapptr = duk_get_heapptr(ctx, -1);",
                    "instance->SetJSHeapptr(heapptr);",
                    "instance->AddRef();"
                );

                scope.AddNewLine();

                scope.Add($"{Target.Name}_wrap(ctx, obj_idx, instance);");
                scope.Add($"rbfx_set_finalizer(ctx, obj_idx, instance);");

                scope.AddNewLine();
                scope.Add("return 1;");

                code.Add(scope);
            }
            code.Add("}");
        }
        private void BuildWrap(CodeBuilder code)
        {
            code.Add($"void {Target.Name}_wrap(duk_context* ctx, duk_idx_t obj_idx, {Target.Name}* instance)");
            code.Add("{");
            {
                CodeBuilder wrapScope = new CodeBuilder();
                BuildProperties(code);

                code.Add(wrapScope);
            }
            code.Add("}");
        }

        private void BuildProperties(CodeBuilder code)
        {
            var props = Target.GetProperties().ToList();
            if (props.Count > 0)
                code.Add("// properties setup");

            props.ForEach(prop =>
            {
                List<string> enumFlags = new List<string>();
                enumFlags.Add("DUK_DEFPROP_HAVE_ENUMERABLE");

                code.Add($"duk_push_string(ctx, \"{GetPropName(prop)}\");");

                if (prop.GetMethod != null)
                {
                    code.Add("duk_push_c_function(ctx, [](duk_context*)");
                    code.Add("{");
                    {
                        CodeBuilder getterScope = new CodeBuilder();
                        getterScope.Add("duk_push_this(ctx);");
                        getterScope.Add($"{Target.Name}* instance = static_cast<{Target.Name}*>(rbfx_get_instance(ctx, -1));");
                        getterScope.Add("duk_pop(ctx);");
                        getterScope.Add($"{GetTypeVarDeclaration(prop.PropertyType)} result = instance->{GetPropName(prop)}();");
                        EmitValueWrite(prop.PropertyType, "result", getterScope);

                        getterScope.Add("return 1;");
                        code.Add(getterScope);
                    }
                    code.Add("}, 0);");
                    enumFlags.Add("DUK_DEFPROP_HAVE_GETTER");
                }
                if(prop.SetMethod != null)
                {
                    code.Add("duk_push_c_function(ctx, [](duk_context*)");
                    code.Add("{");
                    {
                        CodeBuilder setterScope = new CodeBuilder();
                        setterScope.Add(
                            "duk_push_this(ctx);",
                            $"{Target.Name}* instance = static_cast<{Target.Name}*>(rbfx_get_instance(ctx, -1));",
                            "duk_pop(ctx);"
                        );
                        EmitValueRead(prop.PropertyType, "value", "0", setterScope);

                        setterScope.Add(
                            $"instance->{GetPropSetterName(prop)}(value);",
                            "return 0;"
                        );

                        code.Add(setterScope);
                    }
                    code.Add("}, 1);");
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
            });
        }

        private bool IsAbstract()
        {
            AbstractAttribute? attr = Target.GetCustomAttribute<AbstractAttribute>(false);
            return attr != null;
        }
        protected override CodeBuilder GetIncludes()
        {
            CodeBuilder includes = base.GetIncludes();
            if(Target.BaseType != null)
            {
                if (Target.BaseType == typeof(EngineObject))
                    includes.Add("#include <Urho3D/Core/Object.h>");
                else
                    includes.Add($"#include \"{Target.BaseType.Name}_Class.h\"");
            }

            return includes;
        }

        private string GetPropName(PropertyInfo prop)
        {
            string propName = prop.GetCustomAttribute<PropertyMapAttribute>()?.GetterName ?? prop.Name;
            propName = propName.Replace("Get", string.Empty).Replace("Set", string.Empty);
            return ToCamelCase(propName);
        }
        private string GetPropGetterName(PropertyInfo prop)
        {
            return prop.GetCustomAttribute<PropertyMapAttribute>()?.GetterName ?? prop.Name;
        }
        private string GetPropSetterName(PropertyInfo prop)
        {
            return prop.GetCustomAttribute<PropertyMapAttribute>()?.SetterName ?? prop.Name;
        }
    }
}
