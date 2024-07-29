using System.Collections.Generic;
using UnityEngine;

public class TakeInteraction : MonoBehaviour
{
    [SerializeField] private Fingertip[] _fingertips = new Fingertip[5];
    [SerializeField] private TakeBox _takeBox;
    [SerializeField] private GameObject _palmCollider;
    [SerializeField] private float _colliderReactivationDelay = 0.5f;

    private bool _isTaking = false;
    private bool _isReactivatingColliders = false;
    private float _colliderTimer = 0f;

    private void Update()
    {
        HandleColliderTimer();
    }

    private void FixedUpdate()
    {
        TakeableObject closestTakeable = _takeBox.ClosestTakeable;
        List<Fingertip> attachedFingertips = closestTakeable.AttachedFingertips;
        
        if (_isTaking)
        {
            foreach (var fingertip in attachedFingertips)
            {
                if (fingertip.IsReleasing)
                {
                    //closestTakeable.Detach();
                }
            }
        }
        else
        {
            if (!closestTakeable.HasThumb)
                return;
            if (attachedFingertips.Count < 2)
                return;

            //closestTakeable.Attach();
        }
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
        DebugLogManager.Instance.PrintLog("Toggling Bone Colliders : " + value);

        foreach (var fingertip in _fingertips)
        {
            fingertip.ToggleBoneCollider(value);
        }
        _palmCollider.SetActive(value);
    }

}
