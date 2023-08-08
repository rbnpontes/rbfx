using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Bindings.ResourceTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    class CursorShapeInfoPropResolver : IPropertyResolver
    {
        public void EmitGetter(string propName, CodeBuilder code)
        {
            code.Add("void* result = instance->osCursor_;");
        }

        public void EmitSetter(string propName, CodeBuilder code)
        {
            code.Add("instance->osCursor_ = static_cast<SDL_Cursor*>(arg0);");
        }

        public bool HasGetter(string propName) => true;

        public bool HasSetter(string propName) => true;
    }
    [Include("Urho3D/UI/Cursor.h")]
    public class CursorShapeInfo : PrimitiveObject
    {
        [Variable("image_")]
        public SharedPtr<Image> Image = new SharedPtr<Image>();
        [Variable("texture_")]
        public SharedPtr<Texture> Texture = new SharedPtr<Texture>();
        [Variable("imageRect_")]
        public IntRect ImageRect = new IntRect();
        [Variable("hotSpot_")]
        public IntVector2 HotSpot = new IntVector2();
        [Variable("systemDefined_")]
        public bool SystemDefined;
        [Variable("systemCursor_")]
        public int SystemCursor;
        [PropertyResolver(typeof(CursorShapeInfoPropResolver))]
        public IntPtr OSCursor { get; set; }
        public CursorShapeInfo() : base(typeof(CursorShapeInfo)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(int systemCursor) { }
    }
    [Include("Urho3D/UI/Cursor.h")]
    public class Cursor : BorderImage
    {
        [PropertyMap("GetShape", "SetShape")]
        public string Shape { get; set; } = string.Empty;
        [PropertyMap("GetUseSystemShapes", "SetUseSystemShapes")]
        public bool UseSystemShapes { get; set; }
        [PropertyMap("GetShapesAttr", "SetShapesAttr")]
        public Vector<Variant> ShapesAttr { get => new Vector<Variant>(); set { } }
        public Cursor() : base(typeof(Cursor)) { }

        [Method]
        public void DefineShape(string shape, Image image, IntRect imageRect, IntVector2 hotSpot) { }
        [Method]
        public void DefineShape(CursorShape shape, Image image, IntRect imageRect, IntVector2 hotSpot) { }
        [Method]
        public void SetShape(CursorShape shape) { }
        [Method]
        public void ApplyOSCursorShape() { }
    }
}
