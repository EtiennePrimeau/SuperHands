using UnityEngine;

public class PainPoint : GrabbableObject, IPainPointSpawnable
{
    private OVRSkeleton.SkeletonType _hand;
    private OVRSkeleton.BoneId _boneId;

    public void Init(OVRSkeleton.SkeletonType hand, OVRSkeleton.BoneId boneId)
    {
        _hand = hand;
        _boneId = boneId;

        GrabbableDebug.Instance.AssignGrabbableObject(this);
    }

    public override bool TryAttach(FixedJoint fixedJoint, EHandSide handSide)
    {
        //DebugLogManager.Instance.PrintLog("Try Attach - PP");

        if (GrabbingHandIsAttachedHand(handSide))
            return false;

        return base.TryAttach(fixedJoint, handSide);
    }

    public override void Detach(Fingertip releasingFingertip)
    {
        base.Detach(releasingFingertip);

        transform.SetParent(null);
    }

    public override void HighlightAsGrabbable(EHandSide handSide)
    {
        if (GrabbingHandIsAttachedHand(handSide))
            return;

        base.HighlightAsGrabbable(handSide);
    }

    public override void StopHighlight(EHandSide handSide)
    {
        if (GrabbingHandIsAttachedHand(handSide))
            return;
        
        base.StopHighlight(handSide);
    }

    private bool GrabbingHandIsAttachedHand(EHandSide handSide)
    {
        if (_hand == OVRSkeleton.SkeletonType.HandLeft && handSide == EHandSide.Left)
            return true;
        if (_hand == OVRSkeleton.SkeletonType.HandRight && handSide == EHandSide.Right)
            return true;

        return false;
    }
}
