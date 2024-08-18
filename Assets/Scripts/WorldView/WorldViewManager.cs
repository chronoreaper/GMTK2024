using UnityEngine;

public class WorldViewManager : MonoBehaviour
{
    public static WorldViewManager Instance { get; private set; }

    private Views _currentView;
    public Views CurrentView
    {
        get
        {
            return _currentView;
        }
        set
        {
            _currentView = value;

            onViewChanged?.Invoke(value);
        }
    }

    public delegate void OnViewChanged(Views newView);
    public OnViewChanged onViewChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        CurrentView = Views.GalaxyView;
    }
}
