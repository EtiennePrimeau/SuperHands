using UnityEngine;

public class PainPointV2_1 : MonoBehaviour
{
    [SerializeField] private GameObject _triggerPrefab;
    [SerializeField] private GameObject _triggerPrefabRef;

    private OVRSkeleton.SkeletonType _hand;
    private OVRSkeleton.BoneId _boneId;


    public void Init(OVRSkeleton.SkeletonType hand, OVRSkeleton.BoneId boneId, Transform boneTransform)
    {
        _hand = hand;
        _boneId = boneId;

        _triggerPrefabRef = Instantiate(_triggerPrefab, boneTransform);
    }

    private void OnTriggerExit(Collider other)
    {
        transform.SetParent(null);
        Destroy(_triggerPrefabRef);
    }
}
