using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeDetection : MonoBehaviour
{
    // needs to be used with Active State Unity Event Wrapper
        // see PoseDetection1

    public void ShapeDetectionActivated()
    {
        Debug.Log("shape detected");
    }

    public void ShapeDetectionDeactivated()
    {
        Debug.Log("shape not detected anymore");
    }

}
