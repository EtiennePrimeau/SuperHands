using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class PainPointInteractions : MonoBehaviour
{



    public void OnSelect()
    {
        //Debug.Log("OnSelect");

        transform.SetParent(null);
    }
}
