using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBox : MonoBehaviour
{
    private List<GrabbableObject> _registeredGrabbables = new List<GrabbableObject>();
    public List<GrabbableObject> RegisteredGrabbables { get { return _registeredGrabbables; } }

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
