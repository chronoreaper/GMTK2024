using System.Collections;
using Cinemachine;
using UnityEngine;

public class SwitchMapView : MonoBehaviour
{
    [Header("View Cameras")]
    [SerializeField] private CinemachineVirtualCamera galaxyViewCamera;
    [SerializeField] private CinemachineVirtualCamera planetViewCamera;

    [Space]
    [SerializeField] private GameObject planetGround;

    public delegate void OnSwitchPlanetView();
    public static OnSwitchPlanetView onSwitchPlanetView;

    public delegate void OnSwitchGalaxyView();
    public static OnSwitchGalaxyView onSwitchGalaxyView;

    public void SwitchToPlanetView()
    {
        onSwitchPlanetView?.Invoke();

        galaxyViewCamera.gameObject.SetActive(false);

        planetViewCamera.gameObject.SetActive(true);
        planetGround.SetActive(true);
    }

    public void SwitchToGalaxyView()
    {
        onSwitchGalaxyView?.Invoke();
        
        StartCoroutine(GalaxyView());
    }

    private IEnumerator GalaxyView()
    {
        galaxyViewCamera.gameObject.SetActive(true);

        planetViewCamera.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        planetGround.SetActive(false);
    }
}
