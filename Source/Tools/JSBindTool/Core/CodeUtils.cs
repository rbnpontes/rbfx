using JSBindTool.Bindings.MathTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public static class CodeUtils
    {
        public static string ToCamelCase(string input)
        {
            if (input.Length == 0)
                return string.Empty;
            char[] chars = input.ToCharArray();
            chars[0] = char.ToLower(chars[0]);
            return new string(chars);
        }
        public static string GetNativeDeclaration(Type type)
        {
            StringBuilder output = new StringBuilder();

            if (type == typeof(string))
                output.Append("ea::string");
            else if (type == typeof(StringHash))
                output.Append("StringHash");
            else if (type == typeof(bool))
                output.Append("bool");
            else if (type == typeof(float))
                output.Append("float");
            else if (type == typeof(double))
                output.Append("double");
            else if (type == typeof(int))
                output.Append("int");
            else if (type == typeof(uint))
                output.Append("unsigned");
            else if (type == typeof(ushort))
                output.Append("unsigned short");
            else if (type == typeof(IntPtr))
                output.Append("void*");
            else if(type == typeof(JSFunction) || type == typeof(JSObject)) { /*do nothing*/ }
            else if (type.IsSubclassOf(typeof(ClassObject)))
                output.Append(type == typeof(ClassObject) ? "Object" : AnnotationUtils.GetTypeName(type)).Append("*");
            else if (type.IsSubclassOf(typeof(TemplateObject)))
            {
                var templateObj = TemplateObject.Create(type);
                switch (templateObj.TemplateType)
                {
                    case TemplateType.SharedPtr:
                        output.Append($"SharedPtr<{AnnotationUtils.GetTypeName(templateObj.TargetType)}>");
                        break;
                    case TemplateType.WeakPtr:
                        output.Append($"WeakPtr<{AnnotationUtils.GetTypeName(templateObj.TargetType)}>");
                        break;
                    case TemplateType.Vector:
                        output.Append($"ea::vector<{GetNativeDeclaration(templateObj.TargetType)}>");
                        break;
                }
            }
            else
                output.Append(type.Name);
            return output.ToString();
        }
        public static string ToSnakeCase(string input)
        {
            var output = new StringBuilder();

            bool isUpper = false;
            for(int i =0;i < input.Length; ++i)
            {
                if (char.IsUpper(input[i]) && !isUpper)
                {
                    if(i > 0)
                        output.Append('_');
                    isUpper = true;
                }
                else if (char.IsLower(input[i]))
                {
                    isUpper = false;
                }

                output.Append(char.ToLowerInvariant(input[i]));
            }

            return output.ToString();
        }
        public static string GetMethodPrefix(Type type)
        {
            return $"{Constants.MethodPrefix}_{ToSnakeCase(AnnotationUtils.GetTypeName(type))}";
        }

        public static string GetRefSignature(Type type)
        {
            return $"{GetMethodPrefix(type)}_get_ref";
        }
        public static string GetPushRefSignature(Type type)
        {
            return $"{GetMethodPrefix(type)}_push_ref";
        }
        private static int pDeepValueCount = 0;

        public static void EmitValueWrite(Type type, string accessor, CodeBuilder code)
        {
            if (type == typeof(JSFunction) || type == typeof(JSObject)) { /*do nothing*/ }
            else if (type == typeof(string))
                code.Add($"duk_push_string(ctx, {accessor}.c_str());");
            else if (type == typeof(StringHash))
                code.Add($"rbfx_push_string_hash(ctx, {accessor});");
            else if (type == typeof(bool))
                code.Add($"duk_push_boolean(ctx, {accessor});");
            else if (type == typeof(float) || type == typeof(double))
                code.Add($"duk_push_number(ctx, {accessor});");
            else if (type == typeof(int))
                code.Add($"duk_push_int(ctx, {accessor});");
            else if (type == typeof(uint) || type == typeof(ushort))
                code.Add($"duk_push_uint(ctx, {accessor});");
            else if (type == typeof(IntPtr))
                code.Add($"duk_push_pointer(ctx, {accessor});");
            else if (type.IsEnum)
                code.Add($"duk_push_int(ctx, {accessor});");
            else if (type.IsSubclassOf(typeof(ClassObject)) || type.Name.StartsWith("SharedPtr"))
                code.Add($"rbfx_push_object(ctx, {accessor});");
            else if (type.IsSubclassOf(typeof(PrimitiveObject)))
                code.Add($"{GetMethodPrefix(type)}_push(ctx, {accessor});");
            else if (type.IsSubclassOf(typeof(TemplateObject)))
            {
                var templateObj = TemplateObject.Create(type);

                switch (templateObj.TemplateType)
                {
                    case TemplateType.SharedPtr:
                        code.Add($"rbfx_push_object(ctx, {accessor});");
                        break;
                    case TemplateType.WeakPtr:
                        throw new NotImplementedException();
                        break;
                    case TemplateType.RefPtr:
                        code.Add($"{GetPushRefSignature(templateObj.TargetType)}(ctx, {accessor});");
                        break;
                    case TemplateType.Vector:
                        {
                            int deepSuffix = ++pDeepValueCount;

                            code.Add(
                                "duk_push_array(ctx);",
                                $"duk_idx_t arr_idx_{deepSuffix} = duk_get_top_index(ctx);",
                                $"for(duk_idx_t i_{deepSuffix} = 0; i_{deepSuffix} < {accessor}.size(); ++i_{deepSuffix})",
                                "{"
                            );
                            {
                                CodeBuilder scopeLoop = new CodeBuilder();
                                scopeLoop.Add($"{GetNativeDeclaration(templateObj.TargetType)} result_{deepSuffix} = {accessor}.at(i_{deepSuffix});");
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
        public static void EmitValueRead(Type type, string varName, string accessor, CodeBuilder code)
        {
            if (type == typeof(string))
                code.Add($"ea::string {varName} = duk_get_string_default(ctx, {accessor}, \"\");");
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
            else if (type == typeof(uint) || type == typeof(ushort))
                code.Add($"unsigned {varName} = duk_get_uint_default(ctx, {accessor}, 0u);");
            else if (type == typeof(IntPtr))
                code.Add($"void* {varName} = duk_get_pointer_default(ctx, {accessor}, nullptr);");
            else if (type == typeof(JSFunction) || type == typeof(JSObject))
                code.Add($"duk_idx_t {varName} = {accessor};");
            else if (type.IsEnum)
                code.Add($"{type.Name} {varName} = ({type.Name})duk_get_int(ctx, {accessor});");
            else if (type.IsSubclassOf(typeof(ClassObject)))
                code.Add($"{AnnotationUtils.GetTypeName(type)}* {varName} = static_cast<{AnnotationUtils.GetTypeName(type)}*>(rbfx_get_instance(ctx, {accessor}));");
            else if (type.IsSubclassOf(typeof(TemplateObject)))
            {
                var templateObj = TemplateObject.Create(type);
                switch (templateObj.TemplateType)
                {
                    case TemplateType.SharedPtr:
                        code.Add($"{GetNativeDeclaration(templateObj.TargetType)} {varName}(static_cast<{AnnotationUtils.GetTypeName(templateObj.TargetType)}*>(rbfx_get_instance(ctx, {accessor})));");
                        break;
                    case TemplateType.WeakPtr:
                        throw new NotImplementedException();
                    case TemplateType.RefPtr:
                            code.Add($"{AnnotationUtils.GetTypeName(templateObj.TargetType)}* {varName}({GetRefSignature(templateObj.TargetType)}(ctx, {accessor}));");
                        break;
                    case TemplateType.Vector:
                        {
                            code.Add($"{GetNativeDeclaration(type)} {varName};");
                            code.Add("{");
                            {
                                int deepCount = ++pDeepValueCount;
                                CodeBuilder scopeRead = new CodeBuilder();
                                scopeRead.Add($"duk_size_t len_{deepCount} = duk_get_length(ctx, {accessor});");
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
            else if (type.IsSubclassOf(typeof(PrimitiveObject)))
                code.Add($"{AnnotationUtils.GetTypeName(type)} {varName} = {GetMethodPrefix(type)}_resolve(ctx, {accessor});");
            else throw new NotImplementedException();
        }

        public static uint GetTypeHash(Type type)
        {
            string hashInput;

            if (type.IsEnum)
                hashInput = "Number";
            else if (type == typeof(string))
                hashInput = "string";
            else if (type == typeof(StringHash))
                hashInput = "StringHash";
            else if (type == typeof(bool))
                hashInput = "bool";
            else if (type == typeof(float) || type == typeof(double) || type == typeof(int) || type == typeof(uint))
                hashInput = "Number";
            else if (type == typeof(IntPtr))
                hashInput = "void";
            else if (type == typeof(JSFunction))
                hashInput = "Function";
            else if (type == typeof(JSObject))
                hashInput = "VariantMap";
            else if (type.IsSubclassOf(typeof(TemplateObject)))
            {
                var templateObj = TemplateObject.Create(type);
                if (templateObj.TemplateType == TemplateType.Vector)
                    hashInput = "Vector";
                else
                    throw new NotImplementedException("not implemented GetTypeHash for TemplateObject inherited types.");
            }
            else
                hashInput = AnnotationUtils.GetTypeName(type);

            return HashUtils.Hash(hashInput);
        }
    }
}
