using System.Collections.Generic;
using UnityEngine;

public class GrabInteraction : MonoBehaviour
{
    [SerializeField] private Fingertip[] _fingertips = new Fingertip[5];
    [SerializeField] private GrabBox _grabBox;
    [SerializeField] private FixedJoint _fixedJoint;
    [SerializeField] private GameObject _palmCollider;
    [SerializeField] private float _colliderReactivationDelay = 0.5f;

    private List<GrabbableObject> _grabbedObjects = new List<GrabbableObject>();

    private bool _isGrabbing = false;
    private bool _isReactivatingColliders = false;
    private float _colliderTimer = 0f;

    private void Update()
    {
        HandleColliderTimer();  
    }

    private void HandleColliderTimer()
    {
        if (_isReactivatingColliders)
        {
            _colliderTimer += Time.deltaTime;
            if (_colliderTimer > _colliderReactivationDelay)
            {
                ToggleBoneColliders(true);
                _colliderTimer = 0f;
                _isReactivatingColliders = false;
            }
        }
    }

    private void FixedUpdate()
    {
        //DebugLogManager.Instance.PrintLog(_isGrabbing.ToString());
        if (!_isGrabbing)
        {
            foreach (var fingertip in _fingertips)
            {
                if (fingertip.IsGrabbing)
                {
                    //MultiGrab();
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
                    Release();
                    return;
                }
            }
        }
    }

    private void Grab()
    {
        GrabbableObject grabbable = _grabBox.ClosestGrabbable;
        if (grabbable == null)
            return;
        if (grabbable.IsAttached)
            return;


        _isGrabbing = true;

        ToggleBoneColliders(false);

        grabbable.Attach(_fixedJoint);

        _grabbedObjects.Clear();
        _grabbedObjects.Add(grabbable);
    }

    private void MultiGrab()
    {
        if (_grabBox.RegisteredGrabbables.Count == 0)
            return;

        _isGrabbing = true;
        
        ToggleBoneColliders(false);
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
        _isGrabbing = false;
        
        foreach (var grabbed in _grabbedObjects)
        {
            grabbed.Detach();

            //DebugLogManager.Instance.PrintLog(grabbed.gameObject.name + " is trying to detach");
        }
        //ToggleBoneColliders(true);
        _isReactivatingColliders = true;
        //DebugLogManager.Instance.PrintLog("reactivating colliders");
    }

    private void ToggleBoneColliders(bool value)
    {
        DebugLogManager.Instance.PrintLog("Toggling Bone Colliders : " + value);

        foreach (var fingertip in _fingertips)
        {
            fingertip.ToggleBoneCollider(value);
        }
        _palmCollider.SetActive(value);
    }
}
