using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHitbox : MonoBehaviour
{




    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.name.Contains("2"))
            return;
        
        DebugLogManager.Instance.PrintLog(collision.gameObject.name);
    }

}
