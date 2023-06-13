using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.ResourceTypes
{
    [Include("Urho3D/Resource/Resource.h")]
    [Abstract]
    public class Resource : ClassObject
    {
        [PropertyMap("GetName", "SetName")]
        public string Name { get; set; } = string.Empty;

        public Resource() : base(typeof(Resource)) { }
        public Resource(Type type) : base(type) { }
    }
}
