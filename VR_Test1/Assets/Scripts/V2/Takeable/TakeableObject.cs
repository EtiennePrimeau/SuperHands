using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeableObject : MonoBehaviour
{
    private Rigidbody _rb;
    
    private List<Fingertip> _attachedFingertips = new List<Fingertip>();

    private bool _hasThumbAttached = false;
    private FixedJoint _fixedJoint;
    private Vector3 _startPos;

    private bool _isTaken = false;

    private float _releaseTimer = 0f;
    private bool _releaseTimerOn = false;

    private Vector3 _velocity;
    private Vector3 _previousPosition;

    public List<Fingertip> AttachedFingertips {  get { return _attachedFingertips; } }
    public bool HasThumbAttached {  get { return _hasThumbAttached; } }

    public bool IsTaken { get { return _isTaken; } }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _startPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.velocity = Vector3.zero;
            transform.position = _startPos;
        }
    }

    private void FixedUpdate()
    {
        CalculateVelocity();

        if (_releaseTimerOn)
        {
            HandleReleaseTimer();
        }

        //if (_isGrabbed)
        //{
        //    CheckForRelease();
        //}
        //else
        //{
        //    CheckForGrab();
        //}    
    }

    private void CalculateVelocity()
    {
        _velocity = (transform.position - _previousPosition) / Time.fixedDeltaTime;
        _previousPosition = transform.position;
        
        //DebugLogManager.Instance.PrintLog(_velocity.magnitude.ToString());
    }

    private void HandleReleaseTimer()
    {
        _releaseTimer += Time.fixedDeltaTime;

        if (_releaseTimer > 1f)
        {
            _isTaken = false;
            _releaseTimerOn = false;
            _releaseTimer = 0f;
        }
    }

    public void Attach(FixedJoint fixedJoint)
    {
        if (_attachedFingertips.Count == 0)
            return;
        
        //_fixedJoint = _attachedFingertips[0].FixedJoint;
        _fixedJoint = fixedJoint;
        _fixedJoint.connectedBody = _rb;
        _isTaken = true;
    }

    public void Detach(Fingertip releasingFingertip)
    {
        DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(releasingFingertip.Hand, releasingFingertip.BoneId) + " is releasing");
        
        if (_fixedJoint == null)
            return;
        
        _fixedJoint.connectedBody = null;
        _fixedJoint = null;
        _releaseTimerOn = true;

        //_attachedFingertips.Clear();
        ResetAttachedFingertips();

        _rb.velocity = _velocity;
        DebugLogManager.Instance.PrintLog(_velocity.magnitude.ToString());
    }

    private void ResetAttachedFingertips()
    {
        //foreach (var fingertip in _attachedFingertips)
        //{
        //    FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, false);
        //}

        _hasThumbAttached = false;
        _attachedFingertips.Clear();
    }

    //private void CheckForRelease()
    //{
    //    foreach (var fingertip in _attachedFingertips)
    //    {
    //        if (fingertip.IsReleasing)
    //        {
    //            Release(fingertip);
    //            return;
    //        }
    //    }
    //}

    //private void Release(Fingertip releasingFingertip)
    //{
    //    DebugLogManager.Instance.PrintLog(_attachedFingertips.Count + OVRSkeleton.BoneLabelFromBoneId(releasingFingertip.Hand, releasingFingertip.BoneId) + " is releasing");
    //
    //    if (_fixedJoint == null)
    //        return;
    //
    //    _fixedJoint.connectedBody = null;
    //    _fixedJoint = null;
    //    _releaseTimerOn = true;
    //    _rb.velocity = _velocity;
    //}

    //private void CheckForGrab()
    //{
    //    if (!_hasThumbAttached || _attachedFingertips.Count < 2)
    //        return;
    //
    //    DebugLogManager.Instance.PrintLog("attaching");
    //
    //    Grab();
    //}

    //private void Grab()
    //{
    //    _fixedJoint = _attachedFingertips[0].FixedJoint;
    //    _fixedJoint.connectedBody = _rb;
    //    _isGrabbed = true;
    //}

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
            //FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, true);
            //DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " Enter");
        }
        else
        {
            //Debug.Log(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " already in the list");
        }
    }

    public void RemoveFingertipFromList(Fingertip fingertip)
    {
        if (_isTaken)
            return;
        
        if (_attachedFingertips.Remove(fingertip))
        {
            //FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, false);

            if (fingertip.BoneId == OVRSkeleton.BoneId.Hand_ThumbTip)
            {
                _hasThumbAttached = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TakeBox takeBox = other.GetComponent<TakeBox>();
        if (takeBox == null)
            return;

        takeBox.AddTakeableObject(this);
    }

    private void OnTriggerExit(Collider other)
    {
        TakeBox takeBox = other.GetComponent<TakeBox>();
        if (takeBox == null)
            return;

        takeBox.RemoveTakeableObject(this);
    }
}
