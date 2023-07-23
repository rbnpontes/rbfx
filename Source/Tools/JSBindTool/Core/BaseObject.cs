using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public abstract class BaseObject
    {
        const BindingFlags StaticMethodsFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;

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
            EmitStaticMethodSignatures(code);
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
        protected virtual void EmitStaticMethodSignatures(CodeBuilder code)
        {
            var methodsData = GetMethods(StaticMethodsFlags);
            if (methodsData.Count > 0)
                code.Add("// @ static methods definitions");
            foreach(var pair in methodsData)
            {
                code.Add($"duk_idx_t {GetStaticMethodSignature(pair.Key)}(duk_context* ctx);");
                if(pair.Value.Count > 1)
                {
                    for (int i = 0; i < pair.Value.Count; ++i)
                        code.Add($"duk_idx_t {GetVariantStaticMethodSignature(pair.Key, i)}(duk_context* ctx);");
                }
            }
        }
        #endregion

        public virtual void EmitSourceIncludes(CodeBuilder code)
        {
            HashSet<string> includes = new HashSet<string>();
            // add self header
            if(Target.IsSubclassOf(typeof(ClassObject)))
                includes.Add($"#include \"{Target.Name}{Constants.ClassIncludeSuffix}.h\"");
            else if(Target.IsSubclassOf(typeof(PrimitiveObject)))
                includes.Add($"#include \"{Target.Name}{Constants.PrimitiveIncludeSuffix}.h\"");

            // add inheritance headers if exists
            if (Target.BaseType != null && Target.BaseType != typeof(ClassObject) && Target.BaseType != typeof(PrimitiveObject))
            {
                if (Target.IsSubclassOf(typeof(ClassObject)))
                    includes.Add($"#include \"{Target.BaseType.Name}{Constants.ClassIncludeSuffix}.h\"");
                else if (Target.IsSubclassOf(typeof(PrimitiveObject)))
                    includes.Add($"#include \"{Target.BaseType.Name}{Constants.PrimitiveIncludeSuffix}.h\"");
            }

            // add require headers
            includes.Add("#include <Urho3D/IO/Log.h>");
            includes.Add("#include <Urho3D/JavaScript/JavaScriptSystem.h>");
            includes.Add("#include <Urho3D/JavaScript/JavaScriptOperations.h>");


            IEnumerable<Type> usedTypes = new List<Type>();
            {   // collect types used to be include your headers
                Action<List<MethodInfo>> methodTypesCollect = (x) =>
                {
                    x.ForEach(method =>
                    {
                        usedTypes = usedTypes.Concat(method.GetParameters().Select(x => x.ParameterType));
                        if (method.ReturnType != typeof(void))
                            usedTypes = usedTypes.Concat(new Type[] { method.ReturnType });
                    });
                };

                GetMethods().Values.ToList().ForEach(methodTypesCollect);
                GetMethods(StaticMethodsFlags).Values.ToList().ForEach(methodTypesCollect);
                GetProperties().ForEach(prop => usedTypes = usedTypes.Concat(new Type[] { prop.PropertyType }));
            }

            // add all headers from collected types 
            usedTypes.ToList().ForEach(type =>
            {
                if (type.IsSubclassOf(typeof(ClassObject)))
                    includes.Add($"#include \"{type.Name}{Constants.ClassIncludeSuffix}.h\"");
                else if (type.IsSubclassOf(typeof(PrimitiveObject)))
                    includes.Add($"#include \"{type.Name}{Constants.PrimitiveIncludeSuffix}.h\"");
            });

            // finally, generate all headers into code
            code.Add(includes.ToArray()).AddNewLine();
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
            bool isStatic = false;
            uint funcHash = 0u;

            Action<Type, Type> hashParamCalc = (paramType, type) =>
            {
                uint hash = 0u;
                if (paramType == typeof(StringHash))
                {
                    hasStringHash = true;
                    hash = CodeUtils.GetTypeHash(type);
                }
                else
                {
                    hash = CodeUtils.GetTypeHash(paramType);
                }

                HashUtils.Combine(ref funcHash, hash);
            };
            Action<MethodInfo, string, string> insertFunctionDefinition = (method, funcName, funcSignature) =>
            {
                CodeBuilder funcEntriesCode = new CodeBuilder();

                // append static keyword at start of function name, this prevent naming hash collision
                uint methodNameHash = funcHash = HashUtils.Hash((isStatic ? "static" : string.Empty)+funcName);

                hasStringHash = false;

                List<Type> paramTypes = method.GetParameters().Select(x => x.ParameterType).ToList();

                CustomCodeAttribute? attr = method.GetCustomAttribute<CustomCodeAttribute>();
                if (attr != null)
                    paramTypes = attr.ParameterTypes.ToList();

                paramTypes.ForEach(param => hashParamCalc(param, typeof(string)));

                if (funcHash == 1405959817)
                    System.Diagnostics.Debugger.Break();

                funcEntriesCode.Add($"{{ StringHash({(funcHash == 0u ? "0u" : funcHash.ToString())}), {funcSignature} }},");

                // if user code passes a number instead of string
                // then get type method will return Number hash code instead of string hash code
                // this will lead to not found method call, to fix this
                // bind tool will generate two entries for the same method.
                if (hasStringHash)
                {
                    funcHash = methodNameHash;
                    paramTypes.ForEach(param => hashParamCalc(param, typeof(uint)));

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
            // static methods
            isStatic = true;
            foreach(var pair in GetMethods(StaticMethodsFlags))
            {
                if (pair.Value.Count <= 1)
                    continue;

                hasVariants = true;

                for(int i =0; i < pair.Value.Count; ++i)
                {
                    MethodInfo method = pair.Value[i];
                    insertFunctionDefinition(method, pair.Key, GetVariantStaticMethodSignature(pair.Key, i));
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
            bool isStatic = false;

            Action<List<MethodInfo>, string> insertFunction = (methods, funcName) =>
            {
                string methodSignature;
                if (isStatic)
                    methodSignature = GetStaticMethodSignature(funcName);
                else if (operatorType == OperatorType.None)
                    methodSignature = GetMethodSignature(funcName);
                else
                    methodSignature = GetOperatorMethodSignature(operatorType);

                code
                    .Add($"duk_idx_t {methodSignature}(duk_context* ctx)")
                    .Scope(scopeCode =>
                    {
                        if (methods.Count <= 1)
                        {
                            if (isStatic)
                                EmitStaticMethodBody(methods.First(), scopeCode);
                            else if (operatorType == OperatorType.None)
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
                            .Add($"unsigned methodHash = {HashUtils.Hash((isStatic ? "static" : string.Empty)+funcName)};")
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
                    string methodVariantSignature;
                    if (isStatic)
                        methodVariantSignature = GetVariantStaticMethodSignature(funcName, i);
                    else if (operatorType == OperatorType.None)
                        methodVariantSignature = GetVariantMethodSignature(funcName, i);
                    else
                        methodVariantSignature = GetVariantOperatorMethodSignature(operatorType, i);

                    code
                    .Add($"duk_idx_t {methodVariantSignature}(duk_context* ctx)")
                    .Scope(scopeCode =>
                    {
                        if (isStatic)
                            EmitStaticMethodBody(methods[i], scopeCode, false);
                        else if (operatorType == OperatorType.None)
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

            isStatic = true;
            foreach (var pair in GetMethods(StaticMethodsFlags))
                insertFunction(pair.Value, pair.Key);
        }

        protected virtual void EmitMethodBody(MethodInfo methodInfo, CodeBuilder code, bool emitValidations = true)
        {
            var parameters = methodInfo.GetParameters().ToList();

            if (emitValidations)
                EmitArgumentValidation(parameters, methodInfo.GetCustomAttribute<CustomCodeAttribute>(), code);
        }
        protected virtual void EmitOperatorMethodBody(OperatorType opType, MethodInfo methodInfo, CodeBuilder code, bool emitValidations = true)
        {
            var parameters = methodInfo.GetParameters().ToList();

            if (emitValidations)
                EmitArgumentValidation(parameters, methodInfo.GetCustomAttribute<CustomCodeAttribute>(), code);
        }
        protected virtual void EmitStaticMethodBody(MethodInfo methodInfo, CodeBuilder code, bool emitValidations = true)
        {
            string nativeName = AnnotationUtils.GetMethodNativeName(methodInfo);

            var parameters = methodInfo.GetParameters().ToList();

            if (emitValidations)
                EmitArgumentValidation(parameters, methodInfo.GetCustomAttribute<CustomCodeAttribute>(), code);

            CustomCodeAttribute? customCodeAttr = methodInfo.GetCustomAttribute<CustomCodeAttribute>();
            if(customCodeAttr != null)
            {
                EmitCustomCodeMethodBody(customCodeAttr, methodInfo, code);
                return;
            }

            StringBuilder argsCall = new StringBuilder();
            for(int i =0; i < parameters.Count; ++i)
            {
                CodeUtils.EmitValueRead(parameters[i].ParameterType, $"arg{i}", i.ToString(), code);
                argsCall.Append($"arg{i}");
                if (i < parameters.Count - 1)
                    argsCall.Append(", ");
            }

            if(methodInfo.ReturnType == typeof(void))
            {
                code
                    .Add($"{CodeUtils.GetNativeDeclaration(Target)}::{nativeName}({argsCall});")
                    .Add("return 0;");
                return;
            }

            code.Add($"{CodeUtils.GetNativeDeclaration(methodInfo.ReturnType)} result = {CodeUtils.GetNativeDeclaration(Target)}::{nativeName}({argsCall});");

            CodeUtils.EmitValueWrite(methodInfo.ReturnType, "result", code);
            code.Add("return 1;");
        }

        protected virtual void EmitCustomCodeMethodBody(CustomCodeAttribute attr, MethodInfo methodInfo, CodeBuilder code)
        {
            for(int i = 0; i < attr.ParameterTypes.Length; ++i)
                CodeUtils.EmitValueRead(attr.ParameterTypes[i], $"arg{i}", i.ToString(), code);

            if (attr.ReturnType == typeof(void))
                code.Add("duk_idx_t result_code = 0;");
            else
                code.Add("duk_idx_t result_code = 1;");

            code.AddNewLine().Add("// begin custom code");
            methodInfo.Invoke(this, new object[] { code });
            code.Add("// end custom code").AddNewLine();
            if (attr.ReturnType == typeof(void))
            {
                code
                    .Add("duk_push_this(ctx);")
                    .Add($"{CodeUtils.GetMethodPrefix(Target)}_set(ctx, duk_get_top(ctx) - 1, instance);")
                    .Add("duk_pop(ctx);")
                    .Add("return result_code;");
                return;
            }

            CodeUtils.EmitValueWrite(attr.ReturnType, "result", code);
            code.Add("return result_code;");
        }

        protected virtual void EmitArgumentValidation(List<ParameterInfo> parameters, CustomCodeAttribute? customCodeAttr, CodeBuilder code)
        {
            List<Type> parameterTypes = parameters.Select(p => p.ParameterType).ToList();
            if (customCodeAttr != null)
                parameterTypes = customCodeAttr.ParameterTypes.ToList();

            if (parameterTypes.Count == 0)
                return;
            code.Add("#if defined(URHO3D_DEBUG)");
            for (int i = 0; i < parameterTypes.Count; ++i)
            {
                uint typeHash = CodeUtils.GetTypeHash(parameterTypes[i]);
                code.Add($"rbfx_require_type(ctx, {i}, {typeHash}/*{parameterTypes[i].Name}*/);");
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
        protected void EmitStaticMethods(CodeBuilder code, string accessor)
        {
            var methodsData = GetMethods(StaticMethodsFlags);
            if (methodsData.Count > 0)
                code.Add("// static methods setup");

            foreach(var pair in methodsData)
            {
                int maxArgs = 0;
                string methodSignature = GetStaticMethodSignature(pair.Key);

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
        protected Dictionary<string, List<MethodInfo>> GetMethods(BindingFlags flags = BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
        {
            Dictionary<string, List<MethodInfo>> methodsData = new Dictionary<string, List<MethodInfo>>();

            Target
                .GetMethods(flags)
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

        protected string GetMethodSignature(string methodName)
        {
            return $"{CodeUtils.GetMethodPrefix(Target)}_{CodeUtils.ToSnakeCase(methodName)}_call";
        }
        protected string GetVariantMethodSignature(string methodName, int index)
        {
            return $"{CodeUtils.GetMethodPrefix(Target)}_{CodeUtils.ToSnakeCase(methodName)}{index}_call";
        }
        protected string GetStaticMethodSignature(string methodName)
        {
            return $"{CodeUtils.GetMethodPrefix(Target)}_static_{CodeUtils.ToSnakeCase(methodName)}_call";
        }
        protected string GetVariantStaticMethodSignature(string methodName, int index)
        {
            return $"{CodeUtils.GetMethodPrefix(Target)}_static_{CodeUtils.ToSnakeCase(methodName)}{index}_call";
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

        protected abstract void EmitSourceConstructor(CodeBuilder code);
        protected abstract void EmitProperty(PropertyInfo prop, string accessor, CodeBuilder code);
    }
}
