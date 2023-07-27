using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/BoundingBox.h")]
    public class BoundingBox : PrimitiveObject
    {
        #region Variables
        [Variable("min_")]
        public Vector3 Min = new Vector3();
        [Variable("max_")]
        public Vector3 Max = new Vector3();
        #endregion
        #region Properties
        [PropertyMap("Defined")]
        public bool Defined { get => false; }
        [PropertyMap("Center")]
        public Vector3 Center { get => new Vector3(); }
        [PropertyMap("Size")]
        public Vector3 Size { get => new Vector3(); }
        [PropertyMap("HalfSize")]
        public Vector3 HalfSize { get => new Vector3(); }
        [PropertyMap("Volume")]
        public float Volume { get => 0f; }
        #endregion
        public BoundingBox() : base(typeof(BoundingBox))
        {
        }

        #region Methods
        [Method("Define")]
        public void Define(BoundingBox bbox) { }
        [Method("Define")]
        public void Define(Rect rect) { }
        [Method("Define")]
        public void Define(Vector3 min, Vector3 max) { }
        [Method("Define")]
        public void Define(float min, float max) { }
        [Method("Define")]
        public void Define(Vector3 point) { }
        [Method("Define")]
        [CustomCode(new Type[] { typeof(Vector<Vector3>)})]
        public void Define(CodeBuilder code)
        {
            code.Add("instance.Define(arg0.data(), arg0.size());");
        }
        [Method("Define")]
        public void Define(Frustum frustum) { }
        [Method("Define")]
        public void Define(Polyhedron polyhedron) { }
        [Method("Define")]
        public void Define(Sphere sphere) { }
        
        [Method("Merge")]
        public void Merge(Vector3 point) { }
        [Method("Merge")]
        public void Merge(BoundingBox box) { }
        [Method("Merge")]
        [CustomCode(new Type[] { typeof(Vector<Vector3>) })]
        public void Merge(CodeBuilder code)
        {
            code.Add("instance.Merge(arg0.data(), arg0.size());");
        }
        [Method("Merge")]
        public void Merge(Frustum frustum) { }
        [Method("Merge")]
        public void Merge(Polyhedron poly) { }
        [Method("Merge")]
        public void Merge(Sphere sphere) { }
        [Method("Clip")]
        public void Clip(BoundingBox box) { }
        [Method("Transform")]
        public void Transform(Matrix3 transform) { }
        [Method("Transform")]
        public void Transform(Matrix3x4 transform) { }
        [Method("Clear")]
        public void Clear() { }

        [Method("Merged")]
        public BoundingBox Merged(Vector3 point) => new BoundingBox();
        [Method("Merged")]
        public BoundingBox Merged(BoundingBox box) => new BoundingBox();
        [Method("Merged")]
        public BoundingBox Merged(Frustum frustum) => new BoundingBox();
        [Method("Merged")]
        public BoundingBox Merged(Polyhedron poly) => new BoundingBox();
        [Method("Merged")]
        public BoundingBox Merged(Sphere point) => new BoundingBox();

        [Method("Clipped")]
        public BoundingBox Clipped(BoundingBox box) => new BoundingBox();
        [Method("Padded")]
        public BoundingBox Padded(Vector3 padding) => new BoundingBox();
        [Method("Padded")]
        public BoundingBox Padded(Vector3 minPadding, Vector3 maxPadding) => new BoundingBox();

        [Method("Transformed")]
        public BoundingBox Transformed(Matrix3 transform) => new BoundingBox();
        [Method("Transformed")]
        public BoundingBox Transformed(Matrix3x4 transform) => new BoundingBox();
        [Method("Projected")]
        public Rect Projected(Matrix4 transform) => new Rect();
        [Method("DistanceToPoint")]
        public float DistanceToPoint(Vector3 point) => 0f;
        [Method("SignedDistanceToPoint")]
        public float SignedDistanceToPoint(Vector3 point) => 0f;
        [Method("DistanceToBoundingBox")]
        public float DistanceToBoundingBox(BoundingBox box) => 0f;
        [Method("SignedDistanceToBoundingBox")]
        public float SignedDistanceToBoundingBox(BoundingBox box) => 0f;

        [Method("IsInside")]
        public Intersection IsInside(Vector3 point) => Intersection.Outside;
        [Method("IsInside")]
        public Intersection IsInside(BoundingBox box) => Intersection.Outside;
        [Method("IsInside")]
        public Intersection IsInside(Sphere sphere) => Intersection.Outside;
        [Method("IsInsideFast")]
        public Intersection IsInsideFast(BoundingBox box) => Intersection.Outside;
        [Method("IsInsideFast")]
        public Intersection IsInsideFast(Sphere sphere) => Intersection.Outside;

        [Method("ToString")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString() => string.Empty;
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        #endregion
    }
}
