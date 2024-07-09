using UnityEngine;

public class Fingertip : MonoBehaviour
{

    [SerializeField] private OVRSkeleton.SkeletonType _hand;
    [SerializeField] private OVRSkeleton.BoneId _boneId;

    public OVRSkeleton.SkeletonType Hand { get { return _hand; } }
    public OVRSkeleton.BoneId BoneId { get { return _boneId; } }


}
