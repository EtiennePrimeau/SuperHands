using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float _time = 0f;
    private float _timer = 0f;


    void Update()
    {
        if (_timer > _time)
        {
            Destroy(gameObject);
            return;
        }

        _timer += Time.deltaTime;
    }
}
