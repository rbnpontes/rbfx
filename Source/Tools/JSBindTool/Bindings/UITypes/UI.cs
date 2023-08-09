using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/UI.h")]
    public class UI : EngineObject
    {
        [PropertyMap("GetRoot")]
        public UIElement Root { get; } = new UIElement();
        public UI() : base(typeof(UI)) { }
        public UI(Type type) : base(type) { }
    }

    [Include("Urho3D/UI/UI.h")]
    [Include("Urho3D/UI/Font.h")]
    [Namespace(Constants.ProjectName + ".UI")]
    public class UIModule : ModuleObject
    {
        public UIModule() : base(typeof(UIModule)) { }

        [Field("fontTextureMinSize", "FONT_TEXTURE_MIN_SIZE")]
        public static int FontTextureMinSize = 0;
        [Field("fontDpi", "FONT_DPI")]
        public static int FontDpi = 0;
    }
}
