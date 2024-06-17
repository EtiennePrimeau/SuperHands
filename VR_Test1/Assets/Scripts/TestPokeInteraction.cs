using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPokeInteraction : MonoBehaviour
{


    private MeshRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponentInParent<MeshRenderer>();
    }

    public void WhenBeginHighlight()
    {
        Debug.Log("When Begin Highlight");
    }
    public void WhenSelectedHovered()
    {
        Debug.Log("When Selected Hovered");
    }
    public void WhenSelectedEmpty()
    {
        //Debug.Log("When Selected Empty");
        //Celui qui fonctionne avec le setup de base

        Color color = Random.ColorHSV();
        _renderer.material.color = color;

        
    }

    public void WhenUnselectedEmpty()
    {
        transform.root.localScale /= 2;
    }
}
