using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetection : MonoBehaviour
{

    

    public void OnHover()
    {
        Debug.Log("onHover");
    }
    public void OnSelect()
    {
        Debug.Log("OnSelect");
    }
    public void OnViewAdded(IInteractorView interactorView)
    {
        Debug.Log("OnViewAdded   " + interactorView);

    }
    public void OnViewRemoved()
    {
        Debug.Log("OnViewRemoved");
    }
    public void OnSelectingViewAdded()
    {
        Debug.Log("OnSelectingViewAdded");
    }
    public void OnSelectingViewRemoved()
    {
        Debug.Log("OnSelectingViewRemoved");
    }
}
