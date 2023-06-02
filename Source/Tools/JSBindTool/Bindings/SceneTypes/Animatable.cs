using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.SceneTypes
{
    [Abstract]
    [Include("Urho3D/Scene/Animatable")]
    public class Animatable : SerializableType
    {
    }
}
