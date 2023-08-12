using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/FontFaceBitmap.h")]
    public class FontFaceBitmap : FontFace
    {
        public FontFaceBitmap() : base(typeof(FontFaceBitmap)) { }
        [Constructor]
        public void Constructor(Font font) { }
        [Method]
        [CustomCode(typeof(bool), new Type[] { typeof(JSBuffer), typeof(float) })]
        public void Load(CodeBuilder code)
        {
            EmitLoad(code);
        }
        [Method]
        public void Load(FontFace fontFace, bool usedGlyphys) { }
    }
}
