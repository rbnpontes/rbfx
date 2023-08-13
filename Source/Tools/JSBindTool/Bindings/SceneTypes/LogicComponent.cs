using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.SceneTypes
{
    [Include("Urho3D/Scene/LogicComponent.h")]
    public enum UpdateEvent
    {
        NoEvent = 0x0,
        Update = 0x1,
        PostUpdate = 0x2,
        FixedUpdate = 0x4,
        FixedPostUpdate = 0x8
    }
    [Include("Urho3D/Scene/LogicComponent.h")]
    [Abstract]
    public class LogicComponent : Component
    {
        [PropertyMap("GetUpdateEventMask", "SetUpdateEventMask")]
        public FlagSet<UpdateEvent> UpdateEventMask { get => new FlagSet<UpdateEvent>(); set { } }
        [PropertyMap("IsDelayedStartCalled")]
        public bool IsDelayedStartCalled { get; }

        public LogicComponent() : base(typeof(LogicComponent)) { }
        public LogicComponent(Type type) : base(type)
        {
            ValidateInheritance<LogicComponent>();
        }
    }   
}
