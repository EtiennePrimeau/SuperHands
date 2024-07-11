using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrabbableTriggers : MonoBehaviour
{
    private bool _isGrabbed = false;
    private Vector3 _startPos;

    private List<Fingertip> _attachedFingertips = new List<Fingertip>();
    private FixedJoint _fixedJoint;
    private Rigidbody _rb;

    private bool _releaseTimerOn = false;
    private float _releaseTimer = 0f;

    private void Start()
    {
        _startPos = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = _startPos;
        }

    }

    private void FixedUpdate()
    {
        CheckForGrab();

        if (_releaseTimerOn)
        {
            _releaseTimer += Time.fixedDeltaTime;
        }
        else
        {
            _releaseTimer = 0f;
        }
            

    }

    private void OnTriggerEnter(Collider other)
    {
        Fingertip fingertip = other.gameObject.GetComponent<Fingertip>();
        if (fingertip == null)
            return;
    
        FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, true);
        _attachedFingertips.Add(fingertip);
    }


    private void OnTriggerExit(Collider other)
    {
        Fingertip fingertip = other.gameObject.GetComponent<Fingertip>();
        if (fingertip == null)
            return;

        FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, false);
        _attachedFingertips.Remove(fingertip);
    }


    private void CheckForGrab()
    {
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
            DebugLogManager.Instance.PrintLog("no thumb");
            CheckForRelease();
            return;
        }

        if (_isGrabbed)
        {
            _releaseTimerOn = false;
            return;
        }

        DebugLogManager.Instance.PrintLog("grabbing");

        _fixedJoint = _attachedFingertips[0].gameObject.AddComponent<FixedJoint>();
        //_fixedJoint = thumb.gameObject.AddComponent<FixedJoint>();
        _fixedJoint.connectedBody = _rb;
        _isGrabbed = true;
    }

    private void CheckForRelease()
    {
        if (!_isGrabbed)
            return;

        _releaseTimerOn = true;
        if (_releaseTimer < 0.5f)
        {
            DebugLogManager.Instance.PrintLog("waiting for releasing");
            return;
        }

        DebugLogManager.Instance.PrintLog("releasing");

        Destroy(_fixedJoint);
        _isGrabbed = false;
        _releaseTimerOn = false;
    }
}
