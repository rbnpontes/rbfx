using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.GraphicsTypes
{
    [Include("Urho3D/Scene/Scene.h")]
    public class Scene : Node
    {
        public Scene() : base(typeof(Scene)) { }
    }
}
