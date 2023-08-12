namespace JSBindTool.Core
{
    public class PrimitiveObject : BaseObject
    {
        public PrimitiveObject(Type type) : base(type)
        {
            ValidateInheritance<PrimitiveObject>(type);
        }

        protected override string GetSelfHeader()
        {
            return $"{Target.Name}{Constants.PrimitiveIncludeSuffix}.h";
        }
        protected override string GetBaseTypeHeader()
        {
            if (Target.BaseType == null || Target.BaseType == typeof(PrimitiveObject))
                return string.Empty;
            return $"#include \"{Target.BaseType.Name}{Constants.PrimitiveIncludeSuffix}.h\"";
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
                    AnnotationUtils.GetVariables(Target).ForEach(vary =>
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
                        .Add($"duk_push_string(ctx, \"{AnnotationUtils.GetJSTypeName(Target)}\");")
                        .Add("duk_put_prop_string(ctx, obj_idx, \"type\");")
                        .Add($"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, obj_idx, instance);")
                        .Add("duk_dup(ctx, obj_idx);");
                });
        }

        public virtual void EmitPushSource(CodeBuilder code)
        {
            code.Add($"void {CodeUtils.GetPushSignature(Target)}(duk_context* ctx, const {AnnotationUtils.GetTypeName(Target)}& value)");
            code.Scope(scope =>
            {
                scope.Add($"duk_get_global_string(ctx, \"{AnnotationUtils.GetJSTypeName(Target)}\");");
                scope.Add("duk_new(ctx, 0);");
                var variables = AnnotationUtils.GetVariables(Target);
                variables.ForEach(vary =>
                {
                    CodeUtils.EmitValueWrite(vary.Type, "value."+vary.NativeName, scope);
                    scope.Add($"duk_put_prop_string(ctx, -2, \"{vary.JSName}\");");
                });
            });
        }

        protected override void EmitSetupBody(CodeBuilder code)
        {
            base.EmitSetupBody(code);
            EmitInitializationNotice(code);
        }
        protected override void EmitSetupStaticBody(CodeBuilder code)
        {
            base.EmitSetupStaticBody(code);
            EmitInitializationNotice(code);
        }

        protected override void EmitConstructorBody(ConstructorData ctor, CodeBuilder code)
        {
            EmitConstructorCallValidation(code);
            EmitArgumentValidation(ctor, code);

            if (ctor.HasCustomCode)
            {
                EmitCustomConstructorBody(ctor, code);
                return;
            }

            for(int i =0; i < ctor.Types.Count(); ++i)
                CodeUtils.EmitValueRead(ctor.Types.ElementAt(i), $"arg{i}", i.ToString(), code);

            code
                .Add($"{AnnotationUtils.GetTypeName(Target)}* instance = new {AnnotationUtils.GetTypeName(Target)}({GetConstructorArgsCall(ctor)});")
                .AddNewLine()
                .Add(
                    "duk_push_this(ctx);",
                    "duk_idx_t this_idx = duk_get_top_index(ctx);"
                )
                .AddNewLine();
            EmitTypeProperty(code, "this_idx");
            code
                .Add("duk_push_pointer(ctx, instance);")
                .Add("duk_put_prop_string(ctx, this_idx, JS_OBJ_HIDDEN_PTR);")
                .Add("// setup finalizer")
                .Add($"{CodeUtils.GetMethodPrefix(Target)}_set_finalizer(ctx, this_idx, instance);")
                .Add($"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, this_idx, instance);");
            EmitInstanceOf(code, "this_idx");
            code
                .AddNewLine()
                .Add("duk_push_this(ctx);")
                .Add("return 1;");
        }
        protected override void EmitGenericConstructorBody(ConstructorData ctor, CodeBuilder code)
        {
            EmitConstructorCallValidation(code);
            code
                .Add("duk_idx_t argc = duk_get_top(ctx);")
                .Add($"{AnnotationUtils.GetTypeName(Target)}* instance = nullptr;")
                .AddNewLine()
                .Scope(ctorScope =>
                {
                    ctorScope.Add($"instance = new {AnnotationUtils.GetTypeName(Target)}();");
                    int idx = 0;
                    AnnotationUtils.GetVariables(Target).ForEach(vary =>
                    {
                        ctorScope.Add($"if(argc > {idx})");
                        ctorScope.Scope(ifScope =>
                        {
                            CodeUtils.EmitValueRead(vary.Type, "value", idx.ToString(), ifScope);
                            ifScope.Add($"instance->{vary.NativeName} = value;");
                        });
                        ++idx;
                    });
                })
                .AddNewLine()
                .Add(
                    "duk_push_this(ctx);",
                    "duk_idx_t this_idx = duk_get_top_index(ctx);"
                )
                .AddNewLine();
            EmitTypeProperty(code, "this_idx");
            code.Add("duk_push_pointer(ctx, instance);")
                .Add("duk_put_prop_string(ctx, this_idx, JS_OBJ_HIDDEN_PTR);")
                .Add("// setup finalizer")
                .Add($"{CodeUtils.GetMethodPrefix(Target)}_set_finalizer(ctx, this_idx, instance);")
                .Add($"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, this_idx, instance);");
            EmitInstanceOf(code, "this_idx");
            code.AddNewLine()
                .Add("duk_push_this(ctx);")
                .Add("return 1;");
        }

        protected override void EmitParentInstanceOf(CodeBuilder code)
        {
            if (Target.BaseType == typeof(PrimitiveObject) || Target.BaseType == null)
            {
                code
                    .Add("duk_push_boolean(ctx, false);")
                    .Add("return 1;");
            }
            else
                code.Add($"return {CodeUtils.GetInstanceOfSignature(Target.BaseType)}(ctx, type);");
        }
        protected override void EmitParentWrapCall(CodeBuilder code, string accessor)
        {
            var baseType = Target.BaseType;
            if (Target.BaseType == typeof(PrimitiveObject) || baseType == null)
                return;
            code.Add($"{CodeUtils.GetMethodPrefix(baseType)}_wrap(ctx, {accessor}, instance);");
        }

        protected override void EmitThisInstance(CodeBuilder code, string varName)
        {
            code
                .Add("duk_push_this(ctx);")
                .Add($"{AnnotationUtils.GetTypeName(Target)}* {varName} = {GetRefSignature()}(ctx, duk_get_top_index(ctx));")
                .Add("duk_pop(ctx);");
        }

        private void EmitInitializationNotice(CodeBuilder code)
        {
            code.Add($"URHO3D_LOGDEBUG(\"- {AnnotationUtils.GetTypeName(Target)}\");");
        }
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
