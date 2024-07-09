#region assembly Oculus.Interaction, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Splendide\SuperHandsVR\VR_Test1\Library\ScriptAssemblies\Oculus.Interaction.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Buffers;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

public class SS_BaseTransformer : MonoBehaviour, ITransformer
{
    private struct GrabPointDelta
    {
        private const float _epsilon = 1E-06f;

        public Vector3 PrevCentroidOffset { get; private set; }

        public Vector3 CentroidOffset { get; private set; }

        public Quaternion PrevRotation { get; private set; }

        public Quaternion Rotation { get; private set; }

        public GrabPointDelta(Vector3 centroidOffset, Quaternion rotation)
        {
            Vector3 prevCentroidOffset = (CentroidOffset = centroidOffset);
            PrevCentroidOffset = prevCentroidOffset;
            Quaternion prevRotation = (Rotation = rotation);
            PrevRotation = prevRotation;
        }

        public void UpdateData(Vector3 centroidOffset, Quaternion rotation)
        {
            PrevCentroidOffset = CentroidOffset;
            CentroidOffset = centroidOffset;
            PrevRotation = Rotation;
            if (Quaternion.Dot(rotation, Rotation) < 0f)
            {
                rotation.x = 0f - rotation.x;
                rotation.y = 0f - rotation.y;
                rotation.z = 0f - rotation.z;
                rotation.w = 0f - rotation.w;
            }

            Rotation = rotation;
        }

        public bool IsValidAxis()
        {
            return CentroidOffset.sqrMagnitude > 1E-06f;
        }
    }

    [SerializeField]
    [Tooltip("Constrains the position of the object along different axes. Units are meters.")]
    private TransformerUtils.PositionConstraints _positionConstraints = new TransformerUtils.PositionConstraints
    {
        XAxis = default(TransformerUtils.ConstrainedAxis),
        YAxis = default(TransformerUtils.ConstrainedAxis),
        ZAxis = default(TransformerUtils.ConstrainedAxis)
    };

    [SerializeField]
    [Tooltip("Constrains the rotation of the object along different axes. Units are degrees.")]
    private TransformerUtils.RotationConstraints _rotationConstraints = new TransformerUtils.RotationConstraints
    {
        XAxis = default(TransformerUtils.ConstrainedAxis),
        YAxis = default(TransformerUtils.ConstrainedAxis),
        ZAxis = default(TransformerUtils.ConstrainedAxis)
    };

    [SerializeField]
    [Tooltip("Constrains the local scale of the object along different axes. Expressed as a scale factor.")]
    private TransformerUtils.ScaleConstraints _scaleConstraints = new TransformerUtils.ScaleConstraints
    {
        ConstraintsAreRelative = true,
        XAxis = new TransformerUtils.ConstrainedAxis
        {
            ConstrainAxis = true,
            AxisRange = new TransformerUtils.FloatRange
            {
                Min = 1f,
                Max = 1f
            }
        },
        YAxis = new TransformerUtils.ConstrainedAxis
        {
            ConstrainAxis = true,
            AxisRange = new TransformerUtils.FloatRange
            {
                Min = 1f,
                Max = 1f
            }
        },
        ZAxis = new TransformerUtils.ConstrainedAxis
        {
            ConstrainAxis = true,
            AxisRange = new TransformerUtils.FloatRange
            {
                Min = 1f,
                Max = 1f
            }
        }
    };

    private IGrabbable _grabbable;

    private Pose _grabDeltaInLocalSpace;

    private TransformerUtils.PositionConstraints _relativePositionConstraints;

    private TransformerUtils.ScaleConstraints _relativeScaleConstraints;

    private Quaternion _lastRotation = Quaternion.identity;

    private Vector3 _lastScale = Vector3.one;

    private GrabPointDelta[] _deltas;

    public void Initialize(IGrabbable grabbable)
    {
        _grabbable = grabbable;
        _relativePositionConstraints = TransformerUtils.GenerateParentConstraints(_positionConstraints, _grabbable.Transform.localPosition);
        _relativeScaleConstraints = TransformerUtils.GenerateParentConstraints(_scaleConstraints, _grabbable.Transform.localScale);
    }

    public virtual void BeginTransform()
    {
        //Debug.Log("BeginTransform");

        //Debug.Log(_grabbable.Transform.gameObject.name);

        int count = _grabbable.GrabPoints.Count;
        Vector3 centroid = GetCentroid(_grabbable.GrabPoints);
        _deltas = ArrayPool<GrabPointDelta>.Shared.Rent(count);
        for (int i = 0; i < count; i++)
        {
            Vector3 centroidOffset = GetCentroidOffset(_grabbable.GrabPoints[i], centroid);
            _deltas[i] = new GrabPointDelta(centroidOffset, _grabbable.GrabPoints[i].rotation);
        }

        Transform transform = _grabbable.Transform;
        _grabDeltaInLocalSpace = new Pose(transform.InverseTransformVector(centroid - transform.position), transform.rotation);
        _lastRotation = Quaternion.identity;
        _lastScale = transform.localScale;
    }

    public virtual void UpdateTransform()
    {
        //Debug.Log("UpdateTransform");

        int count = _grabbable.GrabPoints.Count;
        Transform transform = _grabbable.Transform;
        Vector3 vector = UpdateTransformerPointData(_grabbable.GrabPoints);

        //_lastScale = UpdateScale(count) * _lastScale;
        //transform.localScale = TransformerUtils.GetConstrainedTransformScale(_lastScale, _relativeScaleConstraints);

        _lastRotation = UpdateRotation(count) * _lastRotation;
        Quaternion unconstrainedRotation = _lastRotation * _grabDeltaInLocalSpace.rotation;
        transform.rotation = TransformerUtils.GetConstrainedTransformRotation(unconstrainedRotation, _rotationConstraints, transform.parent);
        Vector3 unconstrainedPosition = vector - transform.TransformVector(_grabDeltaInLocalSpace.position);
        transform.position = TransformerUtils.GetConstrainedTransformPosition(unconstrainedPosition, _relativePositionConstraints, transform.parent);
    }

    public virtual void EndTransform()
    {
        //Debug.Log("EndTransform");


        ArrayPool<GrabPointDelta>.Shared.Return(_deltas);
        _deltas = null;
    }

    private Vector3 UpdateTransformerPointData(List<Pose> poses)
    {
        Vector3 centroid = GetCentroid(poses);
        for (int i = 0; i < poses.Count; i++)
        {
            Vector3 centroidOffset = GetCentroidOffset(poses[i], centroid);
            _deltas[i].UpdateData(centroidOffset, poses[i].rotation);
        }

        return centroid;
    }

    private Vector3 GetCentroid(List<Pose> poses)
    {
        int count = poses.Count;
        Vector3 zero = Vector3.zero;
        for (int i = 0; i < count; i++)
        {
            zero += poses[i].position;
        }

        return zero / count;
    }

    private Vector3 GetCentroidOffset(Pose pose, Vector3 centre)
    {
        return centre - pose.position;
    }

    private Quaternion UpdateRotation(int count)
    {
        Quaternion quaternion = Quaternion.identity;
        float t = 1f / (float)count;
        for (int i = 0; i < count; i++)
        {
            GrabPointDelta grabPointDelta = _deltas[i];
            Quaternion b = grabPointDelta.Rotation * Quaternion.Inverse(grabPointDelta.PrevRotation);
            if (grabPointDelta.IsValidAxis())
            {
                Vector3 normalized = grabPointDelta.CentroidOffset.normalized;
                Quaternion b2 = Quaternion.FromToRotation(grabPointDelta.PrevCentroidOffset.normalized, normalized);
                quaternion = Quaternion.Slerp(Quaternion.identity, b2, t) * quaternion;
                b.ToAngleAxis(out var angle, out var axis);
                float num = Vector3.Dot(axis, normalized);
                b = Quaternion.AngleAxis(angle * num, normalized);
            }

            quaternion = Quaternion.Slerp(Quaternion.identity, b, t) * quaternion;
        }

        return quaternion;
    }

    private float UpdateScale(int count)
    {
        float num = 0f;
        for (int i = 0; i < count; i++)
        {
            GrabPointDelta grabPointDelta = _deltas[i];
            if (grabPointDelta.IsValidAxis())
            {
                float num2 = Mathf.Sqrt(grabPointDelta.CentroidOffset.sqrMagnitude / grabPointDelta.PrevCentroidOffset.sqrMagnitude);
                num += num2 / (float)count;
            }
            else
            {
                num += 1f / (float)count;
            }
        }

        return num;
    }

    public void InjectOptionalPositionConstraints(TransformerUtils.PositionConstraints constraints)
    {
        _positionConstraints = constraints;
    }

    public void InjectOptionalRotationConstraints(TransformerUtils.RotationConstraints constraints)
    {
        _rotationConstraints = constraints;
    }

    public void InjectOptionalScaleConstraints(TransformerUtils.ScaleConstraints constraints)
    {
        _scaleConstraints = constraints;
    }
}

#if false // Journal de décompilation
'329' éléments dans le cache
------------------
Résoudre : 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Un seul assembly trouvé : 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Charger à partir de : 'C:\Program Files\Unity\Hub\Editor\2022.3.8f1\Editor\Data\NetStandard\ref\2.1.0\netstandard.dll'
------------------
Résoudre : 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Un seul assembly trouvé : 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Charger à partir de : 'C:\Program Files\Unity\Hub\Editor\2022.3.8f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll'
------------------
Résoudre : 'UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Un seul assembly trouvé : 'UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Charger à partir de : 'C:\Program Files\Unity\Hub\Editor\2022.3.8f1\Editor\Data\Managed\UnityEngine\UnityEditor.CoreModule.dll'
------------------
Résoudre : 'UnityEngine.IMGUIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Un seul assembly trouvé : 'UnityEngine.IMGUIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Charger à partir de : 'C:\Program Files\Unity\Hub\Editor\2022.3.8f1\Editor\Data\Managed\UnityEngine\UnityEngine.IMGUIModule.dll'
------------------
Résoudre : 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Un seul assembly trouvé : 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Charger à partir de : 'C:\Program Files\Unity\Hub\Editor\2022.3.8f1\Editor\Data\Managed\UnityEngine\UnityEngine.PhysicsModule.dll'
------------------
Résoudre : 'UnityEngine.UIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Un seul assembly trouvé : 'UnityEngine.UIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Charger à partir de : 'C:\Program Files\Unity\Hub\Editor\2022.3.8f1\Editor\Data\Managed\UnityEngine\UnityEngine.UIModule.dll'
------------------
Résoudre : 'UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
Un seul assembly trouvé : 'UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
Charger à partir de : 'C:\Splendide\SuperHandsVR\VR_Test1\Library\ScriptAssemblies\UnityEngine.UI.dll'
------------------
Résoudre : 'Unity.TextMeshPro, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Un seul assembly trouvé : 'Unity.TextMeshPro, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Charger à partir de : 'C:\Splendide\SuperHandsVR\VR_Test1\Library\ScriptAssemblies\Unity.TextMeshPro.dll'
------------------
Résoudre : 'UnityEngine.AIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Un seul assembly trouvé : 'UnityEngine.AIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Charger à partir de : 'C:\Program Files\Unity\Hub\Editor\2022.3.8f1\Editor\Data\Managed\UnityEngine\UnityEngine.AIModule.dll'
------------------
Résoudre : 'UnityEngine.InputLegacyModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Un seul assembly trouvé : 'UnityEngine.InputLegacyModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Charger à partir de : 'C:\Program Files\Unity\Hub\Editor\2022.3.8f1\Editor\Data\Managed\UnityEngine\UnityEngine.InputLegacyModule.dll'
------------------
Résoudre : 'System.Runtime.InteropServices, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null'
Un seul assembly trouvé : 'System.Runtime.InteropServices, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
AVERTISSEMENT : Incompatibilité de version. Attendu : '2.1.0.0'. Reçu : '4.1.2.0'
Charger à partir de : 'C:\Program Files\Unity\Hub\Editor\2022.3.8f1\Editor\Data\NetStandard\compat\2.1.0\shims\netstandard\System.Runtime.InteropServices.dll'
------------------
Résoudre : 'System.Runtime.CompilerServices.Unsafe, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null'
Introuvable par le nom : 'System.Runtime.CompilerServices.Unsafe, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null'
#endif
