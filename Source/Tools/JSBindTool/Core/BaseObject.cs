using JSBindTool.Bindings.MathTypes;
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
        }
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

            lookupField.Add("ea::unordered_map<StringHash, duk_c_function> g_functions = {");

            bool hasVariants = false;
            var methodsData = GetMethods();
            foreach (var pair in methodsData)
            {
                // only generate lookup table if any methods has two or more variants.
                if (pair.Value.Count <= 1)
                    continue;

                hasVariants = true;

                for (int i = 0; i < pair.Value.Count; ++i)
                {
                    MethodInfo method = pair.Value[i];
                    CodeBuilder funcPair = new CodeBuilder();
                    string funcSignature = $"{CodeUtils.GetMethodPrefix(Target)}_{CodeUtils.ToSnakeCase(pair.Key)}{i}_call";
                    uint funcHash = 0u;

                    bool hasStringHash = false;

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

                    method.GetParameters().ToList().ForEach(param => hashParamCalc(param, typeof(string)));

                    funcPair.Add($"{{ StringHash({funcHash}), {funcSignature} }},");
                    lookupField.Add(funcPair);

                    // if user code passes a number instead of string
                    // then get type method will return Number hash code instead of string hash code
                    // this will lead to not found method call, to fix this
                    // bind tool will generate two entries for the same method.
                    if (hasStringHash)
                    {
                        funcHash = 0u;
                        method.GetParameters().ToList().ForEach(param => hashParamCalc(param, typeof(uint)));

                        funcPair.Add($"{{ StringHash({funcHash}), {funcSignature} }},");
                        lookupField.Add(funcPair);
                    }
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
            var methodData = GetMethods();

            foreach (var pair in methodData)
            {
                code.Add($"duk_idx_t {CodeUtils.GetMethodPrefix(Target)}_{CodeUtils.ToSnakeCase(pair.Key)}_call(duk_context* ctx)");
                code.Scope(scopeCode =>
                {
                    if (pair.Value.Count <= 1)
                        EmitMethodBody(pair.Value.First(), scopeCode);
                    else
                    {
                        int minArgs = 0;
                        int maxArgs = 0;

                        pair.Value.ForEach(x =>
                        {
                            var paramsLength = x.GetParameters().Length;
                            minArgs = Math.Min(minArgs, paramsLength);
                            maxArgs = Math.Max(maxArgs, paramsLength);
                        });

                        // emit method selection
                        scopeCode
                            .Add("unsigned methodHash = 0u;")
                            .Add("duk_idx_t argc = duk_get_top(ctx);")
                            .AddNewLine()
                            .Add("// validate arguments count")
                            .Add($"if(argc < {minArgs})")
                            .Scope(ifCode =>
                            {
                                ifCode
                                    .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid arguments to function '{pair.Key}'.\");")
                                    .Add("return duk_throw(ctx);");
                            })
                            .Add($"else if(argc > {maxArgs})")
                            .Scope(elseScope =>
                            {
                                elseScope
                                    .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid arguments to function '{pair.Key}'.\");")
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
                                    .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid arguments to function '{pair.Key}'.\");")
                                    .Add("return duk_throw(ctx);");
                            })
                            .Add("return funcIt->second(ctx);");
                    }
                });

                if (pair.Value.Count <= 1)
                    continue;

                for (int i = 0; i < pair.Value.Count; ++i)
                {
                    code.Add($"duk_idx_t {CodeUtils.GetMethodPrefix(Target)}_{CodeUtils.ToSnakeCase(pair.Key)}{i}_call(duk_context* ctx)");
                    code.Scope(scopeCode => EmitMethodBody(pair.Value[i], scopeCode, false));
                }
            }
        }

        protected virtual void EmitMethodBody(MethodInfo methodInfo, CodeBuilder code, bool emitValidations = true)
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
                pair.Value.ForEach(x => maxArgs = Math.Max(maxArgs, x.GetParameters().Length));

                if (pair.Value.Count <= 1)
                    code.Add($"duk_push_c_lightfunc(ctx, {CodeUtils.GetMethodPrefix(Target)}_{CodeUtils.ToSnakeCase(pair.Key)}_call, {maxArgs}, {maxArgs}, 0);");
                else
                    code.Add($"duk_push_c_function(ctx, {CodeUtils.GetMethodPrefix(Target)}_{CodeUtils.ToSnakeCase(pair.Key)}_call, DUK_VARARGS);");

                code.Add($"duk_put_prop_string(ctx, {accessor}, \"{pair.Key}\");");
            }
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

        public abstract void EmitSourceIncludes(CodeBuilder code);
        protected abstract void EmitSourceConstructor(CodeBuilder code);
    }
}
