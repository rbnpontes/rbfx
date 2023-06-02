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

        public bool SetFont(Font font, float fontSize)
        {
            return true;
        }
    }
}
