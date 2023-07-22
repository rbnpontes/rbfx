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
    [Operator(OperatorFlags.Add | OperatorFlags.Sub | OperatorFlags.Mul | OperatorFlags.Div | OperatorFlags.Equal)]
    public class IntVector2 : PrimitiveObject
    {
        [Variable("x_")]
        public int X;
        [Variable("y_")]
        public int Y;

        [PropertyMap("Length")]
        public float Length { get => 0f; }

        public IntVector2() : base(typeof(IntVector2)) { }

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
    }

    [Include("Urho3D/Math/Vector3.h")]
    [Operator(OperatorFlags.Add | OperatorFlags.Sub | OperatorFlags.Mul | OperatorFlags.Div | OperatorFlags.Equal)]
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

        public Vector4() : base(typeof(Vector4)) { }
    }
}
