using JSBindTool.Bindings.SceneTypes;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.GraphicsTypes
{
    [Include("Urho3D/Graphics/Camera.h")]
    public class Camera : Component
    {
        public Camera() : base(typeof(Camera)) { }
    }   
}
