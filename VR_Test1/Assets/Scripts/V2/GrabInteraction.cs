using System.Collections.Generic;
using UnityEngine;

public class GrabInteraction : MonoBehaviour
{
    [SerializeField] private Fingertip[] _fingertips = new Fingertip[5];
    [SerializeField] private GrabBox _grabBox;
    [SerializeField] private FixedJoint _fixedJoint;

    private List<GrabbableObject> _grabbedObjects = new List<GrabbableObject>();

    private bool _isGrabbing = false;


    private void FixedUpdate()
    {
        //DebugLogManager.Instance.PrintLog(_isGrabbing.ToString());
        if (!_isGrabbing)
        {
            foreach (var fingertip in _fingertips)
            {
                if (fingertip.IsGrabbing)
                {
                    _isGrabbing = true;
                    Grab();
                    return;
                }
            }
        }

        if (_isGrabbing)
        {
            foreach (var fingertip in _fingertips)
            {
                if (fingertip.IsReleasing)
                {
                    //DebugLogManager.Instance.PrintLog(fingertip.gameObject.name + " is releasing");
                    
                    _isGrabbing = false;
                    Release();
                    return;
                }
            }
        }
    }

    private void Grab()
    {
        foreach (var grabbable in _grabBox.RegisteredGrabbables)
        {
            grabbable.Attach(_fixedJoint);

            //DebugLogManager.Instance.PrintLog(grabbable.gameObject.name + " is trying to attach");
        }
        _grabbedObjects = _grabBox.RegisteredGrabbables;

        //DebugLogManager.Instance.PrintLog("GrabbedObjects are : ");
        //foreach (var obj in _grabbedObjects)
        //{
        //    DebugLogManager.Instance.PrintLog(obj.gameObject.name);
        //}
    }

    private void Release()
    {
        foreach (var grabbed in _grabbedObjects)
        {
            grabbed.Detach();

            //DebugLogManager.Instance.PrintLog(grabbed.gameObject.name + " is trying to detach");
        }
    }
}
