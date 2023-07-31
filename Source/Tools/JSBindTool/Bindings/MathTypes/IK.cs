using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Bindings.MathTypes
{
    [Include("Urho3D/Math/InverseKinematics.h")]
    public class IKSettings : PrimitiveObject
    {
        [Variable("maxIterations_")]
        public uint MaxIterations;
        [Variable("tolerance_")]
        public float Tolerance;
        [Variable("continuousRotations_")]
        public bool ContinuousRotations;

        public IKSettings() : base(typeof(IKSettings)) { }
    }

    [Include("Urho3D/Math/InverseKinematics.h")]
    public class IKNode : PrimitiveObject
    {
        [Variable("localOriginalPosition_")]
        public Vector3 LocalOriginalPosition = new Vector3();
        [Variable("localOriginalRotation_")]
        public Quaternion LocalOriginalRotation = new Quaternion();

        [Variable("originalPosition_")]
        public Vector3 OriginalPosition = new Vector3();
        [Variable("originalRotation_")]
        public Quaternion OriginalRotation = new Quaternion();

        [Variable("position_")]
        public Vector3 Position = new Vector3() ;
        [Variable("rotation_")]
        public Quaternion Rotation = new Quaternion() ;

        [Variable("positionDirty_")]
        public bool PositionDirty;
        [Variable("rotationDirty_")]
        public bool RotationDirty;

        public IKNode() : base(typeof(IKNode)) { }

        [Method("SetOriginalTransform")]
        public void SetOriginalTransform(Vector3 position, Quaternion rotation, Matrix3x4 inverseWorldTransform) { }
        [Method("UpdateOriginalTransform")]
        public void UpdateOriginalTransform(Matrix3x4 worldTransform) { }
        [Method("RotateAround")]
        public void RotateAround(Vector3 point, Quaternion rotation) { }
        [Method("ResetOriginalTransform")]
        public void ResetOriginalTransform() { }
        [Method("StorePreviousTransform")]
        public void StorePreviousTransform() { }
        [Method("MarkPositionDirty")]
        public void MarkPositionDirty() { }
        [Method("MarkRotationDirty")]
        public void MarkRotationDirty() { }
    }

    [Include("Urho3D/Math/InverseKinematics.h")]
    public class IKNodeSegment : PrimitiveObject
    {
        #region Variables
        [Variable("beginNode_")]
        public RefPtr<IKNode> BeginNode = new RefPtr<IKNode>();
        [Variable("endNode_")]
        public RefPtr<IKNode> EndNode = new RefPtr<IKNode>();
        [Variable("length_")]
        public float Length;
        #endregion
        public IKNodeSegment() : base(typeof(IKNodeSegment))
        {
        }

        [Method]
        public Quaternion CalculateRotationDeltaFromPrevious() => new Quaternion();
        [Method]
        public Quaternion CalculateRotationDeltaFromOriginal() => new Quaternion();
        [Method]
        public Quaternion CalculateRotation(IKSettings settings) => new Quaternion();
        [Method]
        public Vector3 CalculateDirection() => new Vector3();

        [Method]
        public void UpdateLength() { }
        [Method]
        public void UpdateRotationInNodes(bool fromPrevious, bool isLastSegment) { }
        [Method]
        public void Twist(float angle, bool isLastSegment) { }
    }

    [Include("Urho3D/Math/InverseKinematics.h")]
    public class IKTrigonometricChain : PrimitiveObject
    {
        [PropertyMap("GetBeginNode")]
        public RefPtr<IKNode> BeginNode { get => new RefPtr<IKNode>(); }
        [PropertyMap("GetMiddleNode")]
        public RefPtr<IKNode> MiddleNode { get => new RefPtr<IKNode>(); }
        [PropertyMap("GetEndNode")]
        public RefPtr<IKNode> EndNode { get => new RefPtr<IKNode>(); }
        [PropertyMap("GetFirstLength")]
        public float FirstLength { get => 0f; }
        [PropertyMap("GetSecondLength")]
        public float SecondLength { get => 0f; }
        [PropertyMap("GetCurrentChainRotation")]
        public Quaternion CurrentChainRotation { get => new Quaternion(); }

        public IKTrigonometricChain() : base(typeof(IKTrigonometricChain))
        {
        }

        #region Methods
        [Method]
        public void Initialize(RefPtr<IKNode> node1, RefPtr<IKNode> node2, RefPtr<IKNode> node3) { }
        [Method]
        public void UpdateLengths() { }
        [Method]
        public void Solve(Vector3 target, Vector3 originalDirection, Vector3 currentDirection, float minAngle, float maxAngle) { }
        #endregion
        #region Static Methods
        public static Quaternion CalculateRotation(
            Vector3 originalPos0, Vector3 originalPos2, Vector3 originalDirection,
            Vector3 currentPos0, Vector3 currentPos2, Vector3 currentDirection
        ) => new Quaternion();
        [Method]
        [CustomCode(new Type[] {
            typeof(Vector3),
            typeof(float),
            typeof(float),
            typeof(Vector3),
            typeof(Vector3),
            typeof(float),
            typeof(float)
        })]
        public static void Solve(CodeBuilder code)
        {
            StringBuilder args = new StringBuilder();
            for(int i =0; i < 7; ++i)
            {
                args.Append($"arg{i}");
                if (i < 6)
                    args.Append(", ");
            }
            code
                .Add($"ea::pair<Vector3, Vector3> result = IKTrigonometricChain::Solve({args});")
                .Add("duk_push_array(ctx);")
                .Add($"{CodeUtils.GetPushSignature(typeof(Vector3))}(ctx, result.first);")
                .Add("duk_put_prop_index(ctx, -2, 0);")
                .Add($"{CodeUtils.GetPushSignature(typeof(Vector3))}(ctx, result.second);")
                .Add("duk_put_prop_index(ctx, -2, 1);")
                .Add("result_code = 1;");
        }
        #endregion
    }
    [Include("Urho3D/Math/InverseKinematics.h")]
    public class IKChain : PrimitiveObject
    {
        public IKChain() : base(typeof(IKChain)) { }
        public IKChain(Type type) : base(type) { }

        [Method]
        public void Clear() { }
        [Method]
        public void AddNode(RefPtr<IKNode> node) { }
        [Method]
        public void UpdateLengths() { }
        [Method]
        public Const<RefPtr<IKNodeSegment>> FindSegment(RefPtr<IKNode> node) => new Const<RefPtr<IKNodeSegment>>();
        [Method]
        public Vector<IKNodeSegment> GetSegments() => new Vector<IKNodeSegment>();
        [Method]
        public Vector<RefPtr<IKNode>> GetNodes() => new Vector<RefPtr<IKNode>>();
    }
    [Include("Urho3D/Math/InverseKinematics.h")]
    public class IKSpineChain : IKChain
    {
        public IKSpineChain() : base(typeof(IKSpineChain)) { }

        [Method]
        public void Solve(Vector3 target, Vector3 baseDirection,
            float maxRotation, IKSettings settings)
        { }
        [Method]
        [CustomCode(
            new Type[]
            {
                typeof(Vector3),
                typeof(Vector3),
                typeof(float),
                typeof(IKSettings),
                typeof(JSFunction)
            }
        )]
        public void Solve(CodeBuilder code)
        {
            StringBuilder args = new StringBuilder();
            for(int i=0; i < 4; ++i)
            {
                args.Append($"arg{i}, ");
            }
            code
                .Add("IKSpineChain::WeightFunction func = [ctx, arg4](float x)")
                .Lambda(code =>
                {
                    code
                        .Add("duk_dup(ctx, arg4);")
                        .Add("duk_push_this(ctx);")
                        .Add("duk_push_number(ctx, x);")
                        .Add("duk_call_method(ctx, 1);")
                        .Add("float res = (float)duk_get_number_default(ctx, -1, 1.0);")
                        .Add("duk_pop(ctx);")
                        .Add("return res;");
                })
                .Add($"instance->Solve({args}func);");
        }
        [Method]
        public void Twist(float angle, IKSettings settings) { }
    }
}
