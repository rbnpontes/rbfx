using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Bindings.SceneTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/TextRenderer3D.h")]
    public class TextParams3D : PrimitiveObject
    {
        [Variable("text_")]
        public string Text = string.Empty;
        [Variable("font_")]
        public SharedPtr<Font> Font = new SharedPtr<Font>();
        [Variable("fixedScreenSize_")]
        public bool FixedScreenSize;
        [Variable("snapToPixels_")]
        public bool SnapToPixels;
        [Variable("horizontalAlignment_")]
        public HorizontalAlignment HorizontalAlignment;
        [Variable("verticalAlignment_")]
        public VerticalAlignment VerticalAlignment;
        [Variable("textAlignment_")]
        public HorizontalAlignment TextAlignment;
        [Variable("hash_")]
        public uint Hash;

        public TextParams3D() : base(typeof(TextParams3D)) { }

        [Constructor]
        public void Constructor() { }
        [Method]
        public void RecalculateHash() { }
        [Method]
        public uint ToHash() => 0;
        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(TextParams3D value) => false;
    }
    [Include("Urho3D/UI/TextRenderer3D.h")]
    [Include("Urho3D/UI/Font.h")]
    public class TextRenderer3D : LogicComponent
    {
        [PropertyMap("GetDefaultFontSize", "SetDefaultFontSize")]
        public float DefaultFontSize { get; set; }
        [PropertyMap("GetDefaultFontAttr", "SetDefaultFontAttr")]
        public ResourceRef DefaultFontAttr { get => new ResourceRef(); set { } }

        public TextRenderer3D() : base(typeof(TextRenderer3D)) { }
        [Method]
        public void AddText3D(Vector3 position, Quaternion rotation, Color color, TextParams3D textParams) { }
    }
}
