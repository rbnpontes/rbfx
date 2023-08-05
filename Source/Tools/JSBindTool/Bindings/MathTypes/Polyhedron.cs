using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Polyhedron.h")]
    public class Polyhedron : PrimitiveObject
    {
        [Variable("faces_")]
        public Vector<Vector<Vector3>> Faces = new Vector<Vector<Vector3>>();
        [PropertyMap("Empty")]
        public bool Empty { get => false; }
        public Polyhedron() : base(typeof(Polyhedron)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(Vector<Vector<Vector3>> faces) { }
        [Constructor]
        public void Constructor(BoundingBox bbox) { }
        [Constructor]
        public void Constructor(Frustum frustum) { }

        [Method]
        public void Define(BoundingBox bbox) { }
        [Method]
        public void Define(Frustum frustum) { }
        [Method]
        public void AddFace(Vector3 v0, Vector3 v1, Vector3 v2) { }
        [Method]
        public void AddFace(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3) { }
        [Method]
        public void AddFace(Vector<Vector3> vertices) { }
        [Method]
        public void Clip(Plane plane) { }
        [Method]
        public void Clip(BoundingBox box) { }
        [Method]
        public void Clip(Frustum frustum) { }
        [Method]
        public void Clear() { }
        [Method]
        public void Transform(Matrix3 transform) { }
        [Method]
        public void Transform(Matrix3x4 transform) { }

        public Polyhedron Transformed(Matrix3 transform) => new Polyhedron();
        public Polyhedron Transformed(Matrix3x4 transform) => new Polyhedron();
    }
}
