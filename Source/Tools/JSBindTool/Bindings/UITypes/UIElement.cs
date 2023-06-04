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
        [PropertyMap("IsEnabled", "SetEnabled")]
        public bool IsEnabled { get; set; }
        [PropertyMap("GetName", "SetName")]
        public string Name { get; set; } = string.Empty;
        [PropertyMap("GetChildren")]
        public Vector<SharedPtr<UIElement>> Children { get; set; } = new Vector<SharedPtr<UIElement>>();
        [PropertyMap("GetHorizontalAlignment", "SetHorizontalAlignment")]
        public HorizontalAlignment HorizontalAlignment { get; set; }
        [PropertyMap("GetVerticalAlignment", "SetVerticalAlignment")]
        public VerticalAlignment VerticalAlignment { get; set; }
        [PropertyMap(null, "SetColor")] 
        public Color Color { set { } }

        public UIElement() : base(typeof(UIElement)) { }
        public UIElement(Type type) : base(type) { }

        [Method]
        public void AddChild(UIElement arg) { }
        [Method]
        public void GetColor(Corner corner) { }
        [Method]
        public void SetColor(Corner corner, Color color) { }
    }
}
