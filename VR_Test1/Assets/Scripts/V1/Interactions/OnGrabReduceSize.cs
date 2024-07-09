using Oculus.Interaction;
using System;
using UnityEngine;

public class OnGrabReduceSize : MonoBehaviour
{

    private bool _hasBeenGrabbed = false;
    [SerializeField] private float _scaleMultiplier = 1.1f;

    private Grabbable _grabbable;

    private void Start()
    {
        _grabbable = GetComponent<Grabbable>();
    }

    private void Update()
    {
        if (!_hasBeenGrabbed) return;

        ReduceSize();
    }

    public void OnSelect()
    {
        //Debug.Log("On Select");
        _hasBeenGrabbed = true;
    }

    public void OnUnselect()
    {
        //Debug.Log("On Unselect");
    }

    public void OnMove()    //kinda works like an Update
    {
        //Debug.Log("OnMove");

        ReduceSize();
    }

    private void ReduceSize()
    {
        //Debug.Log("reduce size");
        if (_scaleMultiplier != 0)
        {
            transform.root.localScale /= _scaleMultiplier;
        }

        if (transform.root.localScale.x < 0.01f)
        {
            Destroy(transform.root.gameObject);
        }


    }
}
