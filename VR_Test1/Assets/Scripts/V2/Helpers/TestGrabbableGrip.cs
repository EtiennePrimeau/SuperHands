using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrabbableGrip : MonoBehaviour
{
    private bool _isGrabbed = false;

    private List<Fingertip> _attachedFingertips = new List<Fingertip>();
    private FixedJoint _fixedJoint;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        //DebugLogManager.Instance.PrintLog(_attachedFingertips.Count.ToString());

        //CheckForGrab();

        bool tryingToGrab = false;
        bool tryingToRelease = false;

        Fingertip thumb = null;
        foreach (var fingertip in _attachedFingertips)
        {
            if (fingertip.BoneId == OVRSkeleton.BoneId.Hand_ThumbTip)
            {
                thumb = fingertip;
            }

            if (fingertip.IsGrabbing)
            {
                tryingToGrab = true;
            }
            if (fingertip.IsReleasing)
            {
                tryingToRelease = true;
            }
        }

        if (thumb == null)
        {
            tryingToRelease = true;
        }

        if (!_isGrabbed && tryingToGrab)
        {
            if (thumb != null)
            {
                DebugLogManager.Instance.PrintLog("grabbing");

                _fixedJoint = thumb.FixedJoint;
                thumb.FixedJoint.connectedBody = _rb;
                _isGrabbed = true;
                return;
            }
        }

        if (_isGrabbed && tryingToRelease)
        {
            DebugLogManager.Instance.PrintLog("releasing");

            _fixedJoint.connectedBody = null;
            _fixedJoint = null;

            _isGrabbed = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Fingertip fingertip = other.gameObject.GetComponent<Fingertip>();
        if (fingertip == null)
            return;

        if (!_attachedFingertips.Contains(fingertip))
        {
            _attachedFingertips.Add(fingertip);
            //FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, true);
            //Debug.Log(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " Enter");
            //DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " Enter");

        }
        else
        {
            //Debug.Log(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " already in the list");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Fingertip fingertip = other.gameObject.GetComponent<Fingertip>();
        if (fingertip == null)
            return;

        if (_attachedFingertips.Remove(fingertip))
        {
            //FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, false);
            //Debug.Log(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " Exit");
            //DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " Exit");

        }
        else
        {
            //Debug.Log(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " not removed");
        }

    }

    private void CheckForGrab()
    {
        //DebugLogManager.Instance.PrintLog(_attachedFingertips.Count.ToString());

        if (_attachedFingertips.Count < 2)
        {
            CheckForRelease();
            return;
        }

        Fingertip thumb = null;
        foreach (var fingertip in _attachedFingertips)
        {
            if (fingertip.BoneId == OVRSkeleton.BoneId.Hand_ThumbTip)
            {
                thumb = fingertip;
                break;
            }
        }
        if (thumb == null)
        {
            //DebugLogManager.Instance.PrintLog("no thumb");
            CheckForRelease();
            return;
        }

        if (_isGrabbed)
        {
            return;
        }

        DebugLogManager.Instance.PrintLog("grabbing");

        _fixedJoint = thumb.FixedJoint;
        thumb.FixedJoint.connectedBody = _rb;
        _isGrabbed = true;
    }

    private void CheckForRelease()
    {
        if (!_isGrabbed)
            return;

        DebugLogManager.Instance.PrintLog("releasing");

        _fixedJoint.connectedBody = null;
        _fixedJoint = null;

        _isGrabbed = false;
    }

}
