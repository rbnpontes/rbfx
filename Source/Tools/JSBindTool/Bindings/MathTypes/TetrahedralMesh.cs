using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    static class TetrahedralCodeUtils
    {
        public static void EmitIndexOutOfBoundsValidation(uint max, CodeBuilder code)
        {
            MathCodeUtils.EmitIndexOutOfBoundsValidation(max, code);
        }
        public static void EmitGetProp(string propType, string prop, uint propSize, CodeBuilder code)
        {
            MathCodeUtils.EmitGetProp(propType, prop, propSize, code);
        }
        public static void EmitSetProp(string prop, uint propSize, CodeBuilder code)
        {
            MathCodeUtils.EmitSetProp(prop, propSize, code);
        }
    }
    [Include("Urho3D/Math/TetrahedralMesh.h")]
    public class HighPrecisionVector3 : PrimitiveObject
    {
        [PropertyMap("GetX", "SetX")]
        public double X { get; set; } = 0.0;
        [PropertyMap("GetY", "SetY")]
        public double Y { get; set; } = 0.0;
        [PropertyMap("GetZ", "SetZ")]
        public double Z { get; set; } = 0.0;
        [PropertyMap("LengthSquared")]
        public double LengthSquared { get => 0.0; }

        public HighPrecisionVector3() : base(typeof(HighPrecisionVector3)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(Vector3 vec) { }

        [OperatorMethod(OperatorType.Add)]
        public HighPrecisionVector3 AddOperator(HighPrecisionVector3 vec) => new HighPrecisionVector3();
        [OperatorMethod(OperatorType.Sub)]
        public HighPrecisionVector3 SubOperator(HighPrecisionVector3 vec) => new HighPrecisionVector3();
        [OperatorMethod(OperatorType.Mul)]
        public HighPrecisionVector3 MulOperator(float value) => new HighPrecisionVector3();

        [Method]
        public double DotProduct(HighPrecisionVector3 vec) => 0.0;
        [Method]
        public HighPrecisionVector3 CrossProduct(HighPrecisionVector3 vec) => new HighPrecisionVector3();

        [Method]
        [CustomCode(typeof(Vector3))]
        public void ToVector3(CodeBuilder code)
        {
            code.Add("Vector3 result = (Vector3)*instance;");
        }
    }
    [Include("Urho3D/Math/TetrahedralMesh.h")]
    public class HighPrecisionSphere : PrimitiveObject
    {
        [Variable("center_")]
        public HighPrecisionVector3 Center;
        [Variable("radius_")]
        public double Radius = 0.0;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public HighPrecisionSphere() : base(typeof(HighPrecisionSphere)) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Constructor]
        public void Constructor() { }

        [Method]
        public double Distance(Vector3 position) => 0.0;
    }
    [Include("Urho3D/Math/TetrahedralMesh.h")]
    public class TetrahedralMeshSurfaceTriangle : PrimitiveObject
    {
        [Variable("unusedIndex_")]
        public uint UnusedIndex = 0;
        [Variable("tetIndex_")]
        public uint TetIndex = 0;
        [Variable("tetFace_")]
        public uint TetFace = 0;
        public TetrahedralMeshSurfaceTriangle() : base(typeof(TetrahedralMeshSurfaceTriangle)) { }

        [Method]
        [CustomCode(typeof(uint), new Type[] { typeof(uint) })]
        public void GetIndex(CodeBuilder code)
        {
            TetrahedralCodeUtils.EmitGetProp("unsigned", "indices_", 3, code);
        }
        [Method]
        [CustomCode(new Type[] { typeof(uint), typeof(uint) })]
        public void SetIndex(CodeBuilder code)
        {
            TetrahedralCodeUtils.EmitSetProp("indices_", 3, code);
        }
        [Method]
        [CustomCode(typeof(uint), new Type[] { typeof(uint) })]
        public void GetNeighbor(CodeBuilder code)
        {
            TetrahedralCodeUtils.EmitGetProp("unsigned", "neighbors_", 3, code);
        }
        [Method]
        [CustomCode(new Type[] { typeof(uint), typeof(uint) })]
        public void SetNeighbor(CodeBuilder code)
        {
            TetrahedralCodeUtils.EmitSetProp("neighbors_", 3, code);
        }

        [Method]
        public bool HasNeighbor(uint neighbordIndex) => false;
        [Method]
        public void Normalize(Vector<Vector3> vertices) { }
        [Method]
        public float CalculateScore(Vector<Vector3> vertices) => 0f;
    }
    [Include("Urho3D/Math/TetrahedralMesh.h")]
    public class TetrahedralMeshSurfaceEdge : PrimitiveObject
    {
        [Variable("faceIndex_")]
        public uint FaceIndex = 0;
        [Variable("edgeIndex_")]
        public uint EdgeIndex = 0;

        public TetrahedralMeshSurfaceEdge() : base(typeof(TetrahedralMeshSurfaceEdge)) { }

        [Constructor]
        public void Constructor() { }
        [Constructor]
        public void Constructor(uint i0, uint i1, uint faceIndex, uint edgeIndex) { }

        [Method]
        [CustomCode(typeof(uint), new Type[] { typeof(uint) })]
        public void GetIndex(CodeBuilder code)
        {
            TetrahedralCodeUtils.EmitGetProp("unsigned", "indices_", 2, code);
        }
        [Method]
        [CustomCode(new Type[] { typeof(uint), typeof(uint) })]
        public void SetIndex(CodeBuilder code)
        {
            TetrahedralCodeUtils.EmitSetProp("indices_", 2, code);
        }
        [Method]
        [CustomCode(typeof(bool), new Type[] { typeof(TetrahedralMeshSurfaceEdge) })]
        public void Compare(CodeBuilder code)
        {
            code.Add("bool result = (*instance) < arg0;");
        }
    }
    [Include("Urho3D/Math/TetrahedralMesh.h")]
    public class TetrahedralMeshSurface : PrimitiveObject
    {
        [Variable("faces_")]
        public Vector<TetrahedralMeshSurfaceTriangle> Faces = new Vector<TetrahedralMeshSurfaceTriangle>();
        [Variable("edges_")]
        public Vector<TetrahedralMeshSurfaceEdge> Edges = new Vector<TetrahedralMeshSurfaceEdge>();

        [PropertyMap("Size")]
        public uint Size { get => 0; }
        [PropertyMap("IsClosedSurface")]
        public bool IsClosedSurface { get => false; }

        public TetrahedralMeshSurface() : base(typeof(TetrahedralMeshSurface)) { }

        [Constructor]
        public void Constructor() { }

        [Method]
        public void Clear() { }
        [Method]
        public bool CalculateAdjacency() => false;
    }
    [Include("Urho3D/Math/TetrahedralMesh.h")]
    public class Tetrahedron : PrimitiveObject
    {
        [Variable("matrix_")]
        public Matrix3x4 Matrix = new Matrix3x4();
        public Tetrahedron() : base(typeof(Tetrahedron)) { }

        [Constructor]
        public void Constructor() { }

        [Method]
        [CustomCode(typeof(uint), new Type[] {typeof(uint)})]
        public void GetIndex(CodeBuilder code)
        {
            TetrahedralCodeUtils.EmitGetProp("unsigned", "indices_", 4, code);
        }
        [Method]
        [CustomCode(new Type[] {typeof(uint), typeof(uint) })]
        public void SetIndex(CodeBuilder code)
        {
            TetrahedralCodeUtils.EmitSetProp("indices_", 4, code);
        }
        [Method]
        [CustomCode(typeof(uint), new Type[] { typeof(uint) })]
        public void GetNeighbor(CodeBuilder code)
        {
            TetrahedralCodeUtils.EmitGetProp("unsigned", "neighbors_", 4, code);
        }
        [Method]
        [CustomCode(new Type[] { typeof(uint), typeof(uint) })]
        public void SetNeighbor(CodeBuilder code)
        {
            TetrahedralCodeUtils.EmitSetProp("neighbors_", 4, code);
        }

        [Method]
        public void CalculateInnerMatrix(Vector<Vector3> vertices) { }
        [Method]
        [CustomCode(new Type[] {typeof(uint), typeof(Array)})]
        public void GetTriangleFaceIndices(CodeBuilder code)
        {
            code
                .Add("unsigned j =0;")
                .Add("for(unsigned i =0; i < 4; ++i)")
                .Scope(code =>
                {
                    code
                        .Add("if(i != arg0)")
                        .Scope(code =>
                        {
                            code
                                .Add("duk_push_uint(ctx, instance->indices_[i]);")
                                .Add("duk_put_prop_index(ctx, arg1, j++);");
                        });
                });
        }
        [Method]
        public TetrahedralMeshSurfaceTriangle GetTriangleFace(uint faceIndex, uint tetIndex, uint tetFace) => new TetrahedralMeshSurfaceTriangle();
        [Method]
        public uint GetNeighborFaceIndex(uint neighborTetIndex) => 0;
        [Method]
        public bool HasNeighbor(uint neighborTetIndex) => false;

        [CustomField("infinity3")]
        public static void GetInfinity3(CodeBuilder code)
        {
            code.Add("duk_push_uint(ctx, Tetrahedron::Infinity3);");
        }
        [CustomField("infinity2")]
        public static void GetInfinity2(CodeBuilder code)
        {
            code.Add("duk_push_uint(ctx, Tetrahedron::Infinity2);");
        }
    }
    [Include("Urho3D/Math/TetrahedralMesh.h")]
    public class TetrahedralMesh : PrimitiveObject
    {
        public TetrahedralMesh() : base(typeof(TetrahedralMesh)) { }

        [Constructor]
        public void Constructor() { }

        [Method]
        public void Define(Vector<Vector3> positions) { }
        [Method]
        [CustomCode(new Type[] { typeof(Array) })]
        public void CollectEdges(CodeBuilder code)
        {
            code
                .Add("unsigned arr_len = duk_get_length(ctx, arg0);")
                .Add("if(arr_len == 0) return result_code;")
                .AddNewLine()
                .Add("ea::vector<ea::pair<unsigned, unsigned>> edges;")
                .Add("for (unsigned i =0; i < arr_len; ++i)")
                .Scope(code =>
                {
                    code
                        .Add("duk_get_prop_index(ctx, arg0, i);")
                        .Add("if (!(duk_is_array(ctx, -1) && duk_get_length(ctx, -1) == 2))")
                        .Scope(code =>
                        {
                            code
                                .Add("duk_push_error_object(ctx, DUK_ERR_TYPE_ERROR, \"invalid pair type on array at index %d.\", i);")
                                .Add("return duk_throw(ctx);");
                        })
                        .Add("ea::pair<unsigned, unsigned> pair;")
                        .Add("// read first member")
                        .Add("duk_get_prop_index(ctx, -1, 0);")
                        .Add("pair.first = duk_get_uint(ctx, -1);")
                        .Add("duk_pop(ctx);")
                        .Add("// read second member")
                        .Add("duk_get_prop_index(ctx, -1, 1);")
                        .Add("pair.second = duk_get_uint(ctx, -1);")
                        .Add("duk_pop_2(ctx);")
                        .Add("edges.push_back(pair);");
                })
                .Add("instance->CollectEdges(edges);");
        }
        [Method]
        public HighPrecisionSphere GetTetrahedronCircumsphere(uint tetIndex) => new HighPrecisionSphere();
        [Method]
        public Vector4 GetInnerBarycentricCoords(uint tetIndex, Vector3 position) => new Vector4();
        [Method]
        public Vector4 GetOuterBarycentricCoords(uint tetIndex, Vector3 position) => new Vector4();
        [Method]
        public Vector4 GetBarycentricCoords(uint tetIndex, Vector3 position) => new Vector4();
        [Method]
        public Vector4 GetInterpolationFactors(Vector3 position, uint tetIndexHint) => new Vector4();
        [Method]
        public double Sample(Vector<double> container, Vector3 position, uint tetIndexHint) => 0.0;
    }
}
