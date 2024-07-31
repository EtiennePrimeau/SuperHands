using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.Rendering.DebugUI;

public class TakeableObject : MonoBehaviour
{
    public enum EPreferedHand
    { Left, Right, None }
    
    
    
    
    private Rigidbody _rb;
    
    private List<Fingertip> _leftHandAttachedFingertips = new List<Fingertip>();
    private List<Fingertip> _rightHandAttachedFingertips = new List<Fingertip>();

    private bool _hasLeftThumbAttached = false;
    private bool _hasRightThumbAttached = false;

    private EPreferedHand _preferredHand = EPreferedHand.None;

    private FixedJoint _fixedJoint;
    private Vector3 _startPos;

    private bool _isTaken = false;

    private float _releaseTimer = 0f;
    private bool _releaseTimerOn = false;

    private Vector3 _velocity;
    private Vector3 _previousPosition;

    //public List<Fingertip> AttachedFingertips {  get { return _attachedFingertips; } }
    public List<Fingertip> AttachedFingertips {  get { return PreferedAttachedFingertips(); } }
    //public bool HasThumbAttached {  get { return _hasThumbAttached; } }

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

        DebugLogManager.Instance.TrackValue("Preferred Hand", _preferredHand.ToString());
        DebugLogManager.Instance.TrackValue("Left Thumb", _hasLeftThumbAttached.ToString());
        DebugLogManager.Instance.TrackValue("Left Fingertips", _leftHandAttachedFingertips.Count.ToString());
        DebugLogManager.Instance.TrackValue("Right Fingertips", _rightHandAttachedFingertips.Count.ToString());

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
        if (AttachedFingertips.Count == 0)
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

        ResetAttachedFingertips();
        _preferredHand = EPreferedHand.None;

        _rb.velocity = _velocity;
        //DebugLogManager.Instance.PrintLog(_velocity.magnitude.ToString());
    }

    private void ResetAttachedFingertips()
    {
        //foreach (var fingertip in _attachedFingertips)
        //{
        //    FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, false);
        //}

        switch (_preferredHand)
        {
            case EPreferedHand.Left:
                SetHasThumbAttached(true, false);
                ClearAttachedFingertips(true);

                break;
            case EPreferedHand.Right:
                SetHasThumbAttached(false, false);
                ClearAttachedFingertips(false);
                break;

            case EPreferedHand.None:
            default:
                break;
        }

        //_hasThumbAttached = false;
        //_attachedFingertips.Clear();
    }

    public bool CanBeTaken()
    {
        if (IsTaken)
            return false;
        //if (!HasThumbAttached)
        //    return false;
        //if (AttachedFingertips.Count < 2)
        //    return false;

        _preferredHand = EPreferedHand.None;

        if (_hasLeftThumbAttached)
        {
            if (_leftHandAttachedFingertips.Count < 2)
                return false;

            _preferredHand = EPreferedHand.Left;
            return true;
        }

        if (_hasRightThumbAttached)
        {
            if (_rightHandAttachedFingertips.Count < 2)
                return false;

            _preferredHand = EPreferedHand.Right;
            return true;
        }

        return false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Fingertip fingertip = collision.gameObject.GetComponent<Fingertip>();
        if (fingertip == null)
            return;

        bool isLeftHand = (fingertip.Hand == OVRSkeleton.SkeletonType.HandLeft);
        //DebugLogManager.Instance.TrackValue("OnColl IsLeft", isLeftHand.ToString());


        if (fingertip.BoneId == OVRSkeleton.BoneId.Hand_ThumbTip)
        {
            //_hasThumbAttached = true;
            SetHasThumbAttached(isLeftHand, true);
        }

        //if (!_attachedFingertips.Contains(fingertip))
        if (!AttachedFingertipsContains(isLeftHand, fingertip))
        {
            //_attachedFingertips.Add(fingertip);
            AddToAttachedFingertips(isLeftHand, fingertip);

            FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, true);
            //DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " Enter");
        }
        else
        {
            //Debug.Log(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId) + " already in the list");
        }
    }

    public void RemoveFingertipFromList(bool isLeftHand, Fingertip fingertip)
    {
        if (_isTaken)
            return;

        //if (_attachedFingertips.Remove(fingertip))
        if (RemoveFromAttachedFingertips(isLeftHand, fingertip))
        {
            FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, false);

            if (fingertip.BoneId == OVRSkeleton.BoneId.Hand_ThumbTip)
            {
                //_hasThumbAttached = false;
                SetHasThumbAttached(isLeftHand, false);
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



    #region OldVersion

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
    #endregion

    #region LeftRightSelectors

    private void SetHasThumbAttached(bool isLeft, bool value)
    {
        if (isLeft)
            _hasLeftThumbAttached = value;
        else
            _hasRightThumbAttached = value;
    }

    private bool AttachedFingertipsContains(bool isLeft, Fingertip fingertip)
    {
        if (isLeft)
            return _leftHandAttachedFingertips.Contains(fingertip);
        else
            return _rightHandAttachedFingertips.Contains(fingertip);
    }

    private void AddToAttachedFingertips(bool isLeft, Fingertip fingertip)
    {
        if (isLeft)
            _leftHandAttachedFingertips.Add(fingertip);
        else
            _rightHandAttachedFingertips.Add(fingertip);
    }

    private bool RemoveFromAttachedFingertips(bool isLeft, Fingertip fingertip)
    {
        if (isLeft)
            return _leftHandAttachedFingertips.Remove(fingertip);
        else
            return _rightHandAttachedFingertips.Remove(fingertip);
    }

    private void ClearAttachedFingertips(bool isLeft)
    {
        if (isLeft)
            _leftHandAttachedFingertips.Clear();
        else
            _rightHandAttachedFingertips.Clear();
    }

    private List<Fingertip> PreferedAttachedFingertips()
    {
        switch (_preferredHand)
        {
            case EPreferedHand.Left:
                return _leftHandAttachedFingertips;
            case EPreferedHand.Right:
                return _rightHandAttachedFingertips;

            case EPreferedHand.None:
            default:
                return null;
        }
    }

    #endregion
}
