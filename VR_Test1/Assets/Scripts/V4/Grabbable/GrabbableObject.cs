using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public enum EHandSide
    { Left, Right, None }
    private EHandSide _preferredHand = EHandSide.None;
    public EHandSide PreferredHand { get { return _preferredHand; } }

    private Rigidbody _rb;
    private FixedJoint _fixedJoint;

    private List<Fingertip> _leftHandAttachedFingertips = new List<Fingertip>();
    private List<Fingertip> _rightHandAttachedFingertips = new List<Fingertip>();
    public List<Fingertip> AttachedFingertips { get { return PreferedAttachedFingertips(); } }

    #region GrabbableDebug
    /// <summary>
    /// Only used for Debug
    /// </summary>
    public List<Fingertip> LeftFingertips { get { return _leftHandAttachedFingertips; } }
    /// <summary>
    /// Only used for Debug
    /// </summary>
    public List<Fingertip> RightFingertips { get { return _rightHandAttachedFingertips; } }
    #endregion

    private bool _hasLeftThumbAttached = false;
    private bool _hasRightThumbAttached = false;

    private bool _isGrabbed = false;
    public bool IsGrabbed { get { return _isGrabbed; } }

    private Vector3 _velocity;
    private Vector3 _previousPosition;
    private Vector3 _startPos;

    private float _releaseTimer = 0f;
    private bool _releaseTimerOn = false;

    private Material _material;
    private Color _originalColor;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _startPos = transform.position;

        _material = GetComponent<MeshRenderer>().material;
        _originalColor = _material.color;
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
    }

    public virtual bool TryAttach(FixedJoint fixedJoint, EHandSide handSide)
    {
        //DebugLogManager.Instance.PrintLog("Try Attach - GO");
        
        if (AttachedFingertips.Count == 0)
            return false;

        Attach(fixedJoint, handSide);
        return true;
    }

    private void Attach(FixedJoint fixedJoint, EHandSide handSide)
    {
        //DebugLogManager.Instance.PrintLog("Attaching - GO");
        
        _fixedJoint = fixedJoint;
        _fixedJoint.connectedBody = _rb;
        _isGrabbed = true;
    }

    public virtual void Detach(Fingertip releasingFingertip)
    {
        //DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(releasingFingertip.Hand, releasingFingertip.BoneId) + " is releasing");

        if (_fixedJoint == null)
            return;

        _fixedJoint.connectedBody = null;
        _fixedJoint = null;
        _releaseTimerOn = true;

        ResetAttachedFingertips();
        _preferredHand = EHandSide.None;

        _rb.velocity = _velocity;
        //DebugLogManager.Instance.PrintLog(_velocity.magnitude.ToString());
    }

    private void CalculateVelocity()
    {
        _velocity = (transform.position - _previousPosition) / Time.fixedDeltaTime;
        _previousPosition = transform.position;
    }

    private void HandleReleaseTimer()
    {
        _releaseTimer += Time.fixedDeltaTime;

        if (_releaseTimer > 1f)
        {
            _isGrabbed = false;
            _releaseTimerOn = false;
            _releaseTimer = 0f;
        }
    }

    public bool CanBeGrabbed(EHandSide grabbingHand)
    {
        if (IsGrabbed)
            return false;

        _preferredHand = EHandSide.None;

        if (grabbingHand == EHandSide.Left && _hasLeftThumbAttached)
        {
            if (_leftHandAttachedFingertips.Count < 2)
                return false;

            _preferredHand = EHandSide.Left;
            return true;
        }

        if (grabbingHand == EHandSide.Right && _hasRightThumbAttached)
        {
            if (_rightHandAttachedFingertips.Count < 2)
                return false;

            _preferredHand = EHandSide.Right;
            return true;
        }

        return false;
    }

    public void ResetPositionAndVelocity()
    {
        transform.position = _startPos;
        _rb.velocity = Vector3.zero;
    }

    public virtual void HighlightAsGrabbable(EHandSide handSide)
    {
        _material.color = Color.gray;
    }

    public virtual void StopHighlight(EHandSide handSide)
    {
        _material.color = _originalColor;
    }

    #region Collisions
    private void OnCollisionEnter(Collision collision)
    {
        Fingertip fingertip = collision.gameObject.GetComponent<Fingertip>();
        if (fingertip == null)
            return;

        bool isLeftHand = (fingertip.Hand == OVRSkeleton.SkeletonType.HandLeft);

        if (fingertip.BoneId == OVRSkeleton.BoneId.Hand_ThumbTip)
        {
            SetHasThumbAttached(isLeftHand, true);
        }

        if (!AttachedFingertipsContains(isLeftHand, fingertip))
        {
            AddToAttachedFingertips(isLeftHand, fingertip);
        }
    }

    public void RemoveFingertipFromList(bool isLeftHand, Fingertip fingertip)
    {
        if (_isGrabbed)
            return;

        if (RemoveFromAttachedFingertips(isLeftHand, fingertip))
        {
            if (fingertip.BoneId == OVRSkeleton.BoneId.Hand_ThumbTip)
            {
                SetHasThumbAttached(isLeftHand, false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandHitBox handHB = other.GetComponent<HandHitBox>();
        if (handHB == null)
            return;

        handHB.AddGrabbableObject(this);
    }

    private void OnTriggerExit(Collider other)
    {
        HandHitBox handHB = other.GetComponent<HandHitBox>();
        if (handHB == null)
            return;

        handHB.RemoveGrabbableObject(this);
    }
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
            case EHandSide.Left:
                return _leftHandAttachedFingertips;
            case EHandSide.Right:
                return _rightHandAttachedFingertips;

            case EHandSide.None:
            default:
                return null;
        }
    }

    private void ResetAttachedFingertips()
    {
        switch (_preferredHand)
        {
            case EHandSide.Left:
                SetHasThumbAttached(true, false);
                ClearAttachedFingertips(true);

                break;
            case EHandSide.Right:
                SetHasThumbAttached(false, false);
                ClearAttachedFingertips(false);
                break;

            case EHandSide.None:
            default:
                break;
        }
    }

    #endregion
}
