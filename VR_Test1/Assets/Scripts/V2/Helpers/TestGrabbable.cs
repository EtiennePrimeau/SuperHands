using System.Collections.Generic;
using UnityEngine;

public class TestGrabbable : MonoBehaviour
{

    private bool _isGrabbed = false;
    private Vector3 _startPos;

    private List<Fingertip> _attachedFingertips = new List<Fingertip>();
    private FixedJoint _fixedJoint;
    private Rigidbody _rb;

    private void Start()
    {
        _startPos = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = _startPos;
        }

    }

    private void FixedUpdate()
    {
        CheckForGrab();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Fingertip fingertip = collision.gameObject.GetComponent<Fingertip>();
        if (fingertip == null)
            return;

        FingerTipDebugVisual.Instance.ChangeDebugVisual(fingertip.BoneId, true);
        _attachedFingertips.Add(fingertip);
    }


    private void OnCollisionExit(Collision collision)
    {
        Fingertip fingertip = collision.gameObject.GetComponent<Fingertip>();
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
        if (_isGrabbed)
        {
            return;
        }

        DebugLogManager.Instance.PrintLog("grabbing");

        _fixedJoint = _attachedFingertips[0].gameObject.AddComponent<FixedJoint>();
        _fixedJoint.connectedBody = _rb;
        _isGrabbed = true;
    }

    private void CheckForRelease()
    {
        if (!_isGrabbed)
            return;

        DebugLogManager.Instance.PrintLog("releasing");

        Destroy(_fixedJoint);
        _isGrabbed = false;
    }
}
