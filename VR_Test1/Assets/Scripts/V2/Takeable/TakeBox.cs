using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeBox : MonoBehaviour
{
    private List<TakeableObject> _registeredTakeables = new List<TakeableObject>();
    public List<TakeableObject> RegisteredTakeables { get { return _registeredTakeables; } }

    private TakeableObject _closestTakeable;
    public TakeableObject ClosestTakeable { get { return _closestTakeable; } }

    private void FixedUpdate()
    {
        HighlightClosestTakeable();
    }

    private void HighlightClosestTakeable()
    {
        TakeableObject closestGrabbable = CalculateClosestTakeableObject();

        if (closestGrabbable == _closestTakeable)
            return;


        if (_closestTakeable != null)
        {
            //_closestTakeable.StopHighlight();
        }
        if (closestGrabbable != null)
        {
            //closestGrabbable.HighlightAsGrabbable();
        }
        _closestTakeable = closestGrabbable;
    }

    private TakeableObject CalculateClosestTakeableObject()
    {
        if (_registeredTakeables.Count == 0)
            return null;
        if (_registeredTakeables.Count == 1)
            return _registeredTakeables[0];


        TakeableObject closestTakeable = _registeredTakeables[0];
        foreach (var takeable in _registeredTakeables)
        {
            if (takeable == closestTakeable) continue;

            float distanceTakeable = Vector3.Distance(transform.position, takeable.transform.position);
            float distanceClosestTakeable = Vector3.Distance(transform.position, closestTakeable.transform.position);
            if (distanceTakeable < distanceClosestTakeable)
            {
                closestTakeable = takeable;
            }
        }
        return closestTakeable;
    }

    public void AddTakeableObject(TakeableObject takeableObject)
    {
        _registeredTakeables.Add(takeableObject);

        //DebugLogManager.Instance.PrintLog(takeableObject.gameObject.name + " was added");
    }

    public void RemoveTakeableObject(TakeableObject takeableObject)
    {
        _registeredTakeables.Remove(takeableObject);

        //DebugLogManager.Instance.PrintLog(takeableObject.gameObject.name + " was removed");

    }
}
