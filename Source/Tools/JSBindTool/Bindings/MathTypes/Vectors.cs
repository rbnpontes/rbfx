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
    [Include("Urho3D/Math/Vector4.h")]
    public class Vector2 : PrimitiveObject
    {
        [Variable("x_")]
        public float X;
        [Variable("y_")]
        public float Y;

        [PropertyMap("Normalized")]
        public Vector2 Normalized
        {
            get => new Vector2();
        }

        public Vector2(): base(typeof(Vector2)) { }

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
    public class Vector3 : PrimitiveObject
    {
        [Variable("x_")]
        public float X;
        [Variable("y_")]
        public float Y;
        [Variable("z_")]
        public float Z;

        public Vector3(): base(typeof(Vector3)) { }
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
