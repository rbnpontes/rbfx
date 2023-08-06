using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Sphere.h")]
    public class Circle : PrimitiveObject
    {
        [Variable("center_")]
        public Vector3 Center = new Vector3();
        [Variable("normal_")]
        public Vector3 Normal = new Vector3();
        [Variable("radius_")]
        public float Radius = 0f;

        [PropertyMap("IsValid")]
        public bool IsValid { get => false; }

        public Circle() : base(typeof(Circle)) { }

        [Constructor]
        public void Constructor() { }
        [Method]
        public Vector3 GetPoint(Vector3 directionHint) => new Vector3();
    }
    [Include("Urho3D/Math/Sphere.h")]
    public class Sphere : PrimitiveObject
    {
        [Variable("center_")]
        public Vector3 Center = new Vector3();
        [Variable("radius_")]
        public float Radius = 0f;

        [PropertyMap("Defined")]
        public bool Defined { get => false; }
        public Sphere() : base(typeof(Sphere))
        {
        }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(Vector3 center, float radius) { }
        [Constructor]
        [CustomCode(new Type[] { typeof(Vector<Vector3>) })]
        public void Constructor(CodeBuilder code, ConstructorData.CustomCodeModifiers modifiers)
        {
            code.Add("Sphere* instance = new Sphere(arg0.data(), arg0.size());");
        }
        [Constructor]
        public void Constructor(BoundingBox box) { }
        [Constructor]
        public void Constructor(Frustum frustum) { }
        [Constructor]
        public void Constructor(Polyhedron poly) { }

        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(Sphere sphere) => false;

        [Method]
        public void Define(Sphere sphere) { }
        [Method]
        public void Define(Vector3 center, float radius) { }
        [Method]
        [CustomCode(new Type[] { typeof(Vector<Vector3>) })]
        public void Define(CodeBuilder code)
        {
            code.Add("instance->Define(arg0.data(), arg0.size());");
        }
        [Method]
        public void Define(BoundingBox box) { }
        [Method]
        public void Define(Frustum frustum) { }
        [Method]
        public void Define(Polyhedron poly) { }
        [Method]
        public void Merge(Vector3 point) { }
        [Method]
        [CustomCode(new Type[] { typeof(Vector<Vector3>) })]
        public void Merge(CodeBuilder code)
        {
            code.Add($"instance->Merge(arg0.data(), arg0.size());");
        }
        [Method]
        public void Merge(BoundingBox bbox) { }
        [Method]
        public void Merge(Frustum frustum) { }
        [Method]
        public void Merge(Polyhedron polyhedron) { }
        [Method]
        public void Merge(Sphere sphere) { }
        [Method]
        public void Clear() { }

        [Method]
        public Intersection IsInside(Vector3 point) => Intersection.Outside;
        [Method]
        public Intersection IsInside(Sphere sphere) => Intersection.Outside;
        [Method]
        public Intersection IsInside(BoundingBox box) => Intersection.Outside;
        [Method]
        public Intersection IsInsideFast(Sphere sphere) => Intersection.Outside;
        [Method]
        public Intersection IsInsideFast(BoundingBox box) => Intersection.Outside;
        [Method]
        public Circle Intersect(Sphere sphere) => new Circle();
        [Method]
        public float Distance(Vector3 point) => 0f;
        [Method]
        public Vector3 GetLocalPoint(float theta, float phi) => new Vector3();
        [Method]
        public Vector3 GetPoint(float theta, float phi) => new Vector3();
    }
}
