using JSBindTool.Core;
using JSBindTool.Core.Annotations;
using System;
using System.Collections.Generic;
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
}
