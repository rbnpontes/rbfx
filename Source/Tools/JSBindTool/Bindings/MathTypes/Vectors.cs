using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Vector2.h")]
    public class IntVector2 : PrimitiveObject
    {
        [Variable("x_")]
        public int X;
        [Variable("y_")]
        public int Y;

        [PropertyMap("Length")]
        public float Length { get => 0f; }

        public IntVector2() : base(typeof(IntVector2)) { }

        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(IntVector2 vec) { return true; }
        [OperatorMethod(OperatorType.Add)]
        public IntVector2 AddOperator(IntVector2 vec) { return new IntVector2(); }
        [OperatorMethod(OperatorType.Sub)]
        public IntVector2 SubOperator(IntVector2 vec) { return new IntVector2(); }
        [OperatorMethod(OperatorType.Mul)]
        public IntVector2 MulOperator(int scalar) { return new IntVector2(); }
        [OperatorMethod(OperatorType.Mul)]
        public IntVector2 MulOperator(IntVector2 scalar) { return new IntVector2(); }
        [OperatorMethod(OperatorType.Div)]
        public IntVector2 DivOperator(int scalar) { return new IntVector2(); }
        [OperatorMethod(OperatorType.Div)]
        public IntVector2 DivOperator(IntVector2 scalar) { return new IntVector2(); }

        [Method("Data")]
        [CustomCode]
        public void Data(CodeBuilder code)
        {
            code
                .Add("result_code = 1;")
                .Add("const int* data = instance.Data();")
                .Add("duk_push_array(ctx);")
                .Add("for(duk_idx_t i = 0; i < 3; ++i)")
                .Scope(loopScope =>
                {
                    loopScope
                        .Add("duk_push_number(ctx, data[i]);")
                        .Add("duk_put_prop_index(ctx, -2, i);");
                });
        }

        [Method("ToString")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() { return string.Empty; }
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        [Method("ToHash")]
        public uint ToHash() { return 0u; }

        [Method("ToVector2")]
        public Vector2 ToVector2() { return new Vector2(); }
        [Method("ToIntVector3")]
        public IntVector3 ToIntVector3() { return new IntVector3(); }
        [Method("ToIntVector3")]
        public IntVector3 ToIntVector3(int z) { return new IntVector3(); }
        [Method("ToVector3")]
        public Vector3 ToVector3() { return new Vector3(); }
        [Method("ToVector3")]
        public Vector3 ToVector3(float z) { return new Vector3(); }
        [Method("ToVector4")]
        public Vector4 ToVector4() { return new Vector4(); }
        [Method("ToVector4")]
        public Vector4 ToVector4(float z) { return new Vector4(); }
        [Method("ToVector4")]
        public Vector4 ToVector4(float z, float w) { return new Vector4(); }

        [Field("zero", "ZERO")]
        public static IntVector2 Zero = new IntVector2();
        [Field("left", "LEFT")]
        public static IntVector2 Left = new IntVector2();
        [Field("right", "RIGHT")]
        public static IntVector2 Right = new IntVector2();
        [Field("up", "UP")]
        public static IntVector2 Up = new IntVector2();
        [Field("down", "DOWN")]
        public static IntVector2 Down = new IntVector2();
        [Field("one", "ONE")]
        public static IntVector2 One = new IntVector2();

        [Method("Min")]
        [CustomCode(typeof(IntVector2), new Type[] { typeof(IntVector2), typeof(IntVector2) })]
        public static void Min(CodeBuilder code)
        {
            code.Add("IntVector2 result = VectorMin(arg0, arg1);");
        }
        [Method("Max")]
        [CustomCode(typeof(IntVector2), new Type[] { typeof(IntVector2), typeof(IntVector2) })]
        public static void Max(CodeBuilder code)
        {
            code.Add("IntVector2 result = VectorMax(arg0, arg1);");
        }
        [Method("Abs")]
        [CustomCode(typeof(IntVector2), new Type[] { typeof(IntVector2) })]
        public static void Abs(CodeBuilder code)
        {
            code.Add("IntVector2 result = VectorAbs(arg0);");
        }
    }
    [Include("Urho3D/Math/Vector2.h")]
    [Include("Urho3D/Math/Vector4.h")]
    public class Vector2 : PrimitiveObject
    {
        [Variable("x_")]
        public float X;
        [Variable("y_")]
        public float Y;

        [PropertyMap("Length")]
        public float Length { get => 0f; }
        [PropertyMap("LengthSquared")]
        public float LengthSquared { get => 0f; }
        [PropertyMap("IsNaN")]
        public bool IsNaN { get => true; }
        [PropertyMap("IsInf")]
        public bool IsInfinite { get => true; }
        [PropertyMap("Normalized")]
        public Vector2 Normalized
        {
            get => new Vector2();
        }
        [PropertyMap("Abs")]
        public Vector2 Abs { get => new Vector2(); }

        public Vector2(): base(typeof(Vector2)) { }

        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(Vector2 vec) { return true; }
        [OperatorMethod(OperatorType.Add)]
        public Vector2 AddOperator(Vector2 vec) { return new Vector2(); }
        [OperatorMethod(OperatorType.Sub)]
        public Vector2 SubOperator(Vector2 vec) { return new Vector2(); }
        [OperatorMethod(OperatorType.Mul)]
        public Vector2 MulOperator(float scalar) { return new Vector2(); }
        [OperatorMethod(OperatorType.Mul)]
        public Vector2 MulOperator(Vector2 scalar) { return new Vector2(); }
        [OperatorMethod(OperatorType.Div)]
        public Vector2 DivOperator(float scalar) { return new Vector2(); }
        [OperatorMethod(OperatorType.Div)]
        public Vector2 DivOperator(Vector2 scalar) { return new Vector2(); }

        [Method("Data")]
        [CustomCode]
        public void Data(CodeBuilder code)
        {
            code
                .Add("result_code = 1;")
                .Add("const float* data = instance.Data();")
                .Add("duk_push_array(ctx);")
                .Add("for(duk_idx_t i = 0; i < 2; ++i)")
                .Scope(loopScope =>
                {
                    loopScope
                        .Add("duk_push_number(ctx, data[i]);")
                        .Add("duk_put_prop_index(ctx, -2, i);");
                });
        }

        [Method("Normalize")]
        public void Normalize() { }
        [Method("DotProduct")]
        public float DotProduct(Vector2 vec) { return 0f; }
        [Method("AbsDotProduct")]
        public float AbsDotProduct(Vector2 vec) { return 0f; }
        [Method("CrossProduct")]
        public float CrossProduct(Vector2 vec) { return 0f; }
        [Method("ProjectOntoAxis")]
        public float ProjectOntoAxis(Vector2 axis) { return 0f; }
        [Method("Angle")]
        float Angle(Vector2 vec) { return 0f; }
        [Method("Lerp")]
        public Vector2 Lerp(Vector2 vec, float t) { return new Vector2(); }

        [Method("Equals")]
        public bool Equals(Vector2 vec) { return true; }
        [Method("Equals")]
        public bool Equals(Vector2 vec, float eps) { return true; }

        [Method("ToIntVector2")]
        public IntVector2 ToIntVector2() { return new IntVector2(); }
        [Method("ToIntVector3")]
        public IntVector3 ToIntVector3() { return new IntVector3(); }
        [Method("ToIntVector3")]
        public IntVector3 ToIntVector3(int z) { return new IntVector3(); }
        [Method("ToVector3")]
        public Vector3 ToVector3() { return new Vector3(); }
        [Method("ToVector3")]
        public Vector3 ToVector3(float z) { return new Vector3(); }
        [Method("ToVector4")]
        public Vector4 ToVector4(float z, float w) { return new Vector4(); }

        [Method("NormalizedOrDefault")]
        public Vector2 NormalizedOrDefault() { return new Vector2(); }
        [Method("NormalizedOrDefault")]
        public Vector2 NormalizedOrDefault(Vector2 defaultValue) { return new Vector2(); }
        [Method("NormalizedOrDefault")]
        public Vector2 NormalizedOrDefault(Vector2 defaultValue, float eps) { return new Vector2(); }

        [Method("ReNormalized")]
        public Vector2 ReNormalized(float minLength, float maxLength) { return new Vector2(); }
        [Method("ReNormalized")]
        public Vector2 ReNormalized(float minLength, float maxLength, Vector2 defaultValue) { return new Vector2(); }
        [Method("ReNormalized")]
        public Vector2 ReNormalized(float minLength, float maxLength, Vector2 defaultValue, float eps) { return new Vector2(); }

        [Method("GetOrthogonalClockwise")]
        public Vector2 GetOrthogonalClockwise() { return new Vector2(); }
        [Method("GetOrthogonalCounterClockwise")]
        public Vector2 GetOrthogonalCounterClockwise() { return new Vector2(); }

        [Method("ToString")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() { return string.Empty; }
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        [Method("ToHash")]
        public uint ToHash() { return 0; }

        [Field("zero", "ZERO")]
        public static Vector2 Zero = new Vector2();
        [Field("left", "LEFT")]
        public static Vector2 Left = new Vector2();
        [Field("right", "RIGHT")]
        public static Vector2 Right = new Vector2();
        [Field("up", "UP")]
        public static Vector2 Up = new Vector2();
        [Field("down", "DOWN")]
        public static Vector2 Down = new Vector2();
        [Field("one", "ONE")]
        public static Vector2 One = new Vector2();

        [Method("Lerp")]
        [CustomCode(typeof(Vector2), new Type[] { typeof(Vector2), typeof(Vector2), typeof(Vector2) })]
        public static void Lerp(CodeBuilder code)
        {
            code.Add("Vector2 result = VectorLerp(arg0, arg1, arg2);");
        }
        [Method("Min")]
        [CustomCode(typeof(Vector2), new Type[] { typeof(Vector2), typeof(Vector2) })]
        public static void Min(CodeBuilder code)
        {
            code.Add("Vector2 result = VectorMin(arg0, arg1);");
        }
        [Method("Max")]
        [CustomCode(typeof(Vector2), new Type[] { typeof(Vector2), typeof(Vector2) })]
        public static void Max(CodeBuilder code)
        {
            code.Add("Vector2 result = VectorMax(arg0, arg1);");
        }
        [Method("Floor")]
        [CustomCode(typeof(Vector2), new Type[] { typeof(Vector2) })]
        public static void Floor(CodeBuilder code)
        {
            code.Add("Vector2 result = VectorFloor(arg0);");
        }
        [Method("Round")]
        [CustomCode(typeof(Vector2), new Type[] { typeof(Vector2) })]
        public static void Round(CodeBuilder code)
        {
            code.Add("Vector2 result = VectorRound(arg0);");
        }
        [Method("Ceil")]
        [CustomCode(typeof(Vector2), new Type[] { typeof(Vector2) })]
        public static void Ceil(CodeBuilder code)
        {
            code.Add("Vector2 result = VectorCeil(arg0);");
        }
        [Method("Abs")]
        [CustomCode(typeof(Vector2), new Type[] { typeof(Vector2) })]
        public static void _Abs(CodeBuilder code)
        {
            code.Add("Vector2 result = VectorAbs(arg0);");
        }
        [Method("Sqrt")]
        [CustomCode(typeof(Vector2), new Type[] { typeof(Vector2) })]
        public static void Sqrt(CodeBuilder code)
        {
            code.Add("Vector2 result = VectorSqrt(arg0);");
        }
        [Method("FloorToInt")]
        [CustomCode(typeof(IntVector2), new Type[] { typeof(Vector2) })]
        public static void FloorToInt(CodeBuilder code)
        {
            code.Add("IntVector2 result = VectorFloorToInt(arg0);");
        }
        [Method("RoundToInt")]
        [CustomCode(typeof(IntVector2), new Type[] { typeof(Vector2) })]
        public static void RoundToInt(CodeBuilder code)
        {
            code.Add("IntVector2 result = VectorRoundToInt(arg0);");
        }
        [Method("CeilToInt")]
        [CustomCode(typeof(IntVector2), new Type[] { typeof(Vector2) })]
        public static void CeilToInt(CodeBuilder code)
        {
            code.Add("IntVector2 result = VectorCeilToInt(arg0);");
        }
        [Method("StableRandom")]
        [CustomCode(typeof(float), new Type[] { typeof(Vector2) })]
        public static void StableRandom(CodeBuilder code)
        {
            code.Add("float result = StableRandom(arg0);");
        }
        [Method("StableRandom")]
        [CustomCode(typeof(float), new Type[] { typeof(float) })]
        public static void StableRandom2(CodeBuilder code)
        {
            code.Add("float result = StableRandom(arg0);");
        }
    }

    [Include("Urho3D/Math/Vector3.h")]
    public class IntVector3 : PrimitiveObject
    {
        [Variable("x_")]
        public int X;
        [Variable("y_")]
        public int Y;
        [Variable("z_")]
        public int Z;

        [PropertyMap("Length")]
        public float Length { get => 0f; }

        public IntVector3() : base(typeof(IntVector3)) { }

        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(IntVector3 vec) { return true; }
        [OperatorMethod(OperatorType.Add)]
        public IntVector3 AddOperator(IntVector3 vec) { return new IntVector3(); }
        [OperatorMethod(OperatorType.Sub)]
        public IntVector3 SubOperator(IntVector3 vec) { return new IntVector3(); }
        [OperatorMethod(OperatorType.Mul)]
        public IntVector3 MulOperator(int scalar) { return new IntVector3(); }
        [OperatorMethod(OperatorType.Mul)]
        public IntVector3 MulOperator(IntVector3 scalar) { return new IntVector3(); }
        [OperatorMethod(OperatorType.Div)]
        public IntVector3 DivOperator(int scalar) { return new IntVector3(); }
        [OperatorMethod(OperatorType.Div)]
        public IntVector3 DivOperator(IntVector3 scalar) { return new IntVector3(); }

        [Method("Data")]
        [CustomCode]
        public void Data(CodeBuilder code)
        {
            code
                .Add("result_code = 1;")
                .Add("const int* data = instance.Data();")
                .Add("duk_push_array(ctx);")
                .Add("for(duk_idx_t i = 0; i < 3; ++i)")
                .Scope(loopScope =>
                {
                    loopScope
                        .Add("duk_push_number(ctx, data[i]);")
                        .Add("duk_put_prop_index(ctx, -2, i);");
                });
        }

        [Method("ToString")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() { return string.Empty; }
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        [Method("ToIntVector2")]
        public IntVector2 ToIntVector2() { return new IntVector2(); }
        [Method("ToVector2")]
        public Vector2 ToVector2() { return new Vector2(); }
        [Method("ToVector3")]
        public Vector3 ToVector3() { return new Vector3(); }
        [Method("ToVector4")]
        public Vector4 ToVector4() { return new Vector4(); }
        [Method("ToVector4")]
        public Vector4 ToVector4(float w) { return new Vector4(); }
        [Method("ToHash")]
        public uint ToHash() { return 0; }

        [Field("zero", "ZERO")]
        public static IntVector3 Zero = new IntVector3();
        [Field("left", "LEFT")]
        public static IntVector3 Left = new IntVector3();
        [Field("right", "RIGHT")]
        public static IntVector3 Right = new IntVector3();
        [Field("up", "UP")]
        public static IntVector3 Up = new IntVector3();
        [Field("down", "DOWN")]
        public static IntVector3 Down = new IntVector3();
        [Field("forward", "FORWARD")]
        public static IntVector3 Forward = new IntVector3();
        [Field("back", "BACK")]
        public static IntVector3 Back = new IntVector3();
        [Field("one", "ONE")]
        public static IntVector3 One = new IntVector3();

        [Method("Min")]
        [CustomCode(typeof(IntVector3), new Type[] { typeof(IntVector3), typeof(IntVector3) })]
        public static void Min(CodeBuilder code)
        {
            code.Add("IntVector3 result = VectorMin(arg0, arg1);");
        }
        [Method("Max")]
        [CustomCode(typeof(IntVector3), new Type[] { typeof(IntVector3), typeof(IntVector3) })]
        public static void Max(CodeBuilder code)
        {
            code.Add("IntVector3 result = VectorMax(arg0, arg1);");
        }
        [Method("Abs")]
        [CustomCode(typeof(IntVector3), new Type[] { typeof(IntVector3) })]
        public static void Abs(CodeBuilder code)
        {
            code.Add("IntVector3 result = VectorAbs(arg0);");
        }
    }

    [Include("Urho3D/Math/Vector3.h")]
    public class Vector3 : PrimitiveObject
    {
        [Variable("x_")]
        public float X;
        [Variable("y_")]
        public float Y;
        [Variable("z_")]
        public float Z;

        [PropertyMap("Length")]
        public float Length { get => 0f; }
        [PropertyMap("LengthSquared")]
        public float LengthSquared { get => 0f; }
        [PropertyMap("Abs")]
        public Vector3 Abs { get => new Vector3(); }
        [PropertyMap("IsNaN")]
        public bool IsNaN { get => true; }
        [PropertyMap("IsInf")]
        public bool IsInfinite { get => true; }
        [PropertyMap("Normalized")]
        public Vector3 Normalized { get => new Vector3(); }
        [PropertyMap("ToXZ")]
        public Vector2 XZ { get => new Vector2(); }

        public Vector3(): base(typeof(Vector3)) { }

        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(Vector3 vec) { return true; }
        [OperatorMethod(OperatorType.Add)]
        public Vector3 AddOperator(Vector3 vec) { return new Vector3(); }
        [OperatorMethod(OperatorType.Sub)]
        public Vector3 SubOperator(Vector3 vec) { return new Vector3(); }
        [OperatorMethod(OperatorType.Mul)]
        public Vector3 MulOperator(float scalar) { return new Vector3(); }
        [OperatorMethod(OperatorType.Mul)]
        public Vector3 MulOperator(Vector3 scalar) { return new Vector3(); }
        [OperatorMethod(OperatorType.Div)]
        public Vector3 DivOperator(float scalar) { return new Vector3(); }
        [OperatorMethod(OperatorType.Div)]
        public Vector3 DivOperator(Vector3 scalar) { return new Vector3(); }

        [Method("Data")]
        [CustomCode]
        public void Data(CodeBuilder code)
        {
            code
                .Add("result_code = 1;")
                .Add("const float* data = instance.Data();")
                .Add("duk_push_array(ctx);")
                .Add("for(duk_idx_t i = 0; i < 3; ++i)")
                .Scope(loopScope =>
                {
                    loopScope
                        .Add("duk_push_number(ctx, data[i]);")
                        .Add("duk_put_prop_index(ctx, -2, i);");
                });
        }

        [Method("Normalize")]
        public void Normalize() { }
        [Method("DotProduct")]
        public float DotProduct(Vector3 vec) { return 0f; }
        [Method("AbsDotProduct")]
        public float AbsDotProduct(Vector3 vec) { return 0f; }

        [Method("ProjectOntoAxis")]
        public float ProjectOntoAxis(Vector3 axis) { return 0f; }

        [Method("ProjectOntoPlane")]
        public Vector3 ProjectOntoPlane(Vector3 origin, Vector3 normal) { return new Vector3(); }

        [Method("ProjectOntoLine")]
        public Vector3 ProjectOntoLine(Vector3 from, Vector3 to) { return new Vector3(); }
        [Method("ProjectOntoLine")]
        public Vector3 ProjectOntoLine(Vector3 from, Vector3 to, bool clamped) { return new Vector3(); }
        [Method("DistanceToPoint")]
        public float DistanceToPoint(Vector3 origin) { return 0f; }
        [Method("DistanceToPlane")]
        public float DistanceToPlane(Vector3 origin, Vector3 normal) { return 0f; }
        [Method("Orthogonalize")]
        public Vector3 Orthogonalize(Vector3 axis) { return new Vector3(); }
        [Method("CrossProduct")]
        public Vector3 CrossProduct(Vector3 vec) { return new Vector3(); }
        [Method("Lerp")]
        public Vector3 Lerp(Vector3 vec, float t) { return new Vector3(); }

        [Method("Equals")]
        public bool Equals(Vector3 rhs) { return true; }
        [Method("Equals")]
        public bool Equals(Vector3 rhs, float eps) { return true; }

        [Method("Angle")]
        public float Angle(Vector3 vec) { return 0f; }
        [Method("SignedAngle")]
        public float SignedAngle(Vector3 rhs, Vector3 axis) { return 0f; }
        [Method("NormalizedOrDefault")]
        public Vector3 NormalizedOrDefault() { return new Vector3(); }
        [Method("NormalizedOrDefault")]
        public Vector3 NormalizedOrDefault(Vector3 defaultValue) { return new Vector3(); }
        [Method("NormalizedOrDefault")]
        public Vector3 NormalizedOrDefault(Vector3 defaultValue, float eps) { return new Vector3(); }

        [Method("ReNormalized")]
        public Vector3 ReNormalized(float minLength, float maxLength) { return new Vector3(); }
        [Method("ReNormalized")]
        public Vector3 ReNormalized(float minLength, float maxLength, Vector3 defaultValue) { return new Vector3(); }
        [Method("ReNormalized")]
        public Vector3 ReNormalized(float minLength, float maxLength, Vector3 defaultValue, float eps) { return new Vector3(); }
        [Method("ToString")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() { return string.Empty; }
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        [Method("ToHash")]
        public uint ToHash() { return 0; }

        [Method("ToIntVector2")]
        public IntVector2 ToIntVector2() { return new IntVector2(); }
        [Method("ToVector2")]
        public Vector2 ToVector2() { return new Vector2(); }
        [Method("ToIntVector3")]
        public IntVector3 ToIntVector3() { return new IntVector3(); }
        [Method("ToVector4")]
        public Vector4 ToVector4() { return new Vector4(); }
        [Method("ToVector4")]
        public Vector4 ToVector4(float w) { return new Vector4(); }


        [Field("zero", "ZERO")]
        public static Vector3 Zero = new Vector3();
        [Field("left", "LEFT")]
        public static Vector3 Left = new Vector3();
        [Field("right", "RIGHT")]
        public static Vector3 Right = new Vector3();
        [Field("up", "UP")]
        public static Vector3 Up = new Vector3();
        [Field("down", "DOWN")]
        public static Vector3 Down = new Vector3();
        [Field("forward", "FORWARD")]
        public static Vector3 Forward = new Vector3();
        [Field("back", "BACK")]
        public static Vector3 Back = new Vector3();
        [Field("one", "ONE")]
        public static Vector3 One = new Vector3();

        [Method("Lerp")]
        [CustomCode(typeof(Vector3), new Type[] { typeof(Vector3), typeof(Vector3), typeof(Vector3)})]
        public static void Lerp(CodeBuilder code)
        {
            code.Add("Vector3 result = VectorLerp(arg0, arg1, arg2);");
        }
        [Method("Min")]
        [CustomCode(typeof(Vector3), new Type[] { typeof(Vector3), typeof(Vector3) })]
        public static void Min(CodeBuilder code)
        {
            code.Add("Vector3 result = VectorMin(arg0, arg1);");
        }
        [Method("Max")]
        [CustomCode(typeof(Vector3), new Type[] { typeof(Vector3), typeof(Vector3) })]
        public static void Max(CodeBuilder code)
        {
            code.Add("Vector3 result = VectorMax(arg0, arg1);");
        }
        [Method("Floor")]
        [CustomCode(typeof(Vector3), new Type[] { typeof(Vector3) })]
        public static void Floor(CodeBuilder code)
        {
            code.Add("Vector3 result = VectorFloor(arg0);");
        }
        [Method("Round")]
        [CustomCode(typeof(Vector3), new Type[] { typeof(Vector3) })]
        public static void Round(CodeBuilder code)
        {
            code.Add("Vector3 result = VectorRound(arg0);");
        }
        [Method("Ceil")]
        [CustomCode(typeof(Vector3), new Type[] { typeof(Vector3) })]
        public static void Ceil(CodeBuilder code)
        {
            code.Add("Vector3 result = VectorCeil(arg0);");
        }
        [Method("Abs")]
        [CustomCode(typeof(Vector3), new Type[] { typeof(Vector3) })]
        public static void _Abs(CodeBuilder code)
        {
            code.Add("Vector3 result = VectorAbs(arg0);");
        }
        [Method("Sqrt")]
        [CustomCode(typeof(Vector3), new Type[] { typeof(Vector3) })]
        public static void Sqrt(CodeBuilder code)
        {
            code.Add("Vector3 result = VectorSqrt(arg0);");
        }
        [Method("FloorToInt")]
        [CustomCode(typeof(IntVector3), new Type[] { typeof(Vector3) })]
        public static void FloorToInt(CodeBuilder code)
        {
            code.Add("IntVector3 result = VectorFloorToInt(arg0);");
        }
        [Method("RoundToInt")]
        [CustomCode(typeof(IntVector3), new Type[] { typeof(Vector3) })]
        public static void RoundToInt(CodeBuilder code)
        {
            code.Add("IntVector3 result = VectorRoundToInt(arg0);");
        }
        [Method("CeilToInt")]
        [CustomCode(typeof(IntVector3), new Type[] { typeof(Vector3) })]
        public static void CeilToInt(CodeBuilder code)
        {
            code.Add("IntVector3 result = VectorCeilToInt(arg0);");
        }
        [Method("StableRandom")]
        [CustomCode(typeof(float), new Type[] { typeof(Vector3) })]
        public static void StableRandom(CodeBuilder code)
        {
            code.Add("float result = StableRandom(arg0);");
        }
    }

    [Include("Urho3D/Math/Vector4.h")]
    public class Vector4 : PrimitiveObject
    {
        [Variable("x_")]
        public float X;
        [Variable("y_")]
        public float Y;
        [Variable("z_")]
        public float Z;
        [Variable("w_")]
        public float W;

        [PropertyMap("Length")]
        public float Length { get => 0f; }
        [PropertyMap("LengthSquared")]
        public float LengthSquared { get => 0f; }
        [PropertyMap("Abs")]
        public Vector4 Abs { get => new Vector4(); }
        [PropertyMap("IsNaN")]
        public bool IsNaN { get => true; }
        [PropertyMap("IsInf")]
        public bool IsInfinite { get => true; }

        public Vector4() : base(typeof(Vector4)) { }

        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(Vector4 vec) { return true; }
        [OperatorMethod(OperatorType.Add)]
        public Vector4 AddOperator(Vector4 vec) { return new Vector4(); }
        [OperatorMethod(OperatorType.Sub)]
        public Vector4 SubOperator(Vector4 vec) { return new Vector4(); }
        [OperatorMethod(OperatorType.Mul)]
        public Vector4 MulOperator(float scalar) { return new Vector4(); }
        [OperatorMethod(OperatorType.Mul)]
        public Vector4 MulOperator(Vector4 scalar) { return new Vector4(); }
        [OperatorMethod(OperatorType.Div)]
        public Vector4 DivOperator(float scalar) { return new Vector4(); }
        [OperatorMethod(OperatorType.Div)]
        public Vector4 DivOperator(Vector4 scalar) { return new Vector4(); }

        [Method("DotProduct")]
        public float DotProduct(Vector4 vec) => 0f;
        [Method("AbsDotProduct")]
        public float AbsDotProduct(Vector4 vec) => 0f;
        [Method("ProjectOntoAxis")]
        public float ProjectOntoAxis(Vector3 vec) => 0f;
        [Method("Lerp")]
        public Vector4 Lerp(Vector4 vec, float t) => new Vector4();
        [Method("Equals")]
        public float Equals(Vector4 vec, float eps) => 0f;

        [Method("ToIntVector2")]
        public IntVector2 ToIntVector2() => new IntVector2();
        [Method("ToVector2")]
        public Vector2 ToVector2() => new Vector2();
        [Method("ToIntVector3")]
        public IntVector3 ToIntVector3() => new IntVector3();
        [Method("ToVector3")]
        public Vector3 ToVector3() => new Vector3();

        [Method("ToString")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() => string.Empty;
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        [Method("ToHash")]
        public uint ToHash() => 0u;

        [Field("zero", "ZERO")]
        public static Vector4 Zero = new Vector4();
        [Field("one", "ONE")]
        public static Vector4 One = new Vector4();

        [Method("Lerp")]
        [CustomCode(typeof(Vector4), new Type[] { typeof(Vector4), typeof(Vector4) , typeof(Vector4) })]
        public static void VectorLerp(CodeBuilder code)
        {
            code.Add("Vector4 result = VectorLerp(arg0, arg1, arg2);");
        }
        [Method("Min")]
        [CustomCode(typeof(Vector4), new Type[] { typeof(Vector4), typeof(Vector4) })]
        public static void VectorMin(CodeBuilder code)
        {
            code.Add("Vector4 result = VectorMin(arg0, arg1);");
        }
        [Method("Max")]
        [CustomCode(typeof(Vector4), new Type[] { typeof(Vector4), typeof(Vector4) })]
        public static void VectorMax(CodeBuilder code)
        {
            code.Add("Vector4 result = VectorMax(arg0, arg1);");
        }
        [Method("Floor")]
        [CustomCode(typeof(Vector4), new Type[] { typeof(Vector4) })]
        public static void VectorFloor(CodeBuilder code)
        {
            code.Add("Vector4 result = VectorFloor(arg0);");
        }
        [Method("Round")]
        [CustomCode(typeof(Vector4), new Type[] { typeof(Vector4) })]
        public static void VectorRound(CodeBuilder code)
        {
            code.Add("Vector4 result = VectorRound(arg0);");
        }
        [Method("Ceil")]
        [CustomCode(typeof(Vector4), new Type[] { typeof(Vector4) })]
        public static void VectorCeil(CodeBuilder code)
        {
            code.Add("Vector4 result = VectorCeil(arg0);");
        }
    }
}
