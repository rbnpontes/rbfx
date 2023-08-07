using JSBindTool.Bindings.SceneTypes;
using JSBindTool.Core.Annotations;
using System.Xml.Linq;

namespace JSBindTool.Bindings.GraphicsTypes
{
    [Include("Urho3D/Scene/Node.h")]
    public class Node : SerializableType
    {
        public Node() : base(typeof(Node)) { }
        public Node(Type type) : base(type) { }
    }
}
