using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.ActionsTypes
{
#if URHO3D_ACTIONS
    [Include("Urho3D/Actions/ActionState.h")]
    [Abstract]
    [Namespace("Actions")]
    [Define("#define Actions_ActionState Urho3D::Actions::ActionState")]
    [TypeName("Actions_ActionState", "ActionState")]
    public class ActionState : ClassObject
    {
        [PropertyMap("GetTarget")]
        public new EngineObjectRef Target { get => new EngineObjectRef(); }
        [PropertyMap("GetOriginalTarget")]
        public EngineObjectRef OriginalTarget { get => new EngineObjectRef(); }
        [PropertyMap("GetAction")]
        public BaseAction Action { get => new BaseAction(); }
        [PropertyMap("IsDone")]
        public bool IsDone { get; }

        public ActionState() : base(typeof(ActionState))
        {
        }

        [Method]
        public void Update(float time) { }
        [Method]
        public void Stop() { }
    }
#endif
}
