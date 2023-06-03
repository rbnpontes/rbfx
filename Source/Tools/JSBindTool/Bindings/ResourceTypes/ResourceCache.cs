using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.ResourceTypes
{
    [Include("Urho3D/Resource/ResourceCache.h")]
    public class ResourceCache : EngineObject
    {
        public ResourceCache() : base(typeof(ResourceCache))
        {
        }

        public Resource GetResource(StringHash type, string resourcePath)
        {
            return new Resource();
        }
    }
}
