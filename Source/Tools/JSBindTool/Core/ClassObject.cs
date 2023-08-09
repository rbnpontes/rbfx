using JSBindTool.Core.Annotations;

namespace JSBindTool.Core
{
    [Abstract]
    [Include("Urho3D/Core/Object.h")]
    public class ClassObject : BaseObject
    {
        public ClassObject(Type type) : base(type)
        {
            ValidateInheritance<ClassObject>(type);
        }

        protected override string GetSelfHeader()
        {
            return $"{Target.Name}{Constants.ClassIncludeSuffix}.h";
        }
        protected override string GetBaseTypeHeader()
        {
            if (Target.BaseType == null || Target.BaseType == typeof(ClassObject))
                return string.Empty;
            return $"#include \"{Target.BaseType.Name}{Constants.ClassIncludeSuffix}.h\"";
        }
        protected override void EmitSourceGetRef(CodeBuilder code)
        {
            code
                .Add($"{AnnotationUtils.GetTypeName(Target)}* {GetRefSignature()}(duk_context* ctx, duk_idx_t obj_idx)")
                .Scope(code =>
                {
                    code.Add($"return static_cast<{AnnotationUtils.GetTypeName(Target)}*>(rbfx_get_instance(ctx, obj_idx));");
                });
        }

        protected override void EmitSetupBody(CodeBuilder code)
        {
            if (AnnotationUtils.IsAbstract(Target))
                code.Add($"duk_push_c_function(ctx, {GetCtorSignature()}, 1);");
            else
                code.Add($"duk_push_c_function(ctx, {GetCtorSignature()}, DUK_VARARGS);");
            code.Add($"duk_put_global_string(ctx, \"{AnnotationUtils.GetJSTypeName(Target)}\");");

            EmitInitializationNotice(code);
        }
        protected override void EmitSetupStaticBody(CodeBuilder code)
        {
            base.EmitSetupStaticBody(code);
            EmitInitializationNotice(code);
        }

        protected override void EmitConstructorBody(ConstructorData ctor, CodeBuilder code)
        {
            throw new NotImplementedException();
        }
        protected override void EmitGenericConstructorBody(ConstructorData ctor, CodeBuilder code)
        {
            if(AnnotationUtils.IsAbstract(Target))
            {
                code
                    .Add($"{AnnotationUtils.GetTypeName(Target)}* instance = static_cast<{AnnotationUtils.GetTypeName(Target)}*>(duk_require_pointer(ctx, 0));")
                    .Add($"duk_push_this(ctx);")
                    .Add($"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, duk_get_top(ctx) - 1, instance);")
                    .Add("return 1;");
                return;
            }

            EmitConstructorCallValidation(code);

            code
                .Add($"{AnnotationUtils.GetTypeName(Target)}* instance = nullptr;")
                .Add($"duk_idx_t obj_idx = duk_get_top(ctx);")
                .Add($"if (obj_idx > 1)")
                .Scope(code =>
                {
                    code
                        .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid '{GetJSTypeIdentifier()}' constructor call.\");")
                        .Add("return duk_throw(ctx);");
                })
                .AddNewLine()
                .Add("duk_push_this(ctx);")
                .Add("// if has argument, the first argument must be a pointer to Object*")
                .Add("if (obj_idx == 0)")
                .Scope(code =>
                {
                    EmitNewCall(code);
                })
                .Add("else")
                .Scope(code =>
                {
                    code
                        .Add($"instance = static_cast<{AnnotationUtils.GetTypeName(Target)}*>(duk_require_pointer(ctx, 0));")
                        .Add("if (!instance)")
                        .Scope(code =>
                        {
                            code
                                .Add($"duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid constructor '{GetJSTypeIdentifier()}' call. pointer argument is null\");")
                                .Add("return duk_throw(ctx);");
                        });
                })
                .AddNewLine()
                .Add("void* heapptr = instance->GetJSHeapptr();")
                .Add("if (rbfx_is_valid_heapptr(ctx, heapptr))")
                .Scope(code =>
                {
                    code
                        .Add("duk_push_heapptr(ctx, heapptr);")
                        .Add($"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, duk_get_top(ctx) - 1, instance);")
                        .Add("return 1;");
                })
                .AddNewLine()
                .Add("heapptr = duk_get_heapptr(ctx, obj_idx);")
                .Add("instance->SetJSHeapptr(heapptr);")
                .Add("instance->AddRef();")
                .Add("rbfx_lock_heapptr(ctx, heapptr);");

            EmitTypeProperty(code, "obj_idx");

            code.AddNewLine()
                .Add($"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, obj_idx, instance);")
                .Add("rbfx_set_finalizer(ctx, obj_idx, instance);");
            EmitInstanceOf(code, "obj_idx");
            code
                .AddNewLine()
                .Add("duk_push_this(ctx);")
                .Add("return 1;");
        }

        protected override void EmitParentInstanceOf(CodeBuilder code)
        {
            if(Target.BaseType == typeof(ClassObject) || Target.BaseType == null)
                code.Add("return rbfx_ref_counted_instanceof(ctx, type);");
            else
                code.Add($"return {CodeUtils.GetInstanceOfSignature(Target.BaseType)}(ctx, type);");
        }
        protected virtual void EmitNewCall(CodeBuilder code)
        {
            code.Add($"instance = new {AnnotationUtils.GetTypeName(Target)}();");
        }
        protected override void EmitParentWrapCall(CodeBuilder code, string accessor)
        {
            if (Target.BaseType == typeof(ClassObject))
                code.Add($"rbfx_ref_counted_wrap(ctx, {accessor}, instance);");
            else if(Target.BaseType != null)
                code.Add($"{CodeUtils.GetMethodPrefix(Target.BaseType)}_wrap(ctx, {accessor}, instance);");
        }
        private void EmitInitializationNotice(CodeBuilder code)
        {
            code.Add($"URHO3D_LOGDEBUG(\"- {AnnotationUtils.GetTypeName(Target)}\");");
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
