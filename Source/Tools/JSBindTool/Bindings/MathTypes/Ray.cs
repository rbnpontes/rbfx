using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Ray.h")]
    public class DistanceAndNormal : StructObject
    {
        [Variable("normal_")]
        public Vector3 Normal = new Vector3();
        [Variable("distance_")]
        public float Distance = 0f;

        public DistanceAndNormal() : base(typeof(DistanceAndNormal)) { }
    }
    [Include("Urho3D/Math/Ray.h")]
    public class Ray : PrimitiveObject
    {
        [Variable("origin_")]
        public Vector3 Origin = new Vector3();
        [Variable("direction_")]
        public Vector3 Direction = new Vector3();
        public Ray() : base(typeof(Ray))
        {
        }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(Ray ray) { }

        [OperatorMethod(OperatorType.Equal)]
        public bool EqualOperator(Ray ray) => false;

        [Method]
        public void Define(Vector3 origin, Vector3 direction) { }
        [Method]
        public Vector3 Project(Vector3 point) => new Vector3();
        [Method]
        public float Distance(Vector3 point) => 0f;
        [Method]
        public Vector3 ClosestPoint(Ray ray) => new Vector3();
        [Method]
        public float HitDistance(Plane plane) => 0f;
        [Method]
        public float HitDistance(BoundingBox bbox) => 0f;
        [Method]
        public DistanceAndNormal HitDistanceAndNormal(BoundingBox bbox) => new DistanceAndNormal();
        [Method]
        public float HitDistance(Frustum frustum) => 0f;
        [Method]
        public float HitDistance(Frustum frustm, bool solidInside) => 0f;
        [Method]
        public float HitDistance(Sphere sphere) => 0f;
        [Method]
        public float HitDistance(Vector3 v0, Vector3 v1, Vector3 v2) => 0f;
        [Method]
        [CustomCode(typeof(float), new Type[]
        {
            typeof(Vector3),
            typeof(Vector3),
            typeof(Vector3),
            typeof(Vector3)
        })]
        public void HitDistance(CodeBuilder code)
        {
            code.Add("float result = instance->HitDistance(arg0, arg1, arg2, &arg3);");
        }
        [Method("HitDistance")]
        [CustomCode(typeof(float), new Type[]
        {
            typeof(Vector3),
            typeof(Vector3),
            typeof(Vector3),
            typeof(Vector3),
            typeof(Vector3)
        })]
        public void HitDistance1(CodeBuilder code)
        {
            code.Add("float result = instance->HitDistance(arg0, arg1, arg2, &arg3, &arg4);");
        }
        [Method]
        public Ray Transformed(Matrix3x4 transform) => new Ray();
    }
}
