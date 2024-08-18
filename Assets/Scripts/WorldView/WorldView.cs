using UnityEngine;

public abstract class WorldView : MonoBehaviour
{
    private void OnEnable()
    {
        WorldViewManager.Instance.onViewChanged += WorldViewChanged;
    }

    private void OnDisable()
    {
        WorldViewManager.Instance.onViewChanged -= WorldViewChanged;
    }

    protected abstract void WorldViewChanged(Views newView);

    protected void ChangeWorldViewToGalaxy()
    {
        if (WorldViewManager.Instance.CurrentView == Views.GalaxyView)
        {
            return;
        }

        WorldViewManager.Instance.CurrentView = Views.GalaxyView;
    }

    protected void ChangeWorldViewToPlanet()
    {
        if (WorldViewManager.Instance.CurrentView == Views.PlanetView)
        {
            return;
        }

        WorldViewManager.Instance.CurrentView = Views.PlanetView;
    }

    protected Views GetCurrentWorldView()
    {
        return WorldViewManager.Instance.CurrentView;
    }
}
