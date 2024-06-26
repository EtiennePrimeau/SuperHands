using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyOnPoseDetection : PainPointInteractions
{
    public static UnityEvent OnPoseDetected = new UnityEvent();

    private bool _isGrabbed = false;


    private void OnEnable()
    {
        Debug.Log("OnEnable");
        OnPoseDetected.AddListener(DestroyOnPoseDetected);
    }

    private void DestroyOnPoseDetected()
    {
        Debug.Log("Event called");
        if (_isGrabbed)
        {
            Destroy(transform.root.gameObject);
        }
    }

    public override void OnSelect()
    {
        base.OnSelect();

        _isGrabbed = true;
    }

    public void OnUnselect()
    {
        _isGrabbed = false; 
    }
}
