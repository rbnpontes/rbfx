using JSBindTool.Bindings.ActionsTypes;
using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Bindings.UITypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.EngineTypes
{
    [Include("Urho3D/Engine/StateManager.h")]
    public class ApplicationState : EngineObject
    {
        [PropertyMap("CanLeaveState")]
        public bool CanLeaveState { get; }
        [PropertyMap("IsMouseVisible", "SetMouseVisible")]
        public bool IsMouseVisible { get; set; }
        [PropertyMap("IsMouseGrabbed", "SetMouseGrabbed")]
        public bool IsMouseGrabbed { get; set; }
        [PropertyMap("GetMouseMode", "SetMouseMode")]
        public MouseMode MouseMode { get; set; }
        [PropertyMap("GetUIRoot")]
        public UIElement UIRoot { get => new UIElement(); }
        [PropertyMap("GetUICustomSize", "SetUICustomSize")]
        public IntVector2 UICustomSize { get => new IntVector2(); set { } }
        [PropertyMap("GetDefaultFogColor", "SetDefaultFogColor")]
        public Color DefaultFogColor { get => new Color(); set { } }
#if URHO3D_ACTIONS
        [PropertyMap("GetActionManager")]
        public ActionManager ActionManager { get => new ActionManager(); }
#endif
        public ApplicationState() : base(typeof(ApplicationState)) { }
        public ApplicationState(Type type) : base(type)
        {
            ValidateInheritance<ApplicationState>();
        }

        [Method]
        public void Activate(StringVariantMap bundle) { }
        [Method]
        public void TransitionComplete() { }
        [Method]
        public void TransitionStarted() { }
        [Method]
        public void Deactivate() { }
        [Method]
        public void Update(float timeStep) { }
        [Method]
        public void SetUICustomSize(int width, int height) { }
        [Method]
        public void SetNumViewports(uint num) { }
        [Method]
        public void SetViewport(uint index, Viewport viewport) { }
        [Method]
        public Viewport GetViewport(uint index) => new Viewport();
        [Method]
        public Viewport GetViewportForScene(Scene scene, uint index) => new Viewport();
#if URHO3D_ACTIONS
        [Method]
        public ActionState AddAction(BaseAction action, EngineObjectRef target) => new ActionState();
        [Method]
        public ActionState AddAction(BaseAction action, EngineObjectRef target, bool paused) => new ActionState();
#endif

    }
    [Include("Urho3D/Engine/StateManager.h")]
    public class StateManager : EngineObject
    {
        [PropertyMap("GetState")]
        public ApplicationState State { get => new ApplicationState(); }
        [PropertyMap("GetTargetState")]
        public StringHash TargetState { get => new StringHash(); }
        [PropertyMap("GetFadeInDuration", "SetFadeInDuration")]
        public float FadeInDuration { get; set; }
        [PropertyMap("GetFadeOutDuration", "SetFadeOutDuration")]
        public float FadeOutDuration { get; set; }

        public StateManager() : base(typeof(StateManager)) { }

        public void Reset() { }
        public void Update(float timeStep) { }
        public void EnqueueState(ApplicationState gameScreen) { }
        public void EnqueueState(ApplicationState gameScreen, StringVariantMap bundle) { }
        public void EnqueueState(StringHash type) { }
        public void EnqueueState(StringHash type, StringVariantMap bundle) { }
    }
}
