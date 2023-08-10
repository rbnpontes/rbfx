using JSBindTool.Bindings.MathTypes;
using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/Font.h")]
    public class Font : Resource
    {
        [PropertyMap("GetAbsoluteGlyphOffset", "SetAbsoluteGlyphOffset")]
        public IntVector2 AbsoluteGlyphOffset { get => new IntVector2(); set { } }
        [PropertyMap("GetScaledGlyphOffset", "SetScaledGlyphOffset")]
        public Vector2 ScaledGlyphOffset { get => new Vector2(); set { } }
        [PropertyMap("GetFontType")]
        public FontType FontType { get; }
        [PropertyMap("IsSDFFont")]
        public bool IsSDFFont { get; }

        public Font() : base(typeof(Font)) { }
        public Font(Type type) : base(type) { }

        [Method]
        public FontFace GetFace(float pointSize) => new FontFace();
        [Method]
        public IntVector2 GetTotalGlyphOffset(float pointSize) => new IntVector2();
        [Method]
        public void ReleaseFaces() { }
    }
}
