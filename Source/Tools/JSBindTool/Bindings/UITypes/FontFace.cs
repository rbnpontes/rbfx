using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/FontFace.h")]
    public class FontGlyph : PrimitiveObject
    {
        [Variable("x_")]
        public short X;
        [Variable("y_")]
        public short Y;
        [Variable("texWidth_")]
        public short TexWidth;
        [Variable("texHeight_")]
        public short TexHeight;
        [Variable("width_")]
        public float Width;
        [Variable("height_")]
        public float Height;
        [Variable("offsetX_")]
        public float OffsetX;
        [Variable("offsetY_")]
        public float OffsetY;
        [Variable("advanceX_")]
        public float AdvanceX;
        [Variable("page_")]
        public float Page;
        [Variable("used_")]
        public bool Used;

        public FontGlyph() : base(typeof(FontGlyph)) { }

        [Constructor]
        public void Constructor() { }
    }

    [Include("Urho3D/UI/FontFace.h")]
    [Dependencies(new Type[] { typeof(FontGlyph) })]
    [Abstract]
    public class FontFace : ClassObject
    {
        [PropertyMap("HasMutableGlyphs")]
        public bool HasMutableGlyphs { get; }
        [PropertyMap("IsDataLost")]
        public bool IsDataLost { get; }
        [PropertyMap("GetPointSize")]
        public float PointSize { get; }
        [PropertyMap("GetRowHeight")]
        public float RowHeight { get; }
        [PropertyMap("GetTextures")]
        public Vector<SharedPtr<Texture2D>> Textures { get => new Vector<SharedPtr<Texture2D>>(); }

        public FontFace() : base(typeof(FontFace)) { }

        [Method]
        [CustomCode(typeof(bool), new Type[] { typeof(JSBuffer), typeof(float) })]
        public void Load(CodeBuilder code)
        {
            code.Add("bool result = instance->Load(arg0, (unsigned)arg0_length, arg1);");
        }
        [Method]
        [CustomCode(new Type[] { typeof(uint) })]
        public void GetGlyph(CodeBuilder code)
        {
            code
                .Add("const FontGlyph* glyph = instance->GetGlyph(arg0);")
                .Add($"{CodeUtils.GetPushSignature(typeof(FontGlyph))}(ctx, *glyph);")
                .Add("result_code = 1;");
        }

        public float GetKerning(uint c, uint d) => 0f;
    }
}
