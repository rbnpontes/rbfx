using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.SceneTypes
{
    [Abstract]
    [TypeName("Component")]
    [Include("Urho3D/Scene/Component.h")]
    public class Component : SerializableType
    {
        [PropertyMap("GetID")]
        public uint Id { get; set; }
        [PropertyMap("IsEnabled", "SetEnabled")]
        public bool Enabled { get; set; }
        [PropertyMap("IsEnabledEffective")]
        public bool IsEnabledEffective { get; set; }

        [Method("GetFullNameDebug")]
        public string GetFullNameDebug() { return string.Empty; }
        [Method("GetComponent")]
        public Component GetComponent(StringHash type) { return new Component(); }
        [Method("GetIndexInParent")]
        public uint GetIndexInParent() { return 0u; }

        [Method("Remove")]
        public void Remove() { }

        public Component() : base(typeof(Component))
        {
        }
    }
}
