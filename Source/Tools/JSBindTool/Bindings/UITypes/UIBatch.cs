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
    [Include("Urho3D/UI/UIBatch.h")]
    public class UIBatch : PrimitiveObject
    {
        [Variable("element_")]
        public UIElement Element = new UIElement();
        [Variable("blendMode_")]
        public BlendMode BlendMode;
        [Variable("scissor_")]
        public IntRect Scissor = new IntRect();
        [Variable("texture_")]
        public Texture Texture = new Texture();
        [Variable("invTextureSize_")]
        public Vector2 InvTextureSize = new Vector2();
        [Variable("vertexStart_")]
        public uint VertexStart = 0;
        [Variable("vertexEnd_")]
        public uint VertexEnd = 0;
        [Variable("color_")]
        public uint Color = 0;
        [Variable("useGradient_")]
        public bool UseGradient = false;
        [Variable("customMaterial_")]
        public Material CustomMaterial = new Material();

        public UIBatch() : base(typeof(UIBatch)) { }

        [Constructor]
        public void Constructor() { }

        [Method]
        public void SetColor(Color color) { }
        [Method]
        public void SetColor(Color color, bool overrideAlpha) { }
        [Method]
        public void SetDefaultColor() { }
        [Method]
        public void AddQuad(float x, float y, float width, float height, int texOffsetX, int texOffsetY) { }
        [Method]
        public void AddQuad(float x, float y, float width, float height, int texOffsetX, int texOffsetY, int texWidth) { }
        [Method]
        public void AddQuad(float x, float y, float width, float height, int texOffsetX, int texOffsetY, int texWidth, int texHeight) { }
        [Method]
        public void AddQuad(Matrix3x4 transform, int x, int y, int width, int height, int texOffsetX, int texOffsetY) { }
        [Method]
        public void AddQuad(Matrix3x4 transform, int x, int y, int width, int height, int texOffsetX, int texOffsetY,
            int texWidth) { }
        [Method]
        public void AddQuad(Matrix3x4 transform, int x, int y, int width, int height, int texOffsetX, int texOffsetY,
            int texWidth, int texHeight)
        { }
        [Method]
        public void AddQuad(int x, int y, int width, int height, int texOffsetX, int texOffsetY, int texWidth, int texHeight, bool tiled) { }
        [Method]
        public void AddQuad(Matrix3x4 transform, IntVector2 a, IntVector2 b, IntVector2 c,
            IntVector2 d, IntVector2 texA, IntVector2 texB, IntVector2 texC, IntVector2 texD) { }
        [Method]
        public void AddQuad(Matrix3x4 transform, IntVector2 a, IntVector2 b, IntVector2 c, IntVector2 d,
            IntVector2 texA, IntVector2 texB, IntVector2 texC, IntVector2 texD,
            Color colorA, Color colorB, Color colorC, Color colorD) 
        { }
        [Method]
        public bool Merge(UIBatch batch) => false;
        [Method]
        public uint GetInterpolatedColor(float x, float y) => 0;

        [Method]
        public static void AddOrMerge(UIBatch batch, Vector<UIBatch> batches) { }
    }
}
