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
    public abstract class BaseObject
    {
        public Type Target { get; private set; }
        public BaseObject(Type type)
        {
            Target = type;
        }

        public virtual void EmitHeaderIncludes(CodeBuilder code)
        {
            code
                .Add(AnnotationUtils.GetIncludes(Target).Select(x => $"#include <{x}>"))
                .Add("#include <duktape/duktape.h>")
                .Add("#include <Urho3D/JavaScript/JavaScriptOperations.h>");
        }

        public virtual void EmitHeaderSignatures(CodeBuilder code)
        {
            code
                .Add($"void {CodeUtils.GetMethodPrefix(Target)}_setup(duk_context* ctx);")
                .Add($"duk_idx_t {CodeUtils.GetMethodPrefix(Target)}_ctor(duk_context* ctx);");

            EmitMethodSignatures(code);
            EmitOperatorMethodSignatures(code);
        }

        #region Method Emit Signatures
        protected virtual void EmitMethodSignatures(CodeBuilder code)
        {
            var methodsData = GetMethods();
            if (methodsData.Count > 0)
                code.Add("// @ methods definitions");
            foreach (var pair in methodsData)
            {
                code.Add($"duk_idx_t {GetMethodSignature(pair.Key)}(duk_context* ctx);");
                // generate variants only if has two or more methods definitions
                if (pair.Value.Count > 1)
                {
                    for (int i = 0; i < pair.Value.Count; ++i)
                        code.Add($"duk_idx_t {GetVariantMethodSignature(pair.Key, i)}(duk_context* ctx);");
                }
            }
        }
        protected virtual void EmitOperatorMethodSignatures(CodeBuilder code)
        {
            var methodsData = GetOperatorMethods();
            if (methodsData.Count > 0)
                code.Add("// @ operator methods definitions");
            foreach (var pair in methodsData)
            {
                code.Add($"duk_idx_t {GetOperatorMethodSignature(pair.Key)}(duk_context* ctx);");
                // generate variants only if has two or more methods definitions
                if (pair.Value.Count > 1)
                {
                    for (int i = 0; i < pair.Value.Count; ++i)
                        code.Add($"duk_idx_t {GetVariantOperatorMethodSignature(pair.Key, i)}(duk_context* ctx);");
                }
            }
        }
        #endregion

        protected string GetMethodSignature(string methodName)
        {
            return $"{CodeUtils.GetMethodPrefix(Target)}_{CodeUtils.ToSnakeCase(methodName)}_call";
        }
        protected string GetVariantMethodSignature(string methodName, int index)
        {
            return $"{CodeUtils.GetMethodPrefix(Target)}_{CodeUtils.ToSnakeCase(methodName)}{index}_call";
        }

        public virtual void EmitSource(CodeBuilder code)
        {
            EmitSourceMethodLookupTable(code);
            EmitSourceConstructor(code);
            EmitSourceSetup(code);
            EmitSourceMethods(code);
        }

        protected void EmitSourceMethodLookupTable(CodeBuilder code)
        {
            CodeBuilder lookupField = new CodeBuilder();
            lookupField.IndentationSize = 0;

            lookupField.Add("static ea::unordered_map<StringHash, duk_c_function> g_functions = {");

            bool hasVariants = false;
            bool hasStringHash = false;
            uint funcHash = 0u;

            Action<ParameterInfo, Type> hashParamCalc = (param, type) =>
            {
                uint hash = 0u;
                if (param.ParameterType == typeof(StringHash))
                {
                    hasStringHash = true;
                    hash = CodeUtils.GetTypeHash(type);
                }
                else
                {
                    hash = CodeUtils.GetTypeHash(param.ParameterType);
                }

                HashUtils.Combine(ref funcHash, hash);
            };
            Action<MethodInfo, string, string> insertFunctionDefinition = (method, funcName, funcSignature) =>
            {
                CodeBuilder funcEntriesCode = new CodeBuilder();

                uint methodNameHash = funcHash = HashUtils.Hash(funcName);

                hasStringHash = false;

                method.GetParameters().ToList().ForEach(param => hashParamCalc(param, typeof(string)));

                funcEntriesCode.Add($"{{ StringHash({(funcHash == 0u ? "0u" : funcHash.ToString())}), {funcSignature} }},");

                // if user code passes a number instead of string
                // then get type method will return Number hash code instead of string hash code
                // this will lead to not found method call, to fix this
                // bind tool will generate two entries for the same method.
                if (hasStringHash)
                {
                    funcHash = methodNameHash;
                    method.GetParameters().ToList().ForEach(param => hashParamCalc(param, typeof(uint)));

                    funcEntriesCode.Add($"{{ StringHash({(funcHash == 0u ? "0u" : funcHash.ToString())}), {funcSignature} }},");
                }

                lookupField.Add(funcEntriesCode);
            };

            // normal operators
            foreach (var pair in GetMethods())
            {
                // only generate lookup table if any methods has two or more variants.
                if (pair.Value.Count <= 1)
                    continue;

                hasVariants = true;

                for (int i = 0; i < pair.Value.Count; ++i)
                {
                    MethodInfo method = pair.Value[i];
                    insertFunctionDefinition(method, pair.Key, GetVariantMethodSignature(pair.Key, i));
                }
            }
            // operator methods
            foreach(var pair in GetOperatorMethods())
            {
                // only generate lookup table if any methods has two or more variants.
                if (pair.Value.Count <= 1)
                    continue;

                hasVariants = true;

                for(int i = 0; i < pair.Value.Count; ++i)
                {
                    MethodInfo method = pair.Value[i];
                    insertFunctionDefinition(method, GetOperatorName(pair.Key), GetVariantOperatorMethodSignature(pair.Key, i));
                }
            }

            lookupField.Add("};").AddNewLine();

            if (hasVariants)
                code.Add(lookupField);
        }

        protected virtual void EmitSourceSetup(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetMethodPrefix(Target)}_setup(duk_context* ctx)");
        }

        protected virtual void EmitSourceMethods(CodeBuilder code)
        {
            OperatorType operatorType = OperatorType.None;

            Action<List<MethodInfo>, string> insertFunction = (methods, funcName) =>
            {
                code
                    .Add($"duk_idx_t {(operatorType == OperatorType.None ? GetMethodSignature(funcName) : GetOperatorMethodSignature(operatorType))}(duk_context* ctx)")
                    .Scope(scopeCode =>
                    {
                        if (methods.Count <= 1)
                        {
                            if (operatorType == OperatorType.None)
                                EmitMethodBody(methods.First(), scopeCode);
                            else
                                EmitOperatorMethodBody(operatorType, methods.First(), scopeCode);
                            return;
                        }

                        int minArgs = 0;
                        int maxArgs = 0;

                        methods.ForEach(x =>
                        {
                            var paramsLength = x.GetParameters().Length;
                            minArgs = Math.Min(minArgs, paramsLength);
                            maxArgs = Math.Max(maxArgs, paramsLength);
                        });

                        // emit method selection
                        scopeCode
                            .Add($"unsigned methodHash = {HashUtils.Hash(funcName)};")
                            .Add("duk_idx_t argc = duk_get_top(ctx);")
                            .AddNewLine()
                            .Add("// validate arguments count")
                            .Add($"if(argc < {minArgs} || argc > {maxArgs})")
                            .Scope(ifCode =>
                            {
                                ifCode
                                    .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid arguments to function '{funcName}'.\");")
                                    .Add("return duk_throw(ctx);");
                            })
                            .AddNewLine()
                            .Add("for(unsigned i = 0; i < argc; ++i)")
                            .Scope(loopScope =>
                            {
                                loopScope
                                    .Add("StringHash typeHash = rbfx_get_type(ctx, i);")
                                    .Add("CombineHash(methodHash, typeHash.Value());");
                            })
                            .AddNewLine()
                            .Add("const auto funcIt = g_functions.find(StringHash(methodHash));")
                            .Add("if(funcIt == g_functions.end())")
                            .Scope(ifScope =>
                            {
                                ifScope
                                    .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid arguments to function '{funcName}'.\");")
                                    .Add("return duk_throw(ctx);");
                            })
                            .Add("return funcIt->second(ctx);");
                    });
                if (methods.Count <= 1)
                    return;

                for(int i =0; i <methods.Count; ++i)
                {
                    code
                    .Add($"duk_idx_t {(operatorType == OperatorType.None ? GetVariantMethodSignature(funcName, i) : GetVariantOperatorMethodSignature(operatorType, i))}(duk_context* ctx)")
                    .Scope(scopeCode =>
                    {
                        if (operatorType == OperatorType.None)
                            EmitMethodBody(methods[i], scopeCode, false);
                        else
                            EmitOperatorMethodBody(operatorType, methods[i], scopeCode, false);
                    });
                }
            };

            var methodData = GetMethods();
            foreach (var pair in GetMethods())
                insertFunction(pair.Value, pair.Key);
            foreach (var pair in GetOperatorMethods())
                insertFunction(pair.Value, GetOperatorName(operatorType = pair.Key));
        }

        protected virtual void EmitMethodBody(MethodInfo methodInfo, CodeBuilder code, bool emitValidations = true)
        {
            var parameters = methodInfo.GetParameters().ToList();

            if (emitValidations)
                EmitArgumentValidation(parameters, code);
        }
        protected virtual void EmitOperatorMethodBody(OperatorType opType, MethodInfo methodInfo, CodeBuilder code, bool emitValidations = true)
        {
            var parameters = methodInfo.GetParameters().ToList();

            if (emitValidations)
                EmitArgumentValidation(parameters, code);
        }

        protected virtual void EmitArgumentValidation(List<ParameterInfo> parameters, CodeBuilder code)
        {
            if (parameters.Count == 0)
                return;
            code.Add("#if defined(URHO3D_DEBUG)");
            for (int i = 0; i < parameters.Count; ++i)
            {
                uint typeHash = CodeUtils.GetTypeHash(parameters[i].ParameterType);
                code.Add($"rbfx_require_type(ctx, {i}, {typeHash}/*{parameters[i].ParameterType.Name}*/);");
            }
            code.Add("#endif").AddNewLine();
        }

        protected virtual void EmitMethods(CodeBuilder code, string accessor)
        {
            var methodsData = GetMethods();

            if (methodsData.Count > 0)
                code.Add("// methods setup");

            foreach (var pair in methodsData)
            {
                int maxArgs = 0;
                string methodSignature = GetMethodSignature(pair.Key);

                pair.Value.ForEach(x => maxArgs = Math.Max(maxArgs, x.GetParameters().Length));
                if (pair.Value.Count <= 1)
                    code.Add($"duk_push_c_lightfunc(ctx, {methodSignature}, {maxArgs}, {maxArgs}, 0);");
                else
                    code.Add($"duk_push_c_function(ctx, {methodSignature}, DUK_VARARGS);");

                code.Add($"duk_put_prop_string(ctx, {accessor}, \"{pair.Key}\");");
            }
        }

        protected virtual void EmitOperatorMethods(CodeBuilder code, string accessor)
        {
            var methodsData = GetOperatorMethods();
            if (methodsData.Count > 0)
                code.Add("// operator methods setup");

            foreach(var pair in methodsData)
            {
                int maxArgs = 0;
                string methodSignature = GetOperatorMethodSignature(pair.Key);

                pair.Value.ForEach(x => maxArgs = Math.Max(maxArgs, x.GetParameters().Length));

                if (pair.Value.Count <= 1)
                    code.Add($"duk_push_c_lightfunc(ctx, {methodSignature}, {maxArgs}, {maxArgs}, 0);");
                else
                    code.Add($"duk_push_c_function(ctx, {methodSignature}, DUK_VARARGS);");

                code.Add($"duk_put_prop_string(ctx, {accessor}, \"{GetOperatorName(pair.Key)}\");");
            }
        }
        protected virtual void EmitProperties(CodeBuilder code, string accessor)
        {
            var props = GetProperties();

            if (props.Count > 0)
                code.Add("// properties setup");

            props.ForEach(prop => EmitProperty(prop, accessor, code));

            if (props.Count > 0)
                code.AddNewLine();
        }

        protected Dictionary<OperatorType, List<MethodInfo>> GetOperatorMethods()
        {
            Dictionary<OperatorType, List<MethodInfo>> methodsData = new Dictionary<OperatorType, List<MethodInfo>>();

            Target
                .GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
                .Where(x => AnnotationUtils.IsValidOperatorMethod(x))
                .ToList()
                .ForEach(x =>
                {
                    List<MethodInfo>? method;
                    OperatorMethodAttribute attr = AnnotationUtils.GetOperatorMethodAttribute(x);
                    if (methodsData.TryGetValue(attr.OperatorType, out method))
                    {
                        method.Add(x);
                    }
                    else
                    {
                        method = new List<MethodInfo>();
                        method.Add(x);
                        methodsData.Add(attr.OperatorType, method);
                    }
                });

            return methodsData;
        }
        protected Dictionary<string, List<MethodInfo>> GetMethods()
        {
            Dictionary<string, List<MethodInfo>> methodsData = new Dictionary<string, List<MethodInfo>>();

            Target
                .GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
                .Where(x => AnnotationUtils.IsValidMethod(x))
                .ToList()
                .ForEach(x =>
                {
                    List<MethodInfo>? method;
                    string methodName = AnnotationUtils.GetMethodName(x);
                    if (methodsData.TryGetValue(methodName, out method))
                    {
                        method.Add(x);
                    }
                    else
                    {
                        method = new List<MethodInfo>();
                        method.Add(x);
                        methodsData.Add(methodName, method);
                    }
                });

            return methodsData;
        }
        protected List<PropertyInfo> GetProperties()
        {
            return Target.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Where(x => AnnotationUtils.IsValidProperty(x))
                .ToList();
        }
        protected string GetOperatorSignal(OperatorType opType)
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
        protected string GetOperatorName(OperatorType opType)
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
        protected string GetOperatorMethodSignature(OperatorType opType)
        {
            return $"{CodeUtils.GetMethodPrefix(Target)}_op_{GetOperatorName(opType)}_call";
        }
        protected string GetVariantOperatorMethodSignature(OperatorType opType, int idx)
        {
            return $"{CodeUtils.GetMethodPrefix(Target)}_op_{GetOperatorName(opType)}{idx}_call";
        }

        public abstract void EmitSourceIncludes(CodeBuilder code);
        protected abstract void EmitSourceConstructor(CodeBuilder code);
        protected abstract void EmitProperty(PropertyInfo prop, string accessor, CodeBuilder code);
    }
}
