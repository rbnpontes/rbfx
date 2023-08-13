using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/Text.h")]
    public class Text : UISelectable
    {
        [PropertyMap("GetText", "SetText")]
        [PropertyName("Text")]
        public string _Text { get; set; } = string.Empty;

        [PropertyMap("GetFont", "SetFont")]
        public Font Font { get; set; } = new Font();
        [PropertyMap("GetFontSize", "SetFontSize")]
        public float FontSize { get; set; }
        [PropertyMap("GetHorizontalAlignment", "SetHorizontalAlignment")]
        public HorizontalAlignment TextAlignment { get; set; }
        [PropertyMap("GetRowSpacing", "SetRowSpacing")]
        public float RowSpacing { get; set; }
        [PropertyMap("GetWordwrap", "SetWordwrap")]
        public bool Wordwrap { get; set; }
        [PropertyMap("GetAutoLocalizable", "SetAutoLocalizable")]
        public bool AutoLocalizable { get; set; }
        [PropertyMap("GetTextEffect", "SetTextEffect")]
        public TextEffect TextEffect { get; set; }
        [PropertyMap("GetEffectShadowOffset", "SetEffectShadowOffset")]
        public IntVector2 EffectShadowOffset { get => new IntVector2(); set { } }
        [PropertyMap("GetEffectStrokeThickness", "SetEffectStrokeThickness")]
        public int EffectStrokeThickness { get; set; }
        [PropertyMap("GetEffectRoundStroke", "SetEffectRoundStroke")]
        public bool EffectRoundStroke { get; set; }
        [PropertyMap("GetEffectColor", "SetEffectColor")]
        public Color EffectColor { get => new Color(); set { } }
        [PropertyMap("GetSelectionStart")]
        public uint SelectionStart { get; }
        [PropertyMap("GetSelectionLength")]
        public uint SelectionLength { get; }
        [PropertyMap("GetRowHeight")]
        public uint RowHeight { get; }
        [PropertyMap("GetNumRows")]
        public uint NumRows { get; }
        [PropertyMap("GetNumChars")]
        public uint NumChars { get; }
        [PropertyMap("GetEffectDepthBias")]
        public float EffectDepthBias { get; set; }
        [PropertyMap("GetFontAttr", "SetFontAttr")]
        public ResourceRef FontAttr { get => new ResourceRef(); set { } }
        [PropertyMap("GetTextAttr", "SetTextAttr")]
        public string TextAttr { get; set; } = string.Empty;

        public Text() : base(typeof(Text)) { }
        public Text(Type type) : base(type) { }

        [Method]
        public bool SetFont(Font font, float fontSize) => false;
        [Method]
        public void SetSelection(uint start) { }
        [Method]
        public void SetSelection(uint start, uint length) { }
        [Method]
        public void ClearSelection() { }
        [Method]
        public float GetRowWidth(uint index) => 0f;
        [Method]
        public Vector2 GetCharPosition(uint index) => new Vector2();
        [Method]
        public Vector2 GetCharSize(uint index) => new Vector2();

    }
}
