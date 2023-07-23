using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Quaternion.h")]
    public class Quaternion : PrimitiveObject
    {
        [Variable("x_")]
        public float X;
        [Variable("y_")]
        public float Y;
        [Variable("z_")]
        public float Z;
        [Variable("w_")]
        public float W;

        public Quaternion() : base(typeof(Quaternion))
        {
        }

        [Method("FromAngularVelocity")]
        public static Quaternion FromAngularVelocity(Vector3 angularVelocity) { return new Quaternion(); }

        [Field("identity", "IDENTITY")]
        public static Quaternion Identity = new Quaternion();
        [Field("zero", "ZERO")]
        public static Quaternion Zero = new Quaternion();
    }
}
