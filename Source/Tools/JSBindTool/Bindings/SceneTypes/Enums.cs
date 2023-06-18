using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.SceneTypes
{
    [Include("Urho3D/Scene/Component.h")]
    public enum AutoRemoveMode
    {
        Disabled =0,
        Component,
        Node
    }
}
