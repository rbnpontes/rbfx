using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.SceneTypes
{
    [Abstract]
    [TypeName("Serializable")]
    [Include("Urho3D/Scene/Serializable.h")]
    public class SerializableType : EngineObject
    {
        public SerializableType() : base(typeof(SerializableType)) { }
        public SerializableType(Type type) : base(type) { }
    }
}
