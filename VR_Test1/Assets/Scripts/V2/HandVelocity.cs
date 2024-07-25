using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVelocity : MonoBehaviour
{
    private Vector3 _velocity;
    private Vector3 _previousPosition;

    public Vector3 Velocity { get { return _velocity; } }
    private void FixedUpdate()
    {
        _velocity = (transform.position - _previousPosition) / Time.fixedDeltaTime;
        _previousPosition = transform.position;
        //DebugLogManager.Instance.PrintLog("hand : " + _velocity);
    }
}
