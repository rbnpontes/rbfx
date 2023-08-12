using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.ActionsTypes
{
#if URHO3D_ACTIONS
    [Include("Urho3D/Actions/ActionManager.h")]
    public class ActionManager : EngineObject
    {
        [PropertyMap("GetEmptyAction")]
        public FiniteTimeAction EmptyAction { get => new FiniteTimeAction(); }
        public ActionManager() : base(typeof(ActionManager)) { }

        [Method]
        public void CompleteAllActions() { }
        [Method]
        public void CancelAllActions() { }
        [Method]
        public void CompleteAllActionsOnTarget(EngineObjectRef target) { }
        [Method]
        public void CancelAllActionsFromTarget(EngineObjectRef target) { }
        [Method]
        public void CancelAction(ActionState actionState) { }
        [Method]
        public uint GetNumActions(EngineObjectRef target) => 0;
        [Method]
        public ActionState AddAction(BaseAction action, EngineObjectRef target) => new ActionState();
        [Method]
        public ActionState AddAction(BaseAction action, EngineObjectRef target, bool paused) => new ActionState();
        [Method]
        public void Update(float dt) { }
    }
#endif
}
