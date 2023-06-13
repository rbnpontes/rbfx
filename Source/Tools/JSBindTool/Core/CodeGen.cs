using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public abstract class CodeGen
    {
        public Type Target { get; private set; }
        public string HeaderName { get; set; } = string.Empty;
        public CodeGen(Type type)
        {
            Target = type;
        }

        protected virtual CodeBuilder GetIncludes()
        {
            CodeBuilder codeBuilder = new CodeBuilder();
            codeBuilder.IndentationSize = 0;
            codeBuilder.Add(GetIncludesFromType(Target).ToList().Select(include => $"#include <{include}>"));
            codeBuilder.Add("#include <Urho3D/JavaScript/JavaScriptOperations.h>");
            codeBuilder.Add("#include <Urho3D/JavaScript/JavaScriptSystem.h>");
            codeBuilder.Add("#include <duktape/duktape.h>");
            return codeBuilder;
        }
        protected HashSet<string> GetIncludesFromType(Type type)
        {
            HashSet<string> includes = new HashSet<string>();
            var attributes = Target.GetCustomAttributes<IncludeAttribute>();
            attributes.ToList().ForEach(attr =>
            {
                if (!includes.Contains(attr.Value))
                    includes.Add(attr.Value);
            });

            return includes;
        }
        protected string ToCamelCase(string input)
        {
            if (input.Length == 0)
                return string.Empty;
            char[] chars = input.ToCharArray();
            chars[0] = char.ToLower(chars[0]);
            return new string(chars);
        }
        public abstract CodeBuilder BuildHeader();
        public abstract CodeBuilder BuildSource();


        private int pDeepValueCount = 0;
        protected void EmitValueWrite(Type type, string accessor, CodeBuilder code)
        {
            if (type == typeof(JSFunction))
                throw new Exception("function cannot be used as write value");
            if (type.IsEnum)
                code.Add($"duk_push_int(ctx, {accessor});");
            else if (type.IsSubclassOf(typeof(ClassObject)) || type.Name.StartsWith("SharedPtr"))
                code.Add($"rbfx_push_object(ctx, {accessor});");
            else if(type.IsSubclassOf(typeof(TemplateObject)))
            {
                var templateObj = GetTemplateObject(type);

                switch (templateObj.TemplateType)
                {
                    case TemplateType.SharedPtr:
                        code.Add($"rbfx_push_object(ctx, {accessor});");
                        break;
                    case TemplateType.WeakPtr:
                        throw new NotImplementedException();
                        break;
                    case TemplateType.Vector:
                        {
                            int deepSuffix = ++pDeepValueCount;

                            code.Add(
                                "duk_push_array(ctx);",
                                $"duk_idx_t arr_idx_{deepSuffix} = duk_get_top(ctx);",
                                $"for(duk_idx_t i_{deepSuffix} = 0; i_{deepSuffix} < {accessor}.size(); ++i_{deepSuffix})",
                                "{"
                            );
                            {
                                CodeBuilder scopeLoop = new CodeBuilder();
                                scopeLoop.Add($"{GetTypeVarDeclaration(templateObj.TargetType)} result_{deepSuffix} = {accessor}.at(i_{deepSuffix});");
                                EmitValueWrite(templateObj.TargetType, $"result_{deepSuffix}", scopeLoop);
                                scopeLoop.Add($"duk_put_prop_index(ctx, arr_idx_{deepSuffix}, i_{deepSuffix});");
                                code.Add(scopeLoop);
                            }
                            code.Add("}");
                            --pDeepValueCount;
                        }
                        break;
                }
            }

        }
        protected void EmitValueRead(Type type, string varName, string accessor, CodeBuilder code)
        {
            if (type == typeof(string))
                code.Add($"const char* {varName} = duk_get_string_default(ctx, {accessor}, \"\");");
            else if (type == typeof(StringHash))
                code.Add($"StringHash {varName} = rbfx_get_string_hash(ctx, {accessor});");
            else if (type == typeof(bool))
                code.Add($"bool {varName} = duk_get_boolean_default(ctx, {accessor}, false);");
            else if (type == typeof(float))
                code.Add($"float {varName} = (float)duk_get_number_default(ctx, {accessor}, 0.0);");
            else if (type == typeof(double))
                code.Add($"double {varName} = duk_get_number_default(ctx, {accessor}, 0.0);");
            else if (type == typeof(int))
                code.Add($"int {varName} = duk_get_int_default(ctx, {accessor}, 0);");
            else if (type == typeof(uint))
                code.Add($"unsigned {varName} = duk_get_uint_default(ctx, {accessor}, 0u);");
            else if (type == typeof(IntPtr))
                code.Add($"void* {varName} = duk_get_pointer_default(ctx, {accessor}, nullptr);");
            else if (type == typeof(JSFunction))
                code.Add($"duk_idx_t {varName} = {accessor};");
            else if (type.IsEnum)
                code.Add($"{type.Name} {varName} = ({type.Name})duk_get_int(ctx, {accessor});");
            else if (type.IsSubclassOf(typeof(ClassObject)))
                code.Add($"{type.Name}* {varName} = static_cast<{type.Name}*>(rbfx_get_instance(ctx, {accessor}));");
            else if (type.IsSubclassOf(typeof(TemplateObject)))
            {
                var templateObj = GetTemplateObject(type);
                switch (templateObj.TemplateType)
                {
                    case TemplateType.SharedPtr:
                        code.Add($"{GetTypeVarDeclaration(templateObj.TargetType)} {varName}(static_cast<{templateObj.TargetType.Name}*>(rbfx_get_instance(ctx, {accessor})));");
                        break;
                    case TemplateType.WeakPtr:
                        throw new NotImplementedException();
                        break;
                    case TemplateType.Vector:
                        {
                            code.Add($"{GetTypeVarDeclaration(type)} {varName};");
                            code.Add("{");
                            {
                                int deepCount = ++pDeepValueCount;
                                CodeBuilder scopeRead = new CodeBuilder();
                                scopeRead.Add($"duk_size_t len_{deepCount} = duk_get_len(ctx, {accessor});");
                                scopeRead.Add($"{varName}.resize(len_{deepCount});").AddNewLine();

                                scopeRead.Add($"for(duk_uarridx_t i_{deepCount} = 0; i_{deepCount} < len_{deepCount}; ++i_{deepCount})");
                                scopeRead.Add("{");
                                {
                                    CodeBuilder scopeLoop = new CodeBuilder();
                                    scopeLoop.Add($"duk_get_prop_index(ctx, {accessor}, i_{deepCount});");
                                    EmitValueRead(templateObj.TargetType, $"result_{deepCount}", "-1", scopeLoop);
                                    scopeLoop.Add($"{varName}[i_{deepCount}] = result_{deepCount};");

                                    scopeRead.Add(scopeLoop);
                                }
                                scopeRead.Add("}");

                                code.Add(scopeRead);
                                --pDeepValueCount;
                            }
                            code.Add("}");
                        }
                        break;
                }
            }
            else if(type.IsSubclassOf(typeof(PrimitiveObject)))
            {

            }
            else throw new NotImplementedException();
        }

        protected string GetTypeVarDeclaration(Type type)
        {
            StringBuilder output = new StringBuilder();

            if (type.IsSubclassOf(typeof(ClassObject)))
                output.Append(type.Name == "EngineObject" ? "Object" : type.Name).Append("*");
            else if (type.IsSubclassOf(typeof(TemplateObject)))
            {
                var templateObj = GetTemplateObject(type);
                switch(templateObj.TemplateType)
                {
                    case TemplateType.SharedPtr:
                        output.Append($"SharedPtr<{templateObj.TargetType.Name}>");
                        break;
                    case TemplateType.WeakPtr:
                        output.Append($"WeakPtr<{templateObj.TargetType.Name}>");
                        break;
                    case TemplateType.Vector:
                        output.Append($"ea::vector<{GetTypeVarDeclaration(templateObj.TargetType)}>");
                        break;
                }
            }
            else
                output.Append(type.Name);
            return output.ToString();
        }
        protected TemplateObject GetTemplateObject(Type templateType)
        {
            var templateObj = Activator.CreateInstance(templateType) as TemplateObject;
            if (templateObj is null)
                throw new Exception("error at instantiate TemplateObject derived type.");
            return templateObj;
        }
    }
}
