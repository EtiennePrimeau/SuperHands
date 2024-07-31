using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeableReleaseTrigger : MonoBehaviour
{
    private TakeableObject _takeableObject;





    private void Start()
    {
        _takeableObject = GetComponentInParent<TakeableObject>();
        if (_takeableObject == null)
            DebugLogManager.Instance.PrintLog("takeable not found");
    }


    private void OnTriggerExit(Collider other)
    {
        Fingertip fingertip = other.gameObject.GetComponent<Fingertip>();
        if (fingertip == null)
            return;

        bool isLeftHand = (fingertip.Hand == OVRSkeleton.SkeletonType.HandLeft);

        _takeableObject.RemoveFingertipFromList(isLeftHand, fingertip);
    }

}
