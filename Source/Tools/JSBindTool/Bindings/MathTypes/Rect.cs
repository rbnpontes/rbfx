using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Rect.h")]
    public class Rect : PrimitiveObject
    {
        [Variable("min_")]
        public Vector2 Min = new Vector2();
        [Variable("max_")]
        public Vector2 Max = new Vector2();
        [PropertyMap("Defined")]
        public bool Defined { get => false; }
        [PropertyMap("Center")]
        public Vector2 Center { get => new Vector2(); }
        [PropertyMap("Size")]
        public Vector2 Size { get => new Vector2(); }
        [PropertyMap("HalfSize")]
        public Vector2 HalfSize { get => new Vector2(); }
        [PropertyMap("Left")]
        public float Left { get => 0f; }
        [PropertyMap("Top")]
        public float Top { get => 0f; }
        [PropertyMap("Right")]
        public float Right { get => 0f; }
        [PropertyMap("Bottom")]
        public float Bottom { get => 0f; }
        public Rect() : base(typeof(Rect))
        {
        }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(Vector2 min, Vector2 max) { }
        [Constructor]
        public void Constructor(float left, float top, float right, float bottom) { }
        [Constructor]
        public void Constructor(Vector4 vector) { }
        [Constructor]
        public void Constructor(Rect rect) { }

        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(Rect rect) => false;
        [OperatorMethod(OperatorType.Add)]
        public Rect AddOperator(Rect rect) => new Rect();
        [OperatorMethod(OperatorType.Sub)]
        public Rect SubOperator(Rect rect) => new Rect();
        [OperatorMethod(OperatorType.Mul)]
        public Rect MulOperator(float value) => new Rect();
        [OperatorMethod(OperatorType.Div)]
        public Rect DivOperator(float value) => new Rect();

        public void Define(Rect rect) { }
        public void Define(Vector2 min, Vector2 max) { }
        public void Define(Vector2 point) { }
        public void Merge(Vector2 point) { }
        public void Merge(Rect rect) { }
        public void Clear() { }
        public void Clip(Rect rect) { }
        public Intersection IsInside(Vector2 point) => Intersection.Outside;
        public Intersection IsInside(Rect rect) => Intersection.Outside;
        [Method("Data")]
        [CustomCode(typeof(Vector<float>))]
        public void Data(CodeBuilder code)
        {
            MathCodeUtils.EmitGetData(code, 4);
        }
        [Method]
        public Vector4 ToVector4() => new Vector4();
        [Method]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() => string.Empty;
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword

        [Field("full", "FULL")]
        public static Rect Full = new Rect();
        [Field("positive", "POSITIVE")]
        public static Rect Positive = new Rect();
        [Field("zero", "ZERO")]
        public static Rect Zero = new Rect();
    }
    [Include("Urho3D/Math/Rect.h")]
    public class IntRect : PrimitiveObject
    {
        [Variable("left_")]
        public int Left = 0;
        [Variable("top_")]
        public int Top = 0;
        [Variable("right_")]
        public int Right = 0;
        [Variable("bottom_")]
        public int Bottom = 0;

        [PropertyMap("Size")]
        public IntVector2 Size { get => new IntVector2(); }
        [PropertyMap("Width")]
        public int Width { get => 0; }
        [PropertyMap("Height")]
        public int Height { get => 0; }
        [PropertyMap("Min")]
        public IntVector2 Min { get => new IntVector2(); }
        [PropertyMap("Max")]
        public IntVector2 Max { get => new IntVector2(); }

        public IntRect() : base(typeof(IntRect)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(IntVector2 min, IntVector2 max) { }
        [Constructor]
        public void Constructor(int left, int top, int right, int bottom) { }

        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(IntRect rect) => false;
        [OperatorMethod(OperatorType.Add)]
        public IntRect AddOperator(IntRect rect) => new IntRect();
        [OperatorMethod(OperatorType.Sub)]
        public IntRect SubOperator(IntRect rect) => new IntRect();
        [OperatorMethod(OperatorType.Mul)]
        public IntRect MulOperator(float value) => new IntRect();
        [OperatorMethod(OperatorType.Div)]
        public IntRect DivOperator(float value) => new IntRect();

        [Method]
        public Intersection IsInside(IntVector2 point) => Intersection.Outside;
        [Method]
        public Intersection IsInside(IntRect rect) => Intersection.Outside;
        [Method]
        public void Clip(IntRect rect) { }
        [Method]
        public void Merge(IntRect rect) { }
        [Method]
        [CustomCode(typeof(Vector<int>))]
        public void GetData(CodeBuilder code)
        {
            MathCodeUtils.EmitGetData(code, Marshal.SizeOf<Int32>(), "int");
        }
        [Method]
        public bool Contains(IntVector2 point) => false;
        [Method]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() => string.Empty;
        [Method]
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        public uint ToHash() => 0u;

        [Field("zero", "ZERO")]
        public static IntRect Zero = new IntRect();
    }
}
