using UnityEngine;

public class GrabbableReleaseTrigger : MonoBehaviour
{
    private GrabbableObject _grabbableObject;

    private void Start()
    {
        _grabbableObject = GetComponentInParent<GrabbableObject>();
        if (_grabbableObject == null)
            DebugLogManager.Instance.PrintLog("grabbable not found");
    }


    private void OnTriggerExit(Collider other)
    {
        Fingertip fingertip = other.gameObject.GetComponent<Fingertip>();
        if (fingertip == null)
            return;

        bool isLeftHand = (fingertip.Hand == OVRSkeleton.SkeletonType.HandLeft);

        _grabbableObject.RemoveFingertipFromList(isLeftHand, fingertip);
    }

}
