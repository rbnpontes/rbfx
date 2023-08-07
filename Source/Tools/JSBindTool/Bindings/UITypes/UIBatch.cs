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
        public UIElement Element = new UIElement();
        public BlendMode BlendMode;
        public IntRect Scissor = new IntRect();
        public Texture Texture = new Texture();
        public Vector2 InvTextureSize = new Vector2();
        public uint VertexStart = 0;
        public uint VertexEnd = 0;
        public uint Color = 0;
        public bool UseGradient = false;

        public UIBatch() : base(typeof(UIBatch)) { }

        [Constructor]
        public void Constructor() { }

        public void SetColor(Color color) { }
        public void SetColor(Color color, bool overrideAlpha) { }
        public void SetDefaultColor() { }
        public void AddQuad(float x, float y, float width, float height, int texOffsetX, int texOffsetY) { }
        public void AddQuad(float x, float y, float width, float height, int texOffsetX, int texOffsetY, int texWidth) { }
        public void AddQuad(float x, float y, float width, float height, int texOffsetX, int texOffsetY, int texWidth, int texHeight) { }
        public void AddQuad(Matrix3x4 transform, int x, int y, int width, int height, int texOffsetX, int texOffsetY) { }
        public void AddQuad(Matrix3x4 transform, int x, int y, int width, int height, int texOffsetX, int texOffsetY,
            int texWidth) { }
        public void AddQuad(Matrix3x4 transform, int x, int y, int width, int height, int texOffsetX, int texOffsetY,
            int texWidth, int texHeight)
        { }
        public void AddQuad(int x, int y, int width, int height, int texOffsetX, int texOffsetY, int texWidth, int texHeight, bool tiled) { }
        public void AddQuad(Matrix3x4 transform, IntVector2 a, IntVector2 b, IntVector2 c,
            IntVector2 d, IntVector2 texA, IntVector2 texB, IntVector2 texC, IntVector2 texD) { }
        public void AddQuad(Matrix3x4 transform, IntVector2 a, IntVector2 b, IntVector2 c,
            IntVector2 texA, IntVector2 texB, IntVector2 texC, IntVector2 texD,
            Color colorA, Color colorB, Color colorC, Color colorD)
        { }
        public bool Merge(UIBatch batch) => false;
        public uint GetInterpolatedColor(float x, float y) => 0;


        public static void AddOrMerge(UIBatch batch, Vector<UIBatch> batches) { }
    }
}
