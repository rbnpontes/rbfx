using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.SceneTypes
{
    [Include("Urho3D/Scene/ValueAnimation.h")]
    public class ValueAnimation : Resource
    {
        public ValueAnimation() : base(typeof(ValueAnimation)) { }
    }
}
