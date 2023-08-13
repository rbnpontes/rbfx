using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;
using System.Diagnostics.SymbolStore;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/Text3D.h")]
    public class Text3D : Drawable
    {
        [PropertyMap("GetFont", "SetFont")]
        public Font Font { get => new Font(); set { } }
        [PropertyMap("GetFontSize", "SetFontSize")]
        public float FontSize { get; set; }
        [PropertyMap("GetMaterial", "SetMaterial")]
        public Material Material { get => new Material(); set { } }
        [PropertyMap("GetText", "SetText")]
        public string Text { get; set; } = string.Empty;
        [PropertyMap("GetVerticalAlignment", "SetVerticalAlignment")]
        public VerticalAlignment VerticalAlignment { get; set; }
        [PropertyMap("GetHorizontalAlignment", "SetHorizontalAlignment")]
        public HorizontalAlignment HorizontalAlignment { get; set; }
        [PropertyMap("GetTextAlignment", "SetTextAlignment")]
        public HorizontalAlignment TextAlignment { get; set; }
        [PropertyMap("GetRowSpacing", "SetRowSpacing")]
        public float RowSpacing { get; set; }
        [PropertyMap("GetWordwrap", "SetWordwrap")]
        public bool Wordwrap { get; set; }
        [PropertyMap("GetTextEffect", "SetTextEffect")]
        public TextEffect TextEffect { get; set; }
        [PropertyMap("GetEffectShadowOffset", "SetEffectShadowOffset")]
        public IntVector2 EffectShadowOffset { get => new IntVector2(); set { } }
        [PropertyMap("GetEffectStrokeThickness", "SetEffectStrokeThickness")]
        public int EffectStrokeThickness { get; set; }
        [PropertyMap("GetEffectStrokeThickness", "SetEffectStrokeThickness")]
        public bool EffectRoundStroke { get; set; }
        [PropertyMap("GetEffectColor", "SetEffectColor")]
        public Color EffectColor { get => new Color(); set { } }
        [PropertyMap("GetEffectDepthBias", "SetEffectDepthBias")]
        public float EffectDepthBias { get; set; }
        [PropertyMap("GetWidth", "SetWidth")]
        public int Width { get; set; }
        [PropertyMap("GetHeight")]
        public int Height { get; }
        [PropertyMap("GetOpacity", "SetOpacity")]
        public float Opacity { get; set; }
        [PropertyMap("IsFixedScreenSize", "SetFixedScreenSize")]
        public bool IsFixedScreenSize { get; set; }
        [PropertyMap("GetSnapToPixels", "SetSnapToPixels")]
        public bool SnapToPixels { get; set; }
        [PropertyMap("GetFaceCameraMode", "SetFaceCameraMode")]
        public FaceCameraMode FaceCameraMode { get; set; }
        [PropertyMap("GetRowHeight", "SetRowHeight")]
        public int RowHeight { get; }
        [PropertyMap("GetNumRows")]
        public uint NumRows { get; }
        [PropertyMap("GetNumChars")]
        public uint NumChars { get; }
        [PropertyMap("GetFontAttr", "SetFontAttr")]
        public ResourceRef FontAttr { get => new ResourceRef(); set { } }
        [PropertyMap("GetMaterialAttr", "SetMaterialAttr")]
        public ResourceRef MaterialAttr { get => new ResourceRef(); set { } }
        [PropertyMap("GetTextAttr", "SetTextAttr")]
        public string TextAttr { get; set; } = string.Empty;
        [PropertyMap("GetColorAttr", "SetColor")]
        public Color Color { get => new Color(); set { } }

        public Text3D() : base(typeof(Text3D)) { }

        [Method]
        public void SetColor(Corner corner, Color color) { }
        [Method]
        public int GetRowWidth(uint index) => 0;
        [Method]
        public Vector2 GetCharPosition(uint index) => new Vector2();
        [Method]
        public Vector2 GetCharSize(uint index) => new Vector2();
        [Method]
        public Color GetColor(Corner corner) => new Color();
    }
}
