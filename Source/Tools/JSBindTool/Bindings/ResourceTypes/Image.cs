using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.ResourceTypes
{
    [Include("Urho3D/Resource/Image.h")]
    public class Image : Resource
    {
        public Image() : base(typeof(Image)) { }
    }
}
