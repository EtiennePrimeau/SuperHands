using UnityEngine;

public class PainPointV2_1 : MonoBehaviour
{

    private OVRSkeleton.SkeletonType _hand;
    private OVRSkeleton.BoneId _boneId;

    private Vector3 _startPos;
    private float _detachDistance = 0.12f;

    public void Init(OVRSkeleton.SkeletonType hand, OVRSkeleton.BoneId boneId)
    {
        _hand = hand;
        _boneId = boneId;
        _startPos = transform.position;
    }

    private void OnCollisionStay(Collision collision)
    {
        var fingertip = collision.gameObject.GetComponent<Fingertip>();
        if (fingertip == null)
            return;


        //if (fingertip.Hand == _hand)
        //{
        //    DebugLogManager.Instance.PrintLog("same hand");
        //}
        //if (fingertip.BoneId == _boneId)
        //{
        //    DebugLogManager.Instance.PrintLog("same bone");
        //}

        if (fingertip.Hand == _hand)
            return;


        //DebugLogManager.Instance.PrintLog(OVRSkeleton.BoneLabelFromBoneId(fingertip.Hand, fingertip.BoneId));
    }

    private void OnTriggerExit(Collider other)
    {
        transform.SetParent(null);
    }

    private void Update()
    {

        //DebugLogManager.Instance.PrintLog(Vector3.Distance(transform.position, _startPos).ToString());

        if (Vector3.Distance(transform.position, _startPos) > _detachDistance)
        {
            //DebugLogManager.Instance.PrintLog("detaching");
            //transform.SetParent(null);
        }
    }
}
