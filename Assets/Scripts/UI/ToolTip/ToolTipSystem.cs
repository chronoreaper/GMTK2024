using System;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem _instance;

    public static ToolTipSystem Instance
    {
        get => _instance;
        private set
        {
            if (_instance == null)
            {
                _instance = value;
            }

            if (_instance != value)
            {
                Destroy(value);
            }
        }
    }

    [SerializeField] private ToolTip toolTip;
    
    private void Awake() => Instance = this;

    public void Show(string content, string header = "")
    {
        toolTip.SetText(content, header);
        toolTip.gameObject.SetActive(true);
    }

    public void Hide() => toolTip.gameObject.SetActive(false);
}
