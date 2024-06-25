using UnityEngine;

public class DestroyIfSmallerThan : MonoBehaviour
{

    [SerializeField] private float _minimumScale = 0f;

    void Update()
    {
        if (transform.localScale.x < _minimumScale)
            Destroy(gameObject);
    }
}
