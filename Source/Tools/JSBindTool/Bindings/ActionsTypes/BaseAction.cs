using JSBindTool.Bindings.SceneTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.ActionsTypes
{
#if URHO3D_ACTIONS
    [Include("Urho3D/Actions/BaseAction.h")]
    [Namespace("Actions")]
    [Define("#define Actions_BaseAction Urho3D::Actions::BaseAction")]
    [TypeName("Actions_BaseAction", "BaseAction")]
    public class BaseAction : SerializableType
    {
        public BaseAction() : base(typeof(BaseAction)) { }
        public BaseAction(Type type) : base(type) { }

        [Method]
        public BaseAction GetOrDefault(BaseAction action) => new BaseAction();
    }
#endif
}
