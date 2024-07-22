using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBox : MonoBehaviour
{
    private List<GrabbableObject> _registeredGrabbables = new List<GrabbableObject>();
    public List<GrabbableObject> RegisteredGrabbables { get { return _registeredGrabbables; } }

    private GrabbableObject _closestGrabbable;

    private void FixedUpdate()
    {
        //if (_registeredGrabbables.Count == 0)
        //    return;

        //DebugLogManager.Instance.PrintLog(_registeredGrabbables.Count.ToString());


        HighlightClosestGrabbable();
    }

    private void HighlightClosestGrabbable()
    {
        GrabbableObject closestGrabbable = CalculateClosestGrabbableObject();

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
            if(distanceGrabbable < distanceClosestGrabbable)
            {
                closestGrabbable = grabbable;
            }
        }
        return closestGrabbable;
    }

    public void AddGrabbableObject(GrabbableObject grabbable)
    {
        _registeredGrabbables.Add(grabbable);

        //DebugLogManager.Instance.PrintLog(grabbable.gameObject.name + " was added");
    }

    public void RemoveGrabbableObject(GrabbableObject grabbable)
    {
        _registeredGrabbables.Remove(grabbable);

        //DebugLogManager.Instance.PrintLog(grabbable.gameObject.name + " was removed");

    }
}
