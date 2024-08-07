using System.Collections.Generic;
using UnityEngine;

public class HandHitBox : MonoBehaviour
{
    [SerializeField] private GrabbableObject.EHandSide _handSide = GrabbableObject.EHandSide.None;

    private List<GrabbableObject> _registeredGrabbables = new List<GrabbableObject>();
    public List<GrabbableObject> RegisteredGrabbables { get { return _registeredGrabbables; } }

    private GrabbableObject _closestGrabbable;
    public GrabbableObject ClosestGrabbable { get { return _closestGrabbable; } }

    private void FixedUpdate()
    {
        HighlightClosestGrabbable();
    }

    private void HighlightClosestGrabbable()
    {
        GrabbableObject closestGrabbable = CalculateClosestGrabbableObject();

        //if (closestGrabbable == _closestGrabbable)
        //    return;


        if (_closestGrabbable != null)
        {
            _closestGrabbable.StopHighlight(_handSide);
        }
        if (closestGrabbable != null)
        {
            closestGrabbable.HighlightAsGrabbable(_handSide);
        }
        _closestGrabbable = closestGrabbable;
    }

    private GrabbableObject CalculateClosestGrabbableObject()
    {
        if (_registeredGrabbables.Count == 0)
            return null;
        if (_registeredGrabbables.Count == 1)
            return _registeredGrabbables[0];


        GrabbableObject closestGrabbable = _registeredGrabbables[0];
        foreach (var grabbable in _registeredGrabbables)
        {
            if (grabbable == closestGrabbable) continue;

            float distanceGrabbable = Vector3.Distance(transform.position, grabbable.transform.position);
            float distanceClosestGrabbable = Vector3.Distance(transform.position, closestGrabbable.transform.position);
            if (distanceGrabbable < distanceClosestGrabbable)
            {
                closestGrabbable = grabbable;
            }
        }
        return closestGrabbable;
    }

    public void AddGrabbableObject(GrabbableObject grabbableObject)
    {
        _registeredGrabbables.Add(grabbableObject);

        //DebugLogManager.Instance.PrintLog(takeableObject.gameObject.name + " was added");
    }

    public void RemoveGrabbableObject(GrabbableObject grabbableObject)
    {
        _registeredGrabbables.Remove(grabbableObject);

        //DebugLogManager.Instance.PrintLog(takeableObject.gameObject.name + " was removed");

    }
}
