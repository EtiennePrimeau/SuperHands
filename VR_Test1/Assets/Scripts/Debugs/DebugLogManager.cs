using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugLogManager : MonoBehaviour
{
    public static DebugLogManager Instance;

    //Basic log
    [SerializeField] private GameObject _logPrefab;
    [SerializeField] private Transform _logTransform;


    //Value tracking
    private TrackedValuesList _trackedValuesList = new TrackedValuesList();
    [SerializeField] private GameObject _trackedValuePrefab;
    [SerializeField] private Transform _trackedValueTransform;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _trackedValuesList.UpdateValues();
    }

    public void PrintLog(string str)
    {
        var go = Instantiate(_logPrefab, _logTransform);
        var tmp = go.GetComponent<TextMeshProUGUI>();

        //Sets the width of the logInstance to the width of its panel
        Vector2 size = _logTransform.gameObject.GetComponent<RectTransform>().sizeDelta;
        size.y = tmp.rectTransform.sizeDelta.y;
        tmp.rectTransform.sizeDelta = size;

        tmp.text = str;
    }

    public void TrackValue(string name, string value)
    {
        if (_trackedValuesList.IsTracking(name))
        {
            _trackedValuesList.UpdateValue(name, value);
            return;
        }

        GameObject go = Instantiate(_trackedValuePrefab, _trackedValueTransform);
        _trackedValuesList.AddValueToTrack(go, value, name);
    }
}

public class TrackedValuesList
{
    private class TrackedValue
    {
        public GameObject go;
        public TextMeshProUGUI tmp;
        public string value;
        public string name;

        public TrackedValue(GameObject _go, string _value, string _name)
        {
            go = _go;
            tmp = go.GetComponent<TextMeshProUGUI>();
            value = _value;
            name = _name;
        }

    }

    private List<TrackedValue> _trackedValues = new List<TrackedValue>();


    public bool IsTracking(string name)
    {
        foreach (var value in _trackedValues)
        {
            if (value.name == name)
                return true;
        }


        return false;
    }

    public void AddValueToTrack(GameObject go, string value, string name)
    {
        TrackedValue newTrackedValue = new TrackedValue(go, value, name);

        _trackedValues.Add(newTrackedValue);
    }

    public void UpdateValue(string name, string value)
    {
        foreach (var element in _trackedValues)
        {
            if (element.name == name)
            {
                element.value = value;
                return;
            }
        }

    }

    public void UpdateValues()
    {
        foreach (var item in _trackedValues)
        {
            item.tmp.text = item.name + " : " + item.value;
        }
    }
}
