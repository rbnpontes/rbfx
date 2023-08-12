using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.ActionsTypes
{
    [Include("Urho3D/Actions/FiniteTimeAction.h")]
    [Define("#define Actions_FiniteTimeAction Actions::FiniteTimeAction")]
    [Namespace("Actions")]
    [TypeName("Actions_FiniteTimeAction", "FiniteTimeAction")]
    public class FiniteTimeAction : BaseAction
    {
        [PropertyMap("GetDuration", "SetDuration")]
        public float Duration { get; set; }

        public FiniteTimeAction() : base(typeof(FiniteTimeAction)) { }

        [Method]
        public FiniteTimeAction GetOrDefault(FiniteTimeAction action) => new FiniteTimeAction();
        [Method]
        public SharedPtr<FiniteTimeAction> Reverse() => new SharedPtr<FiniteTimeAction>();
    }
}
