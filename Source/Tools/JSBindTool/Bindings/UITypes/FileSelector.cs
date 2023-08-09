using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/FileSelector.h")]
    public class FileSelector : EngineObject
    {
        public FileSelector() : base(typeof(FileSelector)) { }
    }   
}
