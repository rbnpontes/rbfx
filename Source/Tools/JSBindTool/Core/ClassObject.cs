using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

namespace JSBindTool.Core
{
    [Abstract]
    [Include("Urho3D/Core/Object.h")]
    public class ClassObject : BaseObject
    {
        public ClassObject(Type type) : base(type)
        {
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
            code
                .Add("duk_idx_t top = duk_get_top_index(ctx);")
                .Add("duk_dup(ctx, -1);")
                .Add($"duk_put_global_string(ctx, \"{AnnotationUtils.GetJSTypeName(Target)}\");");

            EmitStaticFields(code, "top");
            EmitStaticMethods(code, "top");

            code
                .Add("duk_pop(ctx);")
                .AddNewLine()
                .Add($"URHO3D_LOGDEBUG(\"- {AnnotationUtils.GetTypeName(Target)}\");");
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
                    code.Add($"instance = new {AnnotationUtils.GetTypeName(Target)}(JavaScriptSystem::GetContext());");
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
                .Add("rbfx_lock_heapptr(ctx, heapptr);")
                .AddNewLine()
                .Add($"{CodeUtils.GetMethodPrefix(Target)}_wrap(ctx, obj_idx, instance);")
                .Add("rbfx_set_finalizer(ctx, obj_idx, instance);")
                .AddNewLine()
                .Add("duk_push_this(ctx);")
                .Add("return 1;");
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
