using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/FileSelector.h")]
    public class FileSelector : ClassObject
    {
        public FileSelector() : base(typeof(FileSelector)) { }
    }   
}
