using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/Frustum.h")]
    public class Frustum : PrimitiveObject
    {
        public Frustum() : base(typeof(Frustum)) { }
        #region Methods
        [Method("Define")]
        public void Define(float fov, float aspectRatio, float zoom, float nearZ, float farZ) { }
        [Method("Define")]
        public void Define(float fov, float aspectRatio, float zoom, float nearZ, float farZ, Matrix3x4 transform) { }
        [Method("Define")]
        public void Define(Vector3 nearSize, Vector3 farSize) { }
        [Method("Define")]
        public void Define(Vector3 nearSize, Vector3 farSize, Matrix3x4 transform) { }
        [Method("Define")]
        public void Define(BoundingBox box) { }
        [Method("Define")]
        public void Define(BoundingBox box, Matrix3x4 transform) { }
        [Method("Define")]
        public void Define(Matrix4 projection) { }
        [Method("DefineOrtho")]
        public void DefineOrtho(float orthoSize, float aspectRatio, float zoom, float nearZ, float farZ) { }
        [Method("DefineOrtho")]
        public void DefineOrtho(float orthoSize, float aspectRatio, float zoom, float nearZ, float farZ, Matrix3x4 transform) { }
        [Method("DefineSplit")]
        public void DefineSplit(Matrix4 projection, float nearZ, float farZ) { }
        [Method("Transform")]
        public void Transform(Matrix3 transform) { }
        [Method("Transform")]
        public void Transform(Matrix3x4 transform) { }

        [Method("IsInside")]
        public Intersection IsInside(Vector3 point) => Intersection.Outside;
        [Method("IsInside")]
        public Intersection IsInside(Sphere sphere) => Intersection.Outside;
        [Method("IsInside")]
        public Intersection IsInside(BoundingBox box) => Intersection.Outside;
        [Method("IsInsideFast")]
        public Intersection IsInsideFast(Sphere sphere) => Intersection.Outside;
        [Method("IsInsideFast")]
        public Intersection IsInsideFast(BoundingBox box) => Intersection.Outside;

        [Method("Distance")]
        public float Distance(Vector3 point) => 0f;

        [Method("Transformed")]
        public Frustum Transformed(Matrix3 transform) => new Frustum();
        [Method("Transformed")]
        public Frustum Transformed(Matrix3x4 transform) => new Frustum();
        [Method("Projected")]
        public Rect Projected(Matrix4 transform) => new Rect();

        private void EmitPlaneIdxValidation(CodeBuilder code)
        {
            code
                .Add("if(arg0 >= NUM_FRUSTUM_PLANES)")
                .Scope(code =>
                {
                    code
                        .Add("duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"argument index is out of bounds. Must be less than Frustum.numFrustumPlanes.\");")
                        .Add("return duk_throw(ctx);");
                })
                .AddNewLine();
        }
        private void EmitVertexIdxValidation(CodeBuilder code)
        {
            code
                .Add("if(arg0 >= NUM_FRUSTUM_VERTICES)")
                .Scope(code =>
                {
                    code
                        .Add("duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"argument index is out of bounds. Must be less than Frustum.numFrustumVertices.\");")
                        .Add("return duk_throw(ctx);");
                })
                .AddNewLine();
        }
        // the methods bellow has been created because we don't have
        // control by plane and vertice arrays, this can lead to unexpected issues
        // in this case, we will create getters and setters to access planes and vertices
        // members
        [Method("GetPlanes")]
        [CustomCode(typeof(Plane), new Type[] { typeof(uint)})]
        public void GetPlane(CodeBuilder code)
        {
            EmitPlaneIdxValidation(code);
            code.Add("Plane result = instance.planes_[arg0];");
        }
        [Method("SetPlanes")]
        [CustomCode(new Type[] { typeof(uint), typeof(Plane) })]
        public void SetPlane(CodeBuilder code)
        {
            EmitPlaneIdxValidation(code);
            code.Add("instance.planes_[arg0] = arg1;");
        }
        [Method("GetVertex")]
        [CustomCode(typeof(Vector3), new Type[] { typeof(uint) })]
        public void GetVertex(CodeBuilder code)
        {
            EmitVertexIdxValidation(code);
            code.Add("Vector3 result = instance.vertices_[arg0];");
        }
        [Method("SetVertex")]
        [CustomCode(new Type[] { typeof(uint), typeof(Vector3) })]
        public void SetVertex(CodeBuilder code)
        {
            EmitPlaneIdxValidation(code);
            code.Add("instance.vertices_[arg0] = arg1;");
        }
        #endregion
        #region Static Fields
        [CustomField("NumFrustumPlanes")]
        public static void GetNumFrustumPlanes(CodeBuilder code)
        {
            code.Add("duk_push_uint(ctx, NUM_FRUSTUM_PLANES);");
        }
        [CustomField("NumFrustumVertices")]
        public static void GetNumFrustumVertices(CodeBuilder code)
        {
            code.Add("duk_push_uint(ctx, NUM_FRUSTUM_VERTICES);");
        }
        #endregion
    }
}
