using JSBindTool.Bindings.CoreTypes;
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
    [Include("Urho3D/Resource/Resource.h")]
    [Abstract]
    public class ResourceWithMetadata : Resource
    {
        public ResourceWithMetadata() : base(typeof(ResourceWithMetadata)) { }
        public ResourceWithMetadata(Type type) : base(type) { }

        [Method]
        public void AddMetadata(string name, Variant value) { }
        [Method]
        public void RemoveMetadata(string name) { }
        [Method]
        public void RemoveAllMetadata() { }
        [Method]
        public Vector<string> GetMetadataKeys() => new Vector<string>();
        [Method]
        public Variant GetMetadata(string name) => new Variant();
        [Method]
        public bool HasMetadata() => false;
    }
}
