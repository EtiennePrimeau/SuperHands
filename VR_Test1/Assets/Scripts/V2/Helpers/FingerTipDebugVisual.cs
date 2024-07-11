using UnityEngine;

public class FingerTipDebugVisual : MonoBehaviour
{
    
    public static FingerTipDebugVisual Instance;

    [SerializeField] private GameObject _indexCube;
    [SerializeField] private GameObject _middleCube;
    [SerializeField] private GameObject _ringCube;
    [SerializeField] private GameObject _pinkyCube;
    [SerializeField] private GameObject _thumbCube;



    private void Awake()
    {
        Instance = this;
    }


    public void ChangeDebugVisual(OVRSkeleton.BoneId bone, bool value)
    {
        if (bone == OVRSkeleton.BoneId.Hand_IndexTip)
        {
            Material mat = _indexCube.GetComponent<MeshRenderer>().material;
            if (value)
            {
                mat.color = Color.green;
            }
            else
            {
                mat.color = Color.red;  
            }
        }

        if (bone == OVRSkeleton.BoneId.Hand_MiddleTip)
        {
            Material mat = _middleCube.GetComponent<MeshRenderer>().material;
            if (value)
            {
                mat.color = Color.green;
            }
            else
            {
                mat.color = Color.red;  
            }
        }

        if (bone == OVRSkeleton.BoneId.Hand_RingTip)
        {
            Material mat = _ringCube.GetComponent<MeshRenderer>().material;
            if (value)
            {
                mat.color = Color.green;
            }
            else
            {
                mat.color = Color.red;  
            }
        }

        if (bone == OVRSkeleton.BoneId.Hand_PinkyTip)
        {
            Material mat = _pinkyCube.GetComponent<MeshRenderer>().material;
            if (value)
            {
                mat.color = Color.green;
            }
            else
            {
                mat.color = Color.red;  
            }
        }

        if (bone == OVRSkeleton.BoneId.Hand_ThumbTip)
        {
            Material mat = _thumbCube.GetComponent<MeshRenderer>().material;
            if (value)
            {
                mat.color = Color.green;
            }
            else
            {
                mat.color = Color.red;  
            }
        }
    }
}
