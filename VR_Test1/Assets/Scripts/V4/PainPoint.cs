using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainPoint : GrabbableObject, IPainPointSpawnable
{
    private OVRSkeleton.SkeletonType _hand;
    private OVRSkeleton.BoneId _boneId;

    public void Init(OVRSkeleton.SkeletonType hand, OVRSkeleton.BoneId boneId)
    {
        _hand = hand;
        _boneId = boneId;


    }

    private void Update()
    {
        if (IsGrabbed)
        {
            transform.SetParent(null);
        }
    }
}
