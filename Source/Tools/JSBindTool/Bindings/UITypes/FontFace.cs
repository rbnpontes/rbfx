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
        public FontFace() : base(typeof(FontFace)) { }

        [Constructor]
        public void Constructor() { }
    }

    [Include("Urho3D/UI/FontFace.h")]
    public class FontFace : ClassObject
    {
        public FontFace() : base(typeof(FontFace)) { }

        [Constructor]
        public void Constructor(Font font) { }

        [Method]
        [CustomCode(typeof(bool), new Type[] { typeof(JSBuffer) })]
        public void Load(CodeBuilder code)
        {
            code
                .Add("instance->Load(arg0, arg0_length);");
        }
        [Method("Load")]
        [CustomCode(typeof(bool), new Type[] { typeof(JSBuffer), typeof(float) })]
        public void Load1(CodeBuilder code)
        {
            code.Add("instance->Load(arg0, arg0_length, arg1);");
        }
    }
}
