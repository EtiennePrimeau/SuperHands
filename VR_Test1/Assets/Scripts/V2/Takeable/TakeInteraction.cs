using System.Collections.Generic;
using UnityEngine;

public class TakeInteraction : MonoBehaviour
{
    [SerializeField] private Fingertip[] _fingertips = new Fingertip[5];
    [SerializeField] private TakeBox _takeBox;
    [SerializeField] private FixedJoint _fixedJoint;
    [SerializeField] private GameObject _palmCollider;
    [SerializeField] private float _colliderReactivationDelay = 0.5f;

    private bool _isTaking = false; // could be isHolding
    private bool _isReactivatingColliders = false;
    private float _colliderTimer = 0f;

    private TakeableObject _closestTakeable;
    private TakeableObject _currentlyHeldTakeable;

    private void Update()
    {
        HandleColliderTimer();
    }

    private void FixedUpdate()
    {
        if (_isTaking)
        {
            if (_currentlyHeldTakeable == null)
                DebugLogManager.Instance.PrintLog("No currently held object");

            CheckForRelease();
        }
        else
        {
            _closestTakeable = _takeBox.ClosestTakeable;
            CheckForTake();
        }
    }

    private void CheckForRelease()
    {
        foreach (var fingertip in _currentlyHeldTakeable.AttachedFingertips)
        {
            if (fingertip.IsReleasing)
            {
                _closestTakeable.Detach(fingertip);

                _currentlyHeldTakeable = null;
                _isTaking = false;
                _isReactivatingColliders = true;
                return;
            }
        }
    }

    private void CheckForTake()
    {
        if (_closestTakeable == null)
            return;

        //if (_closestTakeable.IsTaken)
        //    return;
        //if (!_closestTakeable.HasThumbAttached)
        //    return;
        //if (_closestTakeable.AttachedFingertips.Count < 2)
        //    return;
        if (!_closestTakeable.CanBeTaken())
            return;

        _isTaking = true;
        ToggleBoneColliders(false);

        _closestTakeable.Attach(_fixedJoint);
        _currentlyHeldTakeable = _closestTakeable;
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

    private void ToggleBoneColliders(bool value)
    {
        //DebugLogManager.Instance.PrintLog("Toggling Bone Colliders : " + value);

        foreach (var fingertip in _fingertips)
        {
            fingertip.ToggleBoneCollider(value);
        }
        _palmCollider.SetActive(value);
    }

}
