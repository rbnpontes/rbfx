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
        public Ray() : base(typeof(Ray))
        {
        }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(Ray ray) { }

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
    }
}
