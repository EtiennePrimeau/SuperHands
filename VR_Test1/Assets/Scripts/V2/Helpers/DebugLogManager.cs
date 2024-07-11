using TMPro;
using UnityEngine;

public class DebugLogManager : MonoBehaviour
{
    public static DebugLogManager Instance;


    [SerializeField] private GameObject _prefab;


    private void Awake()
    {
        Instance = this;
    }

    public void PrintLog(string str)
    {
        var go = Instantiate(_prefab, gameObject.transform);
        var tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = str;
    }
}
