using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorDetection : MonoBehaviour
{

    public void OnSelected()
    {
        Debug.Log("OnSelected");
        
        DestroyOnPoseDetection.OnPoseDetected.Invoke();
    }
}
