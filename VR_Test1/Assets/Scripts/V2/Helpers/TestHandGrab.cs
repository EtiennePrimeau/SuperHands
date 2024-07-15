using System.Collections.Generic;
using UnityEngine;

public class TestHandGrab : MonoBehaviour
{

    public enum EFingers { Thumb, Index, Middle, Ring, Pinky }


    [SerializeField] private Fingertip[] _fingers = new Fingertip[5];



    private void FixedUpdate()
    {
        foreach (var finger in _fingers)
        {
            if (finger.IsGrabbing)
            {
                DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(finger.Hand, finger.BoneId) + " is grabbing");
            }

            if (finger.IsReleasing)
            {
                DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(finger.Hand, finger.BoneId) + " is releasing");
            }
        }

        //_fingers[1].IsGrabbing();
    }
}
