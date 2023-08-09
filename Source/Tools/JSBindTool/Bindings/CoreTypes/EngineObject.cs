using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.CoreTypes
{
    [Include("Urho3D/Core/Object.h")]
    [Abstract]
    public class EngineObject : ClassObject
    {
        public EngineObject(Type type) : base(type)
        {
            ValidateInheritance<EngineObject>(type);
        }
        public EngineObject() : base(typeof(EngineObject)) { }

        protected override string GetBaseTypeHeader()
        {
            if (Target.BaseType == typeof(EngineObject))
                return string.Empty;
            return base.GetBaseTypeHeader();
        }

        protected override void EmitNewCall(CodeBuilder code)
        {
            code.Add($"instance = new {AnnotationUtils.GetTypeName(Target)}(JavaScriptSystem::GetContext());");
        }
        protected override void EmitParentInstanceOf(CodeBuilder code)
        {
            if (Target.BaseType == typeof(EngineObject) || Target.BaseType == null)
                code.Add("return rbfx_object_instanceof(ctx, type);");
            else
                code.Add($"return {CodeUtils.GetInstanceOfSignature(Target.BaseType)}(ctx, type);");
        }
        protected override void EmitParentWrapCall(CodeBuilder code, string accessor)
        {
            if (Target.BaseType == typeof(EngineObject))
                code.Add($"rbfx_object_wrap(ctx, {accessor}, instance);");
            else if(Target.BaseType != null)
                code.Add($"{CodeUtils.GetMethodPrefix(Target.BaseType)}_wrap(ctx, {accessor}, instance);");
        }
        protected override void EmitTypeProperty(CodeBuilder code, string accessor)
        {
            // skip this method because rbfx_object_wrap will insert this property
        }
    }
}
