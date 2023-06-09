using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Color.h")]
    [Operator(OperatorFlags.Add | OperatorFlags.Sub | OperatorFlags.Mul | OperatorFlags.Equal)]
    public class Color : PrimitiveObject
    {
        [Variable("r_")]
        public float R;
        [Variable("g_")]
        public float G;
        [Variable("b_")]
        public float B;
        [Variable("a_")]
        public float A;

        public Color() : base(typeof(Color)) { }

        [Method("ToUInt")]
        public uint ToUInt() { return 0u; }
        [Method("FromUInt")]
        public void FromUInt(uint value) { }
        [Method("FromHSL")]
        public void FromHSL(float arg0, float arg1, float arg2, float arg3) { }
        [Method("FromHSV")]
        public void FromHSV(float arg0, float arg1, float arg2, float arg3) { }
        [Method("SumRGB")]
        public float SumRGB() { return 0f; }

        [Field("white", "WHITE")]
        public static Color White = new Color();
        [Field("gray", "GRAY")]
        public static Color Gray = new Color();
        [Field("black", "BLACK")]
        public static Color Black = new Color();
        [Field("red", "RED")]
        public static Color Red = new Color();
        [Field("green", "GREEN")]
        public static Color Green = new Color();
        [Field("blue", "BLUE")]
        public static Color Blue = new Color();
        [Field("cyan", "CYAN")]
        public static Color Cyan = new Color();
        [Field("magenta", "MAGENTA")]
        public static Color Magenta = new Color();
        [Field("yellow", "YELLOW")]
        public static Color TransparentBlack = new Color();
        [Field("luminosityGamma", "LUMINOSITY_GAMMA")]
        public static Color LuminosityGamma = new Color();
        [Field("luminosityLinear", "LUMINOSITY_LINEAR")]
        public static Color LuminosityLinear = new Color();
    }
}
