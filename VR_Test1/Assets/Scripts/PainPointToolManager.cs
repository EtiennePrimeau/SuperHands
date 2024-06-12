using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainPointToolManager : MonoBehaviour
{
    private static PainPointToolManager instance;

    public static PainPointToolManager Instance
    {
        get
        {
            if (instance != null) return instance;

            Debug.LogError("PainPointToolManager is null");
            return null;
        }
        
    }


    [SerializeField] private OVRHand _rightHand;
    [SerializeField] private OVRHand _leftHand;
    [SerializeField] private OVRSkeleton _rightSkeleton;
    [SerializeField] private OVRSkeleton _leftSkeleton;

    [SerializeField] private GameObject _painPointPrefab;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;
    }

    public void SpawnPainPoint(OVRSkeleton.SkeletonType hand, OVRSkeleton.BoneId boneId)
    {
        if (hand == OVRSkeleton.SkeletonType.HandRight && !_rightHand.IsTracked)
        {
            Debug.Log("right hand is not tracked");
            return;
        }
        if (hand == OVRSkeleton.SkeletonType.HandLeft && !_leftHand.IsTracked)
        {
            Debug.Log("left hand is not tracked");
            return;
        }


        IList<OVRBone> bones;
        switch (hand)
        {
            case OVRSkeleton.SkeletonType.HandLeft:
                bones = _leftSkeleton.Bones;
                break;
            case OVRSkeleton.SkeletonType.HandRight:
                bones = _rightSkeleton.Bones;
                break;
            case OVRSkeleton.SkeletonType.None:
            case OVRSkeleton.SkeletonType.Body:
            case OVRSkeleton.SkeletonType.FullBody:
            default:
                Debug.Log("SkeletonType is not a hand");
                bones = null;
                return;
        }


        foreach (var bone in bones)
        {
            if (bone.Id == boneId)
            {
                Instantiate(_painPointPrefab, bone.Transform);
            }
        }
    }
}
