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
                .Add("#include <Urho3D/JavaScript/JavaScriptOperations.h>")
                .AddNewLine();
            var defines = Target.GetCustomAttributes<DefineAttribute>(false);
            defines.ToList().ForEach(define => code.Add(define.CustomCode));
        }
        public virtual void EmitHeaderSignatures(CodeBuilder code)  
        {
            code.Add($"void {CodeUtils.GetMethodPrefix(Target)}_setup(duk_context* ctx);");
            EmitSetupStaticSignatures(code);
            EmitWrapSignature(code);
            EmitGetRefSignatures(code);
            EmitVariableSignatures(code);
            EmitCtorSignatures(code);
            EmitPropertySignatures(code);
            EmitMethodSignatures(code);
            EmitOperatorMethodSignatures(code);
            EmitStaticMethodSignatures(code);
        }

        #region Method Emit Signatures
        protected virtual void EmitWrapSignature(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetMethodPrefix(Target)}_wrap(duk_context* ctx, duk_idx_t obj_idx, {AnnotationUtils.GetTypeName(Target)}* instance);");
        }
        protected virtual void EmitGetRefSignatures(CodeBuilder code)
        {
            code.Add($"{AnnotationUtils.GetTypeName(Target)}* {GetRefSignature()}(duk_context* ctx, duk_idx_t obj_idx);");
        }
        protected virtual void EmitVariableSignatures(CodeBuilder code)
        {
            var variables = AnnotationUtils.GetVariables(Target);
            if (variables.Count > 0)
                code.Add("// @ variables definitions");

            foreach(var prop in variables)
            {
                code.Add($"duk_idx_t {GetVariableSignature(prop)}(duk_context* ctx);");
                code.Add($"duk_idx_t {GetVariableSignature(prop, true)}(duk_context* ctx);");
            }
        }
        protected virtual void EmitPropertySignatures(CodeBuilder code)
        {
            var properties = GetProperties();
            if (properties.Count > 0)
                code.Add("// @ properties definitions");

            foreach(var prop in properties)
            {
                var attr = AnnotationUtils.GetPropertyMap(prop);
                if(prop.GetMethod != null && !string.IsNullOrEmpty(attr.GetterName))
                    code.Add($"duk_idx_t {GetPropertySignature(prop)}(duk_context* ctx);");
                if(prop.SetMethod != null && !string.IsNullOrEmpty(attr.SetterName))
                    code.Add($"duk_idx_t {GetPropertySignature(prop, true)}(duk_context* ctx);");
            }
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
        protected virtual void EmitCtorSignatures(CodeBuilder code)
        {
            var ctors = AnnotationUtils.GetConstructors(Target);
            code.Add("// @ constructor definitions");

            code.Add($"duk_idx_t {GetCtorSignature()}(duk_context* ctx);");
            if (ctors.Count == 1)
                return;

            for(int i =0; i < ctors.Count; ++i)
            {
                code.Add($"duk_idx_t {GetVariantCtorSignature(i)}(duk_context* ctx);");
            }
        }
        protected virtual void EmitSetupStaticSignatures(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetSetupStaticSignature(Target)}(duk_context* ctx);");
        }
        #endregion

        public virtual void EmitSourceIncludes(CodeBuilder code)
        {
            HashSet<string> includes = new HashSet<string>();
            // add self header
            includes.Add($"#include \"{GetSelfHeader()}\"");
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
                        Type returnType = method.ReturnType;
                        if(AnnotationUtils.IsCustomCodeMethod(method))
                        {
                            var attr = AnnotationUtils.GetCustomCodeAttribute(method);
                            returnType = attr.ReturnType;
                            usedTypes = usedTypes.Concat(attr.ParameterTypes);
                        }
                        else
                        {
                            usedTypes = usedTypes.Concat(method.GetParameters().Select(x => x.ParameterType));
                        }

                        if (returnType != typeof(void))
                            usedTypes = usedTypes.Concat(new Type[] { returnType });
                    });
                };

                GetMethods().Values.ToList().ForEach(methodTypesCollect);
                GetOperatorMethods().Values.ToList().ForEach(methodTypesCollect);
                GetMethods(StaticMethodsFlags).Values.ToList().ForEach(methodTypesCollect);
                GetProperties().ForEach(prop => usedTypes = usedTypes.Concat(new Type[] { prop.PropertyType }));
                AnnotationUtils.GetVariables(Target).ForEach(vary => usedTypes = usedTypes.Concat(new Type[] { vary.Type }));
                AnnotationUtils.GetConstructors(Target).ForEach(ctor => usedTypes = usedTypes.Concat(ctor.Types));
            }


            // add all headers from collected types action.
            Action<Type> insertType = (type) =>
            {
                if (type.IsSubclassOf(typeof(ClassObject)))
                    includes.Add($"#include \"{type.Name}{Constants.ClassIncludeSuffix}.h\"");
                else if (type.IsSubclassOf(typeof(PrimitiveObject)))
                    includes.Add($"#include \"{type.Name}{Constants.PrimitiveIncludeSuffix}.h\"");
                else if (type.IsSubclassOf(typeof(StructObject)))
                    includes.Add($"#include \"{type.Name}{Constants.StructIncludeSuffix}.h\"");
            };


            int nextIdx = 0;
            Type? processType = null;

            while(nextIdx < usedTypes.Count())
            {
                if(processType is null)
                {
                    processType = usedTypes.ElementAt(nextIdx);
                    if (processType is null)
                        ++nextIdx;
                    continue;
                }

                if(processType.IsSubclassOf(typeof(TemplateObject)))
                {
                    TemplateObject templateObj = TemplateObject.Create(processType);
                    processType = templateObj.TargetType;
                    continue;
                }

                insertType(processType);
                ++nextIdx;
                processType = null;
            }

            // finally, generate all headers into code
            code.Add(includes.ToArray()).AddNewLine();
        }
        public virtual void EmitSource(CodeBuilder code)
        {
            EmitSourceMethodLookupTable(code);
            EmitSourceConstructors(code);
            EmitSourceSetup(code);
            EmitSourceSetupStatic(code);
            EmitSourceWrap(code);
            EmitSourceGetRef(code);
            EmitSourceVariables(code);
            EmitSourceProperties(code);
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
            // ctors methods
            var ctors = AnnotationUtils.GetConstructors(Target);
            
            if(ctors.Count > 1)
            {
                uint ctorNameHash = HashUtils.Hash(GetCtorSignature());
                hasVariants = true;

                for(int i =0; i < ctors.Count; ++i)
                {
                    funcHash = ctorNameHash;

                    var ctor = ctors[i];
                    var ctorSignature = GetVariantCtorSignature(i);
                    var paramTypes = ctor.Types.ToList();
                    CodeBuilder ctorEntriesCode = new CodeBuilder();

                    hasStringHash = false;
                    paramTypes.ForEach(type => hashParamCalc(type, typeof(string)));

                    ctorEntriesCode.Add($"{{ StringHash({(funcHash == 0u ? "0u" : funcHash.ToString())}), {ctorSignature} }},");

                    if (hasStringHash)
                    {
                        funcHash = ctorNameHash;
                        paramTypes.ForEach(param => hashParamCalc(param, typeof(uint)));

                        ctorEntriesCode.Add($"{{ StringHash({(funcHash == 0u ? "0u" : funcHash.ToString())}), {ctorSignature} }},");
                    }

                    lookupField.Add(ctorEntriesCode);
                }
            }
            lookupField.Add("};").AddNewLine();

            if (hasVariants)
                code.Add(lookupField);
        }

        protected virtual void EmitSourceGetRef(CodeBuilder code)
        {
            code
                .Add($"{AnnotationUtils.GetTypeName(Target)}* {GetRefSignature()}(duk_context* ctx, duk_idx_t obj_idx)")
                .Scope(code =>
                {
                    code
                        .Add($"if(!duk_get_prop_string(ctx, obj_idx, JS_OBJ_HIDDEN_PTR))")
                        .Scope(code =>
                        {
                            code
                                .Add("duk_pop(ctx);")
                                .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"could not possible to get ref of object '{AnnotationUtils.GetTypeName(Target)}. Internal ptr prop does not exists.'\");")
                                .Add("duk_throw(ctx);")
                                .Add("return nullptr;");
                        })
                        .Add($"{AnnotationUtils.GetTypeName(Target)}* result = static_cast<{AnnotationUtils.GetTypeName(Target)}*>(duk_get_pointer(ctx, -1));")
                        .Add("duk_pop(ctx);")
                        .Add("return result;");
                });
        }
        protected virtual void EmitSourceWrap(CodeBuilder code)
        {
            code
                .Add($"void {CodeUtils.GetMethodPrefix(Target)}_wrap(duk_context* ctx, duk_idx_t obj_idx, {AnnotationUtils.GetTypeName(Target)}* instance)")
                .Scope(code =>
                {
                    if (Target.BaseType == typeof(ClassObject))
                        code.Add("rbfx_object_wrap(ctx, obj_idx, instance);");
                    else if (Target.BaseType != null && Target.BaseType != typeof(PrimitiveObject))
                        code.Add($"{CodeUtils.GetMethodPrefix(Target.BaseType)}_wrap(ctx, obj_idx, instance);");
                    EmitVariables(code, "obj_idx");
                    EmitProperties(code, "obj_idx");
                    EmitOperatorMethods(code, "obj_idx");
                    EmitMethods(code, "obj_idx");
                });
        }
        protected virtual void EmitSourceSetup(CodeBuilder code)
        {
            code
                .Add($"void {CodeUtils.GetMethodPrefix(Target)}_setup(duk_context* ctx)")
                .Scope(code => EmitSetupBody(code));
        }
        protected virtual void EmitSourceSetupStatic(CodeBuilder code)
        {
            code
                .Add($"void {CodeUtils.GetSetupStaticSignature(Target)}(duk_context* ctx)")
                .Scope(code => EmitSetupStaticBody(code));
        }

        protected virtual void EmitSourceConstructors(CodeBuilder code)
        {
            var ctors = AnnotationUtils.GetConstructors(Target);

            code.Add($"duk_idx_t {GetCtorSignature()}(duk_context* ctx)");
            if(ctors.Count == 1)
            {
                code.Scope(code => {
                    if (ctors[0].IsDefault)
                        EmitGenericConstructorBody(ctors[0], code);
                    else
                        EmitConstructorBody(ctors[0], code);
                });
                return;
            }

            // write constructor resolver
            code.Scope(code =>
            {
                int minArgs = 0;
                int maxArgs = 0;

                // calc min and max args
                ctors.ForEach(data =>
                {
                    minArgs = Math.Min(minArgs, data.Types.Count());
                    maxArgs = Math.Max(maxArgs, data.Types.Count());
                });

                EmitConstructorCallValidation(code);

                code.Add($"unsigned ctorHash = {HashUtils.Hash(GetCtorSignature())};")
                    .Add("duk_idx_t argc = duk_get_top(ctx);")
                    .AddNewLine()
                    .Add("// validate arguments count");

                if(minArgs == maxArgs)
                {
                    code
                        .Add($"if(argc != {maxArgs}")
                        .Scope(ifCode =>
                        {
                            ifCode
                                .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid arguments to constructor '{GetJSTypeIdentifier()}'.\");")
                                .Add("return duk_throw(ctx);");
                        });
                }
                else
                {
                    string validation = string.Empty;
                    if (minArgs > 0)
                        validation = $"argc < {minArgs} || ";

                    code
                        .Add($"if({validation}argc > {maxArgs})")
                        .Scope(ifCode =>
                        {
                            ifCode
                                .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid arguments to constructor '{GetJSTypeIdentifier()}'.\");")
                                .Add("return duk_throw(ctx);");
                        });
                }

                code
                    .AddNewLine()
                    .Add("for (unsigned i = 0; i < argc; ++i)")
                    .Scope(code =>
                    {
                        code
                            .Add("StringHash typeHash = rbfx_get_type(ctx, i);")
                            .Add("CombineHash(ctorHash, typeHash.Value());");
                    })
                    .AddNewLine()
                    .Add("const auto funcIt = g_functions.find(StringHash(ctorHash));")
                    .Add("if (funcIt == g_functions.end())")
                    .Scope(code =>
                    {
                        code
                            .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid arguments to constructor '{GetJSTypeIdentifier()}'.\");")
                            .Add("return duk_throw(ctx);");
                    })
                    .Add("return funcIt->second(ctx);");
            });

            for(int i =0; i < ctors.Count; ++i)
            {
                code
                    .Add($"duk_idx_t {GetVariantCtorSignature(i)}(duk_context* ctx)")
                    .Scope(code =>
                    {
                        EmitConstructorBody(ctors[i], code);
                    });
            }
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
                            int paramsLength = x.GetParameters().Length;
                            if (AnnotationUtils.IsCustomCodeMethod(x))
                                paramsLength = AnnotationUtils.GetCustomCodeAttribute(x).ParameterTypes.Length;
                            minArgs = Math.Min(minArgs, paramsLength);
                            maxArgs = Math.Max(maxArgs, paramsLength);
                        });

                        // emit method selection
                        scopeCode
                            .Add($"unsigned methodHash = {HashUtils.Hash((isStatic ? "static" : string.Empty) + funcName)};")
                            .Add("duk_idx_t argc = duk_get_top(ctx);")
                            .AddNewLine()
                            .Add("// validate arguments count");

                        if(minArgs == maxArgs)
                        {
                            scopeCode
                                .Add($"if(argc != {maxArgs}")
                                .Scope(ifCode =>
                                {
                                    ifCode
                                        .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid arguments to function '{funcName}'.\");")
                                        .Add("return duk_throw(ctx);");
                                });
                        }
                        else
                        {
                            string validation = string.Empty;
                            if (minArgs > 0)
                                validation = $"argc < {minArgs} || ";

                            scopeCode
                                .Add($"if({validation}argc > {maxArgs})")
                                .Scope(ifCode =>
                                {
                                    ifCode
                                        .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid arguments to function '{funcName}'.\");")
                                        .Add("return duk_throw(ctx);");
                                });
                        }

                        scopeCode
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
                            EmitStaticMethodBody(methods[i], scopeCode);
                        else if (operatorType == OperatorType.None)
                            EmitMethodBody(methods[i], scopeCode);
                        else
                            EmitOperatorMethodBody(operatorType, methods[i], scopeCode);
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
        protected virtual void EmitSourceVariables(CodeBuilder code)
        {
            AnnotationUtils.GetVariables(Target).ForEach(variable =>
            {
                EmitVariableGetter(variable, code);
                EmitVariableSetter(variable, code);
            });
        }
        protected virtual void EmitSourceProperties(CodeBuilder code)
        {
            GetProperties().ForEach(prop =>
            {
                var attr = AnnotationUtils.GetPropertyMap(prop);
                if (prop.GetMethod != null && !string.IsNullOrEmpty(attr.GetterName))
                    EmitPropertyGetter(prop, code);
                if (prop.SetMethod != null && !string.IsNullOrEmpty(attr.SetterName))
                    EmitPropertySetter(prop, code);
            });
        }

        protected virtual void EmitSetupBody(CodeBuilder code)
        {
            code
                .Add($"duk_push_c_function(ctx, {GetCtorSignature()}, DUK_VARARGS);")
                .Add($"duk_put_global_string(ctx, \"{AnnotationUtils.GetJSTypeName(Target)}\");");
        }
        protected virtual void EmitSetupStaticBody(CodeBuilder code)
        {
            code
                .Add("duk_idx_t top = duk_get_top(ctx);")
                .Add($"duk_get_global_string(ctx, \"{AnnotationUtils.GetJSTypeName(Target)}\");");

            EmitStaticFields(code, "top");
            EmitStaticMethods(code, "top");

            code.Add("duk_pop(ctx);");
        }
        
        protected virtual void EmitCustomConstructorBody(ConstructorData ctor, CodeBuilder code)
        {
            for (int i = 0; i < ctor.Types.Count(); ++i)
                CodeUtils.EmitValueRead(ctor.Types.ElementAt(i), $"arg{i}", i.ToString(), code);

            var modifiers = new ConstructorData.CustomCodeModifiers();
            code
                .Add("duk_idx_t result_code = 1;")
                .Add("duk_idx_t this_idx = duk_get_top(ctx);")
                .AddNewLine()
                .Add("duk_push_this(ctx);")
                .Add("// begin custom code");
            // execute custom code call
            ctor.CustomCodeCall(this, code, modifiers);

            code
                .Add("// end custom code")
                .AddNewLine();

            if (!modifiers.SkipTypeInsert)
            {
                code
                    .Add($"duk_push_string(ctx, \"{GetJSTypeIdentifier()}\");")
                    .Add("duk_put_prop_string(ctx, this_idx, \"type\");");
            }
            if(!modifiers.SkipPtrInsert)
            {
                code
                    .Add("duk_push_pointer(ctx, instance);")
                    .Add("duk_put_prop_string(ctx, this_idx, JS_OBJ_HIDDEN_PTR);");
            }
            if (!modifiers.SkipFinalizer)
                code.Add($"{CodeUtils.GetMethodPrefix(Target)}_set_finalizer(ctx, this_idx, instance);");
            if (!modifiers.SkipWrap)
                code.Add($"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, this_idx, instance);");
            code.AddNewLine()
                .Add("duk_dup(ctx, this_idx);")
                .Add("return result_code;");
        }
        protected virtual string GetConstructorArgsCall(ConstructorData ctor)
        {
            StringBuilder result = new StringBuilder();
            for(int i =0; i < ctor.Types.Count(); ++i)
            {
                result.Append($"arg{i}");
                if (i < ctor.Types.Count() - 1)
                    result.Append(", ");
            }

            return result.ToString();
        }

        protected virtual void EmitMethodBody(MethodInfo methodInfo, CodeBuilder code)
        {
            var parameters = methodInfo.GetParameters().ToList();

            EmitArgumentValidation(parameters, methodInfo.GetCustomAttribute<CustomCodeAttribute>(), code);

            string nativeName = AnnotationUtils.GetMethodNativeName(methodInfo);

            code.Add("duk_push_this(ctx);");
            code.Add($"{AnnotationUtils.GetTypeName(Target)}* instance = {GetRefSignature()}(ctx, duk_get_top_index(ctx));");
            code.Add("duk_pop(ctx);");

            CustomCodeAttribute? customCodeAttr = methodInfo.GetCustomAttribute<CustomCodeAttribute>();
            if (customCodeAttr != null)
            {
                EmitCustomCodeMethodBody(customCodeAttr, methodInfo, code);
                return;
            }

            StringBuilder argsCall = new StringBuilder();
            for (int i = 0; i < parameters.Count; ++i)
            {
                CodeUtils.EmitValueRead(parameters[i].ParameterType, $"arg{i}", i.ToString(), code);
                argsCall.Append($"arg{i}");
                if (i < parameters.Count - 1)
                    argsCall.Append(", ");
            }

            if (methodInfo.ReturnType == typeof(void))
            {
                code.Add($"instance->{nativeName}({argsCall});");
                code.Add("return 0;");
            }
            else
            {
                code.Add($"{CodeUtils.GetNativeDeclaration(methodInfo.ReturnType)} result = instance->{nativeName}({argsCall});");
                CodeUtils.EmitValueWrite(methodInfo.ReturnType, "result", code);
                code.Add("return 1;");
            }
        }
        protected virtual void EmitOperatorMethodBody(OperatorType opType, MethodInfo method, CodeBuilder code)
        {
            var parameters = method.GetParameters().ToList();
            EmitArgumentValidation(parameters, method.GetCustomAttribute<CustomCodeAttribute>(), code);

            string operatorSignal = GetOperatorSignal(opType);

            code
                .Add("duk_push_this(ctx);")
                .Add($"{AnnotationUtils.GetTypeName(Target)}& instance = {CodeUtils.GetMethodPrefix(Target)}_resolve(ctx, -1);")
                .Add("duk_pop(ctx);")
                .AddNewLine();

            CustomCodeAttribute? customCodeAttr = method.GetCustomAttribute<CustomCodeAttribute>();
            if (customCodeAttr != null)
            {
                EmitCustomCodeMethodBody(customCodeAttr, method, code);
                return;
            }

            if (parameters.Count > 1)
                throw new Exception($"Operators that contains more than 1 argument must add custom code attribute. Error Class: {Target.FullName}");

            CodeUtils.EmitValueRead(parameters[0].ParameterType, $"value", "0", code);

            if (opType == OperatorType.Equal)
            {
                code
                    .Add($"duk_push_boolean(ctx, instance == value);")
                    .Add("return 1;");
                return;
            }

            code.Add($"{CodeUtils.GetNativeDeclaration(method.ReturnType)} result = instance {operatorSignal} value;");
            CodeUtils.EmitValueWrite(method.ReturnType, "result", code);
            code.Add("return 1;");
        }
        protected virtual void EmitStaticMethodBody(MethodInfo methodInfo, CodeBuilder code)
        {
            string nativeName = AnnotationUtils.GetMethodNativeName(methodInfo);

            var parameters = methodInfo.GetParameters().ToList();

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
                if (methodInfo.IsStatic)
                {
                    code.Add("return result_code;");
                    return;
                }

                code.Add("return result_code;");
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

            EmitArgumentValidation(parameterTypes, code);
        }
        protected virtual void EmitArgumentValidation(ConstructorData ctor, CodeBuilder code)
        {
            EmitArgumentValidation(ctor.Types.ToList(), code);
        }
        protected virtual void EmitArgumentValidation(List<Type> types, CodeBuilder code)
        {
            if (types.Count == 0)
                return;

            code.Add("#if defined(URHO3D_DEBUG)");

            for (int i = 0; i < types.Count; ++i)
            {
                uint typeHash = CodeUtils.GetTypeHash(types[i]);
                if (TemplateObject.IsVectorType(types[i]))
                {
                    CodeBuilder topCodeValidation = new CodeBuilder();
                    CodeBuilder codeValidation = topCodeValidation;
                    codeValidation.IndentationSize = 0;

                    codeValidation.Add($"duk_dup(ctx, {i});");

                    uint arrayHash = HashUtils.Hash("Vector");
                    Type type = types[i];

                    while (TemplateObject.IsVectorType(type))
                    {
                        codeValidation
                            .Add($"rbfx_require_type(ctx, -1, {arrayHash}/*Array*/);")
                            .Add($"if (duk_get_length(ctx, -1) > 0)")
                            .Scope(code =>
                            {
                                code.Add("duk_get_prop_index(ctx, -1, 0);");
                                codeValidation = code;
                                TemplateObject tobj = TemplateObject.Create(type);
                                type = tobj.TargetType;
                            });

                    }

                    typeHash = CodeUtils.GetTypeHash(type);
                    codeValidation.Add($"rbfx_require_type(ctx, -1, {typeHash}/*{AnnotationUtils.GetTypeName(type)}*/);");

                    code.Scope(code =>
                    {
                        code
                            .Add("duk_idx_t arg_top = duk_get_top(ctx);")
                            .Add(topCodeValidation)
                            .Add("duk_pop_n(ctx, duk_get_top(ctx) - arg_top);");
                    });
                    continue;
                }

                code.Add($"rbfx_require_type(ctx, {i}, {typeHash}/*{AnnotationUtils.GetTypeName(types[i])}*/);");
            }

            code.Add("#endif").AddNewLine();
        }
        #region Methods Setup Bindings
        protected virtual void EmitMethods(CodeBuilder code, string accessor)
        {
            var methodsData = GetMethods();

            if (methodsData.Count > 0)
                code.Add("// methods setup");

            foreach (var pair in methodsData)
            {
                int maxArgs = 0;
                int minArgs = 0;
                string methodSignature = GetMethodSignature(pair.Key);

                pair.Value.ForEach(x =>
                {
                    int paramCount = x.GetParameters().Length;
                    var customCodeAttr = x.GetCustomAttribute<CustomCodeAttribute>();
                    if (customCodeAttr != null)
                        paramCount = customCodeAttr.ParameterTypes.Length;

                    maxArgs = Math.Max(maxArgs, paramCount);
                    minArgs = Math.Min(minArgs, paramCount);
                });

                if (pair.Value.Count <= 1 || minArgs == maxArgs)
                    code.Add($"duk_push_c_lightfunc(ctx, {methodSignature}, {maxArgs}, {maxArgs}, 0);");
                else
                    code.Add($"duk_push_c_function(ctx, {methodSignature}, DUK_VARARGS);");

                code.Add($"duk_put_prop_string(ctx, {accessor}, \"{pair.Key}\");");
            }
        }
        protected virtual bool EmitStaticMethods(CodeBuilder code, string accessor)
        {
            var methodsData = GetMethods(StaticMethodsFlags);
            if (methodsData.Count > 0)
                code.Add("// static methods setup");

            foreach(var pair in methodsData)
            {
                int maxArgs = 0;
                int minArgs = 0;
                string methodSignature = GetStaticMethodSignature(pair.Key);

                pair.Value.ForEach(x =>
                {
                    int paramCount = x.GetParameters().Length;
                    var customCodeAttr = x.GetCustomAttribute<CustomCodeAttribute>();
                    if (customCodeAttr != null)
                        paramCount = customCodeAttr.ParameterTypes.Length;

                    maxArgs = Math.Max(maxArgs, paramCount);
                    minArgs = Math.Min(minArgs, paramCount);
                });

                if (pair.Value.Count <= 1 || minArgs == maxArgs)
                    code.Add($"duk_push_c_lightfunc(ctx, {methodSignature}, {maxArgs}, {maxArgs}, 0);");
                else
                    code.Add($"duk_push_c_function(ctx, {methodSignature}, DUK_VARARGS);");

                code.Add($"duk_put_prop_string(ctx, {accessor}, \"{pair.Key}\");");
            }

            return methodsData.Count > 0;
        }
        protected virtual void EmitOperatorMethods(CodeBuilder code, string accessor)
        {
            var methodsData = GetOperatorMethods();
            if (methodsData.Count > 0)
                code.Add("// operator methods setup");

            foreach(var pair in methodsData)
            {
                int maxArgs = 0;
                int minArgs = 0;
                string methodSignature = GetOperatorMethodSignature(pair.Key);

                pair.Value.ForEach(x =>
                {
                    int paramCount = x.GetParameters().Length;
                    var customCodeAttr = x.GetCustomAttribute<CustomCodeAttribute>();
                    if (customCodeAttr != null)
                        paramCount = customCodeAttr.ParameterTypes.Length;

                    maxArgs = Math.Max(maxArgs, paramCount);
                    minArgs = Math.Min(minArgs, paramCount);
                });

                if (pair.Value.Count <= 1 || minArgs == maxArgs)
                    code.Add($"duk_push_c_lightfunc(ctx, {methodSignature}, {maxArgs}, {maxArgs}, 0);");
                else
                    code.Add($"duk_push_c_function(ctx, {methodSignature}, DUK_VARARGS);");

                code.Add($"duk_put_prop_string(ctx, {accessor}, \"{GetOperatorName(pair.Key)}\");");
            }
        }
        protected virtual void EmitVariables(CodeBuilder code, string accessor)
        {
            var variables = AnnotationUtils.GetVariables(Target);
            if (variables.Count > 0)
                code.Add("// variables setup");

            foreach(var variable in variables)
            {
                code
                    .Add($"duk_push_string(ctx, \"{variable.JSName}\");")
                    .Add($"duk_push_c_lightfunc(ctx, {GetVariableSignature(variable)}, 0, 0, 1);")
                    .Add($"duk_push_c_lightfunc(ctx, {GetVariableSignature(variable, true)}, 1, 1, 1);")
                    .Add($"duk_def_prop(ctx, {accessor}, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_SETTER);");
            }
        }
        protected virtual void EmitProperties(CodeBuilder code, string accessor)
        {
            var props = GetProperties();

            if (props.Count > 0)
                code.Add("// properties setup");

            foreach (var prop in props)
            {
                List<string> enumFlags = new List<string>();
                enumFlags.Add("DUK_DEFPROP_HAVE_ENUMERABLE");

                code.Add($"duk_push_string(ctx, \"{CodeUtils.ToCamelCase(AnnotationUtils.GetJSPropertyName(prop))}\");");

                var attr = AnnotationUtils.GetPropertyMap(prop);
                if (prop.GetMethod != null && !string.IsNullOrEmpty(attr.GetterName))
                {
                    code.Add($"duk_push_c_lightfunc(ctx, {GetPropertySignature(prop)}, 0, 0, 1);");
                    enumFlags.Add("DUK_DEFPROP_HAVE_GETTER");
                }
                if (prop.SetMethod != null && !string.IsNullOrEmpty(attr.SetterName))
                {
                    code.Add($"duk_push_c_lightfunc(ctx, {GetPropertySignature(prop, true)}, 1, 1, 1);");
                    enumFlags.Add("DUK_DEFPROP_HAVE_SETTER");
                }

                StringBuilder propFlags = new StringBuilder();
                for (int i = 0; i < enumFlags.Count; ++i)
                {
                    propFlags.Append(enumFlags[i]);
                    if (i < enumFlags.Count - 1)
                        propFlags.Append(" | ");
                }

                code.Add($"duk_def_prop(ctx, {accessor}, {propFlags});");
            }

            if (props.Count > 0)
                code.AddNewLine();
        }
        protected virtual bool EmitStaticFields(CodeBuilder code, string accessor)
        {
            CodeBuilder target = new CodeBuilder();
            target.IndentationSize = 0;

            int totalMethods = 0;
            Target
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.GetCustomAttribute<FieldAttribute>() != null)
                .ToList()
                .ForEach(field =>
                {
                    EmitStaticField(field, target, accessor);
                    ++totalMethods;
                });

            Target
                .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(x => x.GetCustomAttribute<CustomFieldAttribute>() != null)
                .ToList()
                .ForEach(method =>
                {
                    EmitStaticField(method, target, accessor);
                    ++totalMethods;
                });

            if (totalMethods > 0)
                code.Add("// @ static fields setup");
            code.Add(target);

            return totalMethods > 0;
        }
        #endregion
        #region Variables and Properties Body
        protected virtual void EmitVariableGetter(BindingVariable variable, CodeBuilder code)
        {
            code
                .Add($"duk_idx_t {GetVariableSignature(variable)}(duk_context* ctx)")
                .Scope(code =>
                {
                    code
                        .Add("duk_push_this(ctx);")
                        .Add($"{AnnotationUtils.GetTypeName(Target)}* instance = {GetRefSignature()}(ctx, duk_get_top_index(ctx));")
                        .Add("duk_pop(ctx);")
                        .Add($"{CodeUtils.GetNativeDeclaration(variable.Type)} result = instance->{variable.NativeName};");
                    CodeUtils.EmitValueWrite(variable.Type, "result", code);
                    code.Add("return 1;");
                });
        }
        protected virtual void EmitVariableSetter(BindingVariable variable, CodeBuilder code)
        {
            code
                .Add($"duk_idx_t {GetVariableSignature(variable, true)}(duk_context* ctx)")
                .Scope(code =>
                {
                    code
                        .Add("duk_push_this(ctx);")
                        .Add($"{AnnotationUtils.GetTypeName(Target)}* instance = {GetRefSignature()}(ctx, duk_get_top_index(ctx));")
                        .Add("duk_pop(ctx);");
                    CodeUtils.EmitValueRead(variable.Type, "result", "0", code);
                    code
                        .Add($"instance->{variable.NativeName} = result;")
                        .Add("return 0;");
                });
        }
        protected virtual void EmitPropertyGetter(PropertyInfo prop, CodeBuilder code)
        {
            code
                .Add($"duk_idx_t {GetPropertySignature(prop)}(duk_context* ctx)")
                .Scope(code =>
                {
                    code
                        .Add("duk_push_this(ctx);")
                        .Add($"{AnnotationUtils.GetTypeName(Target)}* instance = {GetRefSignature()}(ctx, duk_get_top_index(ctx));")
                        .Add("duk_pop(ctx);")
                        .Add($"{CodeUtils.GetNativeDeclaration(prop.PropertyType)} result = instance->{AnnotationUtils.GetGetterName(prop)}();");
                    CodeUtils.EmitValueWrite(prop.PropertyType, "result", code);
                    code.Add("return 1;");
                });
        }
        protected virtual void EmitPropertySetter(PropertyInfo prop, CodeBuilder code)
        {
            code
                .Add($"duk_idx_t {GetPropertySignature(prop, true)}(duk_context* ctx)")
                .Scope(code =>
                {
                    code
                        .Add("duk_push_this(ctx);")
                        .Add($"{AnnotationUtils.GetTypeName(Target)}* instance = {GetRefSignature()}(ctx, duk_get_top_index(ctx));")
                        .Add("duk_pop(ctx);");
                    CodeUtils.EmitValueRead(prop.PropertyType, "result", "0", code);

                    code
                        .AddNewLine()
                        .Add($"instance->{AnnotationUtils.GetSetterName(prop)}(result);")
                        .Add("return 0;");
                });
        }
        protected virtual void EmitStaticField(FieldInfo field, CodeBuilder code, string accessor)
        {
            FieldAttribute fieldAttr = AnnotationUtils.GetFieldAttribute(field);
            code.Add($"{CodeUtils.GetMethodPrefix(Target)}_push(ctx, {AnnotationUtils.GetTypeName(field.FieldType)}::{fieldAttr.NativeName});");
            code.Add($"duk_put_prop_string(ctx, {accessor}, \"{fieldAttr.JSName}\");");
        }
        protected virtual void EmitStaticField(MethodInfo method, CodeBuilder code, string accessor)
        {
            CustomFieldAttribute attr = AnnotationUtils.GetCustomFieldAttribute(method);
            method.Invoke(this, new object[] { code });
            code.Add($"duk_put_prop_string(ctx, {accessor}, \"{attr.JSName}\");");
        }
        #endregion

        protected virtual void EmitConstructorCallValidation(CodeBuilder code)
        {
            code.Add("if (!duk_is_constructor_call(ctx))")
                .Scope(code =>
                {
                    code
                        .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid constructor call. must call with 'new {GetJSTypeIdentifier()}(...args)'\");")
                        .Add("return duk_throw(ctx);");
                })
                .AddNewLine();
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

        protected virtual string GetJSTypeIdentifier()
        {
            return AnnotationUtils.GetTypeName(Target);
        }

        protected string GetCtorSignature()
        {
            return $"{CodeUtils.GetMethodPrefix(Target)}_ctor_call"; ;
        }
        protected string GetVariantCtorSignature(int index)
        {
            return $"{CodeUtils.GetMethodPrefix(Target)}_ctor{index}_call";
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
                    op = "equals";
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
        protected string GetPropertySignature(PropertyInfo prop, bool isSetter = false)
        {
            if (isSetter)
                return $"{CodeUtils.GetMethodPrefix(Target)}_prop_setter_{CodeUtils.ToSnakeCase(prop.Name)}_call";
            return $"{CodeUtils.GetMethodPrefix(Target)}_prop_getter_{CodeUtils.ToSnakeCase(prop.Name)}_call";
        }
        protected string GetVariableSignature(BindingVariable variable, bool isSetter = false)
        {
            if (isSetter)
                return $"{CodeUtils.GetMethodPrefix(Target)}_var_setter_{CodeUtils.ToSnakeCase(variable.JSName)}_call";
            return $"{CodeUtils.GetMethodPrefix(Target)}_var_getter_{CodeUtils.ToSnakeCase(variable.JSName)}_call";
        }
        protected string GetRefSignature()
        {
            return CodeUtils.GetRefSignature(Target);
        }

        protected abstract string GetSelfHeader();
        protected abstract void EmitConstructorBody(ConstructorData ctor, CodeBuilder code);
        protected abstract void EmitGenericConstructorBody(ConstructorData ctor, CodeBuilder code);
    }
}
