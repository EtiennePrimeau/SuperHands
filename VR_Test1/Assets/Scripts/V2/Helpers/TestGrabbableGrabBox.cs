using UnityEngine;

public class TestGrabbableGrabBox : MonoBehaviour
{
    private FixedJoint _attachedFixedJoint;
    private Rigidbody _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GrabBox grabBox = other.GetComponent<GrabBox>();
        if (grabBox == null)
            return;

        //grabBox.AddGrabbableObject(this); //Modify GrabBox
    }

    private void OnTriggerExit(Collider other)
    {
        GrabBox grabBox = other.GetComponent<GrabBox>();
        if (grabBox == null)
            return;

        //grabBox.RemoveGrabbableObject(this); //Modify GrabBox
    }

    public void Attach(FixedJoint fixedJoint)
    {
        //DebugLogManager.Instance.PrintLog(gameObject.name + " is attaching");
        
        _attachedFixedJoint = fixedJoint;
        _attachedFixedJoint.connectedBody = _rb;
    }

    public void Detach()
    {
        //DebugLogManager.Instance.PrintLog(gameObject.name + " is detaching");

        _attachedFixedJoint.connectedBody = null;
        _attachedFixedJoint = null;
    }
}
