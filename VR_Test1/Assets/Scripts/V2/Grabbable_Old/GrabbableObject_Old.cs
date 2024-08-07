using UnityEngine;

public class GrabbableObject_Old : MonoBehaviour
{
    private FixedJoint _attachedFixedJoint;
    private Rigidbody _rb;
    private Vector3 _previousPos;
    private Vector3 _velocity;
    private Material _material;
    private Color _originalColor;
    private bool _isAttached = false;

    public bool IsAttached { get { return _isAttached; } }

    private Vector3 _startPos; // for debugging/resetting pos

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _material = GetComponent<MeshRenderer>().material;
        _originalColor = _material.color;

        _startPos = transform.position;
    }

    private void FixedUpdate()
    {
        _velocity = (transform.position - _previousPos) / Time.fixedDeltaTime;
        _previousPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetPositionAndVelocity();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GrabBox_Old grabBox = other.GetComponent<GrabBox_Old>();
        if (grabBox == null)
            return;

        grabBox.AddGrabbableObject(this);
    }

    private void OnTriggerExit(Collider other)
    {
        GrabBox_Old grabBox = other.GetComponent<GrabBox_Old>();
        if (grabBox == null)
            return;

        grabBox.RemoveGrabbableObject(this);
    }

    public void Attach(FixedJoint fixedJoint)
    {
        //DebugLogManager.Instance.PrintLog(gameObject.name + " is attaching");
        _isAttached = true;

        _attachedFixedJoint = fixedJoint;
        _attachedFixedJoint.connectedBody = _rb;
    }

    public void Detach()
    {
        //DebugLogManager.Instance.PrintLog(gameObject.name + " is detaching");
        _isAttached = false;

        //DebugLogManager.Instance.PrintLog(_velocity.ToString());
        _attachedFixedJoint.connectedBody = null;
        _attachedFixedJoint = null;

        _rb.velocity = _velocity;
    }

    public void HighlightAsGrabbable()
    {
        //DebugLogManager.Instance.PrintLog("Highlighting");

        //Color color = _material.color;
        //color.a = 0.1f;
        //_material.color = color;

        _material.color = Color.gray;
    }

    public void StopHighlight()
    {
        //DebugLogManager.Instance.PrintLog("stopping highlight");

        //Color color = _material.color;
        //color.a = 1f;
        //_material.color = color;

        _material.color = _originalColor;
    }

    public void ResetPositionAndVelocity()
    {
        transform.position = _startPos;
        _rb.velocity = Vector3.zero;
    }
}
