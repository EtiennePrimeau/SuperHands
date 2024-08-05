using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrabbableDebug : MonoBehaviour
{
    #region Singleton
    private static GrabbableDebug instance;
    public static GrabbableDebug Instance
    {
        get
        {
            if (instance != null) return instance;

            Debug.LogError("GrabbableDebug is null");
            return null;
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;
    }
    #endregion

    private enum EFingers
    { Thumb, Index, Middle, Ring, Pinky }

    [SerializeField] private GrabbableObject _grabbableObject;

    [SerializeField] private Image[] _leftHand = new Image[5];
    [SerializeField] private Image[] _rightHand = new Image[5];

    [SerializeField] private TextMeshProUGUI _preferredHandTMP;

    private List<Fingertip> _leftHandFingertips = new List<Fingertip>();
    private List<Fingertip> _rightHandFingertips = new List<Fingertip>();

    private GrabbableObject.EHandSide _preferredHand;




    private void Start()
    {
        _leftHandFingertips = _grabbableObject.LeftFingertips;
        _rightHandFingertips = _grabbableObject.RightFingertips;

        _preferredHand = _grabbableObject.PreferredHand;
    }

    private void Update()
    {
        UpdateImages(_leftHand, _leftHandFingertips);
        UpdateImages(_rightHand, _rightHandFingertips);

        _preferredHandTMP.text = "Preferred Hand : " + _grabbableObject.PreferredHand.ToString();
    }

    private void UpdateImages(Image[] hand, List<Fingertip> fingertips)
    {
        foreach (var image in hand)
        {
            image.color = Color.red;
        }
        
        foreach (var fingertip in fingertips)
        {
            switch (fingertip.BoneId)
            {
                case OVRSkeleton.BoneId.Hand_ThumbTip:
                    hand[(int)EFingers.Thumb].color = Color.green;
                    break;
                case OVRSkeleton.BoneId.Hand_IndexTip:
                    hand[(int)EFingers.Index].color = Color.green;
                    break;
                case OVRSkeleton.BoneId.Hand_MiddleTip:
                    hand[(int)EFingers.Middle].color = Color.green;
                    break;
                case OVRSkeleton.BoneId.Hand_RingTip:
                    hand[(int)EFingers.Ring].color = Color.green;
                    break;
                case OVRSkeleton.BoneId.Hand_PinkyTip:
                    hand[(int)EFingers.Pinky].color = Color.green;
                    break;
                default:
                    break;
            }
        }
    }

}
