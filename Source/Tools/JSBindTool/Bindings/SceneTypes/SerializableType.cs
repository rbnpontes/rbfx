using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.SceneTypes
{
    [Abstract]
    [ClassName("Serializable")]
    [Include("Urho3D/Scene/Serializable.h")]
    public class SerializableType : EngineObject
    {
    }
}
