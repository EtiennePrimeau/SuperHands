using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeableObject : MonoBehaviour
{
    private Rigidbody _rb;
    
    private List<Fingertip> _attachedFingertips = new List<Fingertip>();

    private bool _hasThumbAttached = false;
    private FixedJoint _fixedJoint;
    private HandVelocity _handVelocity;

    private bool _isGrabbed = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_isGrabbed)
        {
            _rb.velocity = _handVelocity.Velocity;
            DebugLogManager.Instance.PrintLog("obj : " + _rb.velocity);

        }
        
        if (!_hasThumbAttached || _attachedFingertips.Count < 2)
        {
            if (_isGrabbed)
            {
                _fixedJoint.connectedBody = null;
                _fixedJoint = null;
                _handVelocity = null;
                DebugLogManager.Instance.PrintLog("releasing");
                _isGrabbed = false;
            }
            return;
        }



        //if (_isGrabbed)
        //{
        //    foreach (var finger in _attachedFingertips)
        //    {
        //        if (finger.IsReleasing)
        //        {
        //            _fixedJoint.connectedBody = null;
        //            _fixedJoint = null;
        //            _isGrabbed = false;
        //            DebugLogManager.Instance.PrintLog("releasing");
        //            return;
        //        }
        //    }
        //}

        if (!_isGrabbed)
        {
            _fixedJoint = _attachedFingertips[0].FixedJoint;
            _fixedJoint.connectedBody = _rb;
            _handVelocity = _fixedJoint.gameObject.GetComponent<HandVelocity>();
            _isGrabbed = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Fingertip fingertip = collision.gameObject.GetComponent<Fingertip>();
        if (fingertip == null)
            return;

        if (fingertip.BoneId == OVRSkeleton.BoneId.Hand_ThumbTip)
        {
            _hasThumbAttached = true;
        }

        if (!_attachedFingertips.Contains(fingertip))
        {
            _attachedFingertips.Add(fingertip);
            FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, true);
            //DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " Enter");

        }
        else
        {
            //Debug.Log(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " already in the list");
        }


    }

    public void RemoveFingertipFromList(Fingertip fingertip)
    {
        
        
        if (_attachedFingertips.Remove(fingertip))
        {
            FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, false);

            if (fingertip.BoneId == OVRSkeleton.BoneId.Hand_ThumbTip)
            {
                _hasThumbAttached = false;
            }

        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Fingertip fingertip = other.gameObject.GetComponent<Fingertip>();
    //    if (fingertip == null)
    //        return;
    //
    //    if (!_attachedFingertips.Contains(fingertip))
    //    {
    //        _attachedFingertips.Add(fingertip);
    //        FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, true);
    //        //DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " Enter");
    //
    //    }
    //    else
    //    {
    //        //Debug.Log(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " already in the list");
    //    }
    //
    //}


    //private void OnTriggerExit(Collider other)
    //{
    //    Fingertip fingertip = other.gameObject.GetComponent<Fingertip>();
    //    if (fingertip == null)
    //        return;
    //
    //    if (_attachedFingertips.Remove(fingertip))
    //    {
    //        //FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, false);
    //        //Debug.Log(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " Exit");
    //        DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " Exit");
    //
    //    }
    //    else
    //    {
    //        //Debug.Log(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " not removed");
    //    }
    //
    //}
}
