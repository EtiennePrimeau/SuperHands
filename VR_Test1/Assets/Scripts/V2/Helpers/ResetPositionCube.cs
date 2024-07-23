using UnityEngine;

public class ResetPositionCube : MonoBehaviour
{
    [SerializeField] private GrabbableObject cube1;
    [SerializeField] private GrabbableObject cube2;
    [SerializeField] private Material _material;

    private float _timer = 0f;
    private const float MAX_TIMER = 2f;



    private void OnTriggerStay(Collider other)
    {
        Fingertip fingertip = other.GetComponent<Fingertip>();
        if (fingertip == null)
            return;

        if (fingertip.BoneId != OVRSkeleton.BoneId.Hand_IndexTip)
            return;

        _timer += Time.deltaTime;
        _material.color = Color.Lerp(Color.red, Color.green, _timer / 2f);

        if (_timer > MAX_TIMER)
        {
            cube1.ResetPositionAndVelocity();
            cube2.ResetPositionAndVelocity();
            _timer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Fingertip fingertip = other.GetComponent<Fingertip>();
        if (fingertip == null)
            return;

        if (fingertip.BoneId != OVRSkeleton.BoneId.Hand_IndexTip)
            return;

        _timer = 0f;
        _material.color = Color.red;
    }
}
