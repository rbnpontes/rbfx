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
    public class Resource
    {
        [PropertyMap("GetName", "SetName")]
        public string Name { get; set; } = string.Empty;
    }
}
