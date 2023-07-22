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
    public class Vector2 : PrimitiveObject
    {
        [Variable("x_")]
        public float X;
        [Variable("y_")]
        public float Y;

        public Vector2(): base(typeof(Vector2)) { }


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
}
