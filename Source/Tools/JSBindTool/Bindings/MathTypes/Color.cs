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
    [Define("#define ColorChannelMask Color::ChannelMask")]
    public class ColorChannelMask : PrimitiveObject
    {
        [Variable("r_")]
        public uint R;
        [Variable("b_")]
        public uint B;
        [Variable("g_")]
        public uint G;
        [Variable("a_")]
        public uint A;

        public ColorChannelMask() : base(typeof(ColorChannelMask))
        {
        }
    }
    [Include("Urho3D/Math/Color.h")]
    public class Color : PrimitiveObject
    {
        #region Variables
        [Variable("r_")]
        public float R;
        [Variable("g_")]
        public float G;
        [Variable("b_")]
        public float B;
        [Variable("a_")]
        public float A;
        #endregion
        #region Properties
        [PropertyMap("SumRGB")]
        public float SumRGB { get => 0f; }
        [PropertyMap("Average")]
        public float Average { get => 0f; }
        [PropertyMap("Luma")]
        public float Luma { get => 0f; }
        [PropertyMap("Chroma")]
        public float Chroma { get => 0f; }
        [PropertyMap("Hue")]
        public float Hue { get => 0f; }
        [PropertyMap("SaturationHSL")]
        public float SaturationHSL { get => 0f; }
        [PropertyMap("SaturationHSV")]
        public float SaturationHSV { get => 0f; }
        [PropertyMap("Value")]
        public float Value { get => 0f; }
        [PropertyMap("GammaToLinear")]
        public Color GammaToLinear { get => new Color(); }
        [PropertyMap("LinearToGamma")]
        public Color LinearToGamma { get => new Color(); }
        [PropertyMap("Lightness")]
        public float Lightness { get => 0f; }
        [PropertyMap("MaxRGB")]
        public float MaxRGB { get => 0f; }
        [PropertyMap("MinRGB")]
        public float MinRGB { get => 0f; }
        [PropertyMap("Range")]
        public float Range { get => 0f; }
        #endregion
        public Color() : base(typeof(Color)) { }
        #region Operators
        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(Color color) => false;
        [OperatorMethod(OperatorType.Equal)]
        [CustomCode(typeof(bool), new Type[] { typeof(Color), typeof(float) })]
        public void EqualOperator(CodeBuilder code)
        {
            MathCodeUtils.EmitEqualOperator(code);
        }
        [OperatorMethod(OperatorType.Mul)]
        public Color MulOperator(Color color) => new Color();
        [OperatorMethod(OperatorType.Mul)]
        public Color MulOperator(float value) => new Color();
        [OperatorMethod(OperatorType.Add)]
        public Color AddOperator(Color color) => new Color();
        [OperatorMethod(OperatorType.Sub)]
        public Color SubOperator(Color color) => new Color();
        #endregion
        #region Methods
        [Method("Data")]
        [CustomCode(typeof(Vector<float>))]
        public void GetData(CodeBuilder code)
        {
            MathCodeUtils.EmitGetData(code, 4);
        }
        
        [Method("ToUInt")]
        public uint ToUInt() { return 0u; }
        [Method("ToUIntMask")]
        public uint ToUIntMask(ColorChannelMask mask) => 0;
        [Method("ToHSL")]
        public Vector3 ToHSL() => new Vector3();
        [Method("ToHSV")]
        public Vector3 ToHSV() => new Vector3();
        [Method("FromUInt")]
        public void FromUInt(uint color) { }
        [Method("FromUIntMask")]
        public void FromUIntMask(uint color, ColorChannelMask mask) { }
        [Method("FromHSL")]
        public void FromHSL(float arg0, float arg1, float arg2, float arg3) { }
        [Method("FromHSV")]
        public void FromHSV(float arg0, float arg1, float arg2, float arg3) { }

        [Method("ToVector3")]
        public Vector3 ToVector3() => new Vector3();
        [Method("ToVector4")]
        public Vector4 ToVector4() => new Vector4();

        [Method("Clip")]
        public void Clip() { }
        [Method("Clip")]
        public void Clip(bool clipAlpha) { }
        [Method("Invert")]
        public void Invert() { }
        [Method("Invert")]
        public void Invert(bool invertAlpha) { }
        [Method("Lerp")]
        public Color Lerp(Color rhs, float t) => new Color();
        [Method("Abs")]
        public Color Abs() => new Color();
        [Method("ToString")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() => string.Empty;
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        [Method("ToUIntArgb")]
        public uint ToUIntArgb() => 0;
        [Method("ToHash")]
        public uint ToHash() => 0;
        #endregion
        #region Static Fields
        [CustomField("abgr")]
        public static void ABGRField(CodeBuilder code)
        {
            code.Add("jsbind_color_channel_mask_push(ctx, Color::ABGR);");
        }
        [CustomField("argb")]
        public static void ARGBField(CodeBuilder code)
        {
            code.Add("jsbind_color_channel_mask_push(ctx, Color::ARGB);");
        }
        [CustomField("rgb")]
        public static void RGBField(CodeBuilder code)
        {
            code.Add("jsbind_color_channel_mask_push(ctx, Color::RGB);");
        }

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
        #endregion
        #region Static Methods
        [Method("ConvertGammaToLinear")]
        public static float ConvertGammaToLinear(float value) => 0f;
        [Method("ConvertLinearToGamma")]
        public static float ConvertLinearToGamma(float value) => 0f;
        #endregion
    }
}
