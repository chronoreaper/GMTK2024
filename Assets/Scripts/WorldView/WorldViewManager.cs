using UnityEngine;

public class WorldViewManager : MonoBehaviour
{
    public static WorldViewManager Instance { get; private set; }

    private Views _currentView = Views.GalaxyView;
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
    public static OnViewChanged onViewChanged;

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
}
