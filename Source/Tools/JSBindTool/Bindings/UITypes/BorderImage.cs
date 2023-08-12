using JSBindTool.Bindings.CoreTypes;
using JSBindTool.Bindings.GraphicsTypes;
using JSBindTool.Bindings.MathTypes;
using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.UITypes
{
    [Include("Urho3D/UI/Cursor.h")]
    [Abstract]
    [Dependencies(new Type[] { typeof(UIBatch) })]
    public class BorderImage : UIElement
    {
        [PropertyMap("GetTexture", "SetTexture")]
        public Texture Texture { get => new Texture(); set { } }
        [PropertyMap("GetImageRect", "SetImageRect")]
        public IntRect ImageRect { get => new IntRect(); set { } }
        [PropertyMap("GetBorder", "SetBorder")]
        public IntRect Border { get => new IntRect(); set { } }
        [PropertyMap("GetImageBorder", "SetImageBorder")]
        public IntRect ImageBorder { get => new IntRect(); set { } }
        [PropertyMap("GetHoverOffset", "SetHoverOffset")]
        public IntVector2 HoverOffset { get => new IntVector2(); set { } }
        [PropertyMap("GetDisabledOffset", "SetDisabledOffset")]
        public IntVector2 DisabledOffset { get => new IntVector2(); set { } }
        [PropertyMap("GetBlendMode", "SetBlendMode")]
        public BlendMode BlendMode { get; set; }
        [PropertyMap("IsTiled", "SetTiled")]
        public bool IsTiled { get; set; }
        [PropertyMap("GetMaterial", "SetMaterial")]
        public Material Material { get => new Material(); set { } }
        [PropertyMap("GetTextureAttr", "SetTextureAttr")]
        public ResourceRef TextureAttr { get => new ResourceRef(); set { } }
        [PropertyMap("GetMaterialAttr", "SetMaterialAttr")]
        public ResourceRef MaterialAttr { get => new ResourceRef(); set { } }

        public BorderImage() : base(typeof(BorderImage)) { }
        public BorderImage(Type type) : base(type)
        {
            ValidateInheritance<BorderImage>();
        }

        [Method]
        public void SetFullImageRect() { }
        [Method]
        public void SetHoverOffset(int x, int y) { }
        [Method]
        public void SetDisabledOffset(int x, int y) { }
    }
}
