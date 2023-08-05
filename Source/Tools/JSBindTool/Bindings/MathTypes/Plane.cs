using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Plane.h")]
    public class Plane : PrimitiveObject
    {
        [Variable("normal_")]
        public Vector3 Normal = new Vector3();
        [Variable("absNormal_")]
        public Vector3 AbsNormal = new Vector3();
        [Variable("d_")]
        public float D = 0f;
        [PropertyMap("ReflectionMatrix")]
        public Matrix3x4 ReflectionMatrix { get => new Matrix3x4(); }

        public Plane() : base(typeof(Plane)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(Plane plane) { }
        [Constructor]
        public void Constructor(Vector3 v0, Vector3 v1, Vector3 v2) { }
        [Constructor]
        public void Constructor(Vector3 normal, Vector3 point) { }
        [Constructor]
        public void Constructor(Vector4 plane) { }

        [Method]
        public void Define(Vector3 v0, Vector3 v1, Vector3 v2) { }
        [Method]
        public void Define(Vector3 normal, Vector3 point) { }
        [Method]
        public void Define(Vector4 plane) { }
        [Method]
        public void Transform(Matrix3 transform) { }
        [Method]
        public void Transform(Matrix3x4 transform) { }
        [Method]
        public void Transform(Matrix4 transform) { }
        [Method]
        public Vector3 Project(Vector3 point) => new Vector3();
        [Method]
        public float Distance(Vector3 point) => 0f;
        [Method]
        public Vector3 Reflect(Vector3 direction) => new Vector3();
        [Method]
        public Plane Transformed(Matrix3 transform) => new Plane();
        [Method]
        public Plane Transformed(Matrix3x4 transform) => new Plane();
        [Method]
        public Plane Transformed(Matrix4 transform) => new Plane();

        [Method]
        public Vector4 ToVector4() => new Vector4();

        [Field("up", "UP")]
        public static Plane Up = new Plane();
    }
}
