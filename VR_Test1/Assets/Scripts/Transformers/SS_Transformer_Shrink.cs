using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_Transformer_Shrink : SS_BaseTransformer
{

    [SerializeField] private float _shrinkSpeed = 1f;

    public override void UpdateTransform()
    {
        base.UpdateTransform();

        transform.root.localScale *= _shrinkSpeed;
    }

}

