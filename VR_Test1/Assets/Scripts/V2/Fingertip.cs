using UnityEngine;

public class Fingertip : MonoBehaviour
{

    [SerializeField] private OVRSkeleton.SkeletonType _hand;
    [SerializeField] private OVRSkeleton.BoneId _boneId;
    [SerializeField] private FixedJoint _fixedJoint;
    [SerializeField] private Transform _palmPoint;
    [SerializeField] private GameObject _boneCollider;

    public OVRSkeleton.SkeletonType Hand { get { return _hand; } }
    public OVRSkeleton.BoneId BoneId { get { return _boneId; } }
    public FixedJoint FixedJoint { get { return _fixedJoint; } }
    public bool IsGrabbing { get { return _isGrabbing; } }
    public bool IsReleasing { get { return _isReleasing; } }
    

    private float _previousDistance = 0f;
    private const float THRESHOLD = 12f;
    private const float MIN_DIST = 0.04f;
    private const float MAX_DIST = 0.12f;
    private const float RANGE = MAX_DIST - MIN_DIST;

    private bool _isGrabbing = false;
    private bool _isReleasing = false;

    private void FixedUpdate()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        _isGrabbing = false;
        _isReleasing = false;
        
        // Calculate absolute distance between fingertip and center of hand (palm)
        float distance = Vector3.Distance(transform.position, _palmPoint.position);
        float frameDistance = Mathf.Abs(_previousDistance - distance);

        // Checks if finger is moving towards palm (aka closing)
        bool isClosing = false;
        if (distance < _previousDistance) { isClosing = true; }

        _previousDistance = distance;

        // Ratio is the % of movement in a frame for the full range potential (finger-palm)
        float ratio = frameDistance / RANGE * 100f;
        if (ratio > THRESHOLD)
        {
            if (isClosing)
            {
                _isGrabbing = true;
                
                //DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(Hand, BoneId) + " is grabbing");
                //DebugLogManager.Instance.PrintLog(ratio.ToString());
            }
            else
            {
                _isReleasing = true;

                //DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(Hand, BoneId) + " is releasing");
            }
        }


    }

    public void ToggleBoneCollider(bool value)
    {
        if(_boneCollider == null)
            return;

        _boneCollider.SetActive(value);
    }
}
