using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/FontFaceFreeType.h")]
    public class FontFaceFreeType : FontFace
    {
        public FontFaceFreeType() : base(typeof(FontFaceFreeType)) { }

        [Constructor]
        public void Constructor(Font font) { }
        [Method]
        [CustomCode(typeof(bool), new Type[] { typeof(JSBuffer), typeof(float) })]
        public void Load(CodeBuilder code)
        {
            EmitLoad(code);
        }
    }   
}
