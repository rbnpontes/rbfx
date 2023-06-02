using JSBindTool.Bindings.MathTypes;
using JSBindTool.Bindings.SceneTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/UIElement.h")]
    public class UIElement : Animatable
    {
        public bool IsEnabled { get; set; }
        [PropertyMap("GetName", "SetName")]
        public string Name { get; set; } = string.Empty;
        [PropertyMap("GetChildren")]
        public IList<SharedPtr<UIElement>> Children { get; set; } = new List<SharedPtr<UIElement>>();
        [PropertyMap("GetHorizontalAlignment", "SetHorizontalAlignment")]
        public HorizontalAlignment HorizontalAlignment { get; set; }
        [PropertyMap(null, "SetColor")]
        public Color Color { get; set; }

        public void AddChild(UIElement arg) { }
        public void GetColor(Corner corner) { }
        public void SetColor(Corner corner, Color color) { }
    }
}
