using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    private FixedJoint _attachedFixedJoint;
    private Rigidbody _rb;
    private Vector3 _previousPos;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _previousPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        GrabBox grabBox = other.GetComponent<GrabBox>();
        if (grabBox == null)
            return;

        grabBox.AddGrabbableObject(this);
    }

    private void OnTriggerExit(Collider other)
    {
        GrabBox grabBox = other.GetComponent<GrabBox>();
        if (grabBox == null)
            return;

        grabBox.RemoveGrabbableObject(this);
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

        Vector3 velocity = (transform.position - _previousPos) / Time.fixedDeltaTime;

        _attachedFixedJoint.connectedBody = null;
        _attachedFixedJoint = null;

        _rb.velocity = velocity;
    }
}
