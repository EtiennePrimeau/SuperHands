using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBox_Old : MonoBehaviour
{
    private List<GrabbableObject_Old> _registeredGrabbables = new List<GrabbableObject_Old>();
    public List<GrabbableObject_Old> RegisteredGrabbables { get { return _registeredGrabbables; } }

    private GrabbableObject_Old _closestGrabbable;
    public GrabbableObject_Old ClosestGrabbable { get { return _closestGrabbable; } }

    private void FixedUpdate()
    {
        HighlightClosestGrabbable();
    }

    private void HighlightClosestGrabbable()
    {
        GrabbableObject_Old closestGrabbable = CalculateClosestGrabbableObject();

        if (closestGrabbable == _closestGrabbable)
            return;


        if (_closestGrabbable != null)
        {
            _closestGrabbable.StopHighlight();
        }
        if (closestGrabbable != null)
        {
            closestGrabbable.HighlightAsGrabbable();
        }
        _closestGrabbable = closestGrabbable;
    }

    private GrabbableObject_Old CalculateClosestGrabbableObject()
    {
        if (_registeredGrabbables.Count == 0)
            return null;
        if (_registeredGrabbables.Count == 1)
            return _registeredGrabbables[0];


        GrabbableObject_Old closestGrabbable = _registeredGrabbables[0];
        foreach (var grabbable in _registeredGrabbables)
        {
            if (grabbable == closestGrabbable) continue;

            float distanceGrabbable = Vector3.Distance(transform.position, grabbable.transform.position);
            float distanceClosestGrabbable = Vector3.Distance(transform.position, closestGrabbable.transform.position);
            if(distanceGrabbable < distanceClosestGrabbable)
            {
                closestGrabbable = grabbable;
            }
        }
        return closestGrabbable;
    }

    public void AddGrabbableObject(GrabbableObject_Old grabbable)
    {
        _registeredGrabbables.Add(grabbable);

        //DebugLogManager.Instance.PrintLog(grabbable.gameObject.name + " was added");
    }

    public void RemoveGrabbableObject(GrabbableObject_Old grabbable)
    {
        _registeredGrabbables.Remove(grabbable);

        //DebugLogManager.Instance.PrintLog(grabbable.gameObject.name + " was removed");

    }
}
