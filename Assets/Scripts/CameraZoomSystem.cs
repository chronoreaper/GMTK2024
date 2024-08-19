using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraZoomSystem : WorldView
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float zoomIncreaseAmount;
    [SerializeField] private float zoomSpeed;
    [SerializeField] CameraDragSystem cameraDragSystem;

    private float initialZoomAmount;
    private float targetZoomAmont;
    private Coroutine currentZoomCoroutine;
    private Coroutine currentCameraCoroutine;

    private void Start()
    {
        initialZoomAmount = virtualCamera.m_Lens.OrthographicSize;
        targetZoomAmont = initialZoomAmount - zoomIncreaseAmount;
    }

    private void OnEnable()
    {
        PlayerMouse.planetSelected += ZoomInToPlanet;
        PlayerMouse.planetDeselected += ZoomOutToGalaxy;
    }

    private void OnDisable()
    {
        PlayerMouse.planetSelected -= ZoomInToPlanet;
        PlayerMouse.planetDeselected -= ZoomOutToGalaxy;
    }

    private void ZoomInToPlanet(Vector2 planetLocation)
    {
        if (GetCurrentWorldView() == Views.PlanetView)
        {
            return;
        }

        if (currentCameraCoroutine != null)
        {
            StopCoroutine(currentCameraCoroutine);
        }
        currentCameraCoroutine = StartCoroutine(SmoothCameraMovementCorountine(planetLocation));
        
        if (currentZoomCoroutine != null)
        {
            StopCoroutine(currentZoomCoroutine);
        }
        currentZoomCoroutine = StartCoroutine(SmoothZoomCoroutine(targetZoomAmont));

        cameraDragSystem.LockDrag();

        ChangeWorldViewToPlanet();
    }

    private void ZoomOutToGalaxy()
    {
        if (GetCurrentWorldView() == Views.GalaxyView)
        {
            return;
        }

        if (currentCameraCoroutine != null)
        {
            StopCoroutine(currentCameraCoroutine);
        }

        if (currentZoomCoroutine != null)
        {
            StopCoroutine(currentZoomCoroutine);
        }

        currentZoomCoroutine = StartCoroutine(SmoothZoomCoroutine(initialZoomAmount));

        cameraDragSystem.UnlockDrag();

        ChangeWorldViewToGalaxy();
    }

    private IEnumerator SmoothZoomCoroutine(float zoomAmount)
    {
        float tolerance = 0.01f;
        while (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - zoomAmount) > tolerance)
        {
            virtualCamera.m_Lens.OrthographicSize =
                Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, zoomAmount, Time.deltaTime * zoomSpeed);

            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = zoomAmount;
    }

    private IEnumerator SmoothCameraMovementCorountine(Vector2 planetLocation)
    {
        float tolerance = 0.02f;

        while (Vector2.Distance(transform.position, planetLocation) > tolerance)
        {
            transform.position = 
                Vector2.Lerp(transform.position, planetLocation, Time.deltaTime * zoomSpeed); 
            yield return null;
        }

        currentCameraCoroutine = null;
    }

    protected override void WorldViewChanged(Views newView)
    {
        if (newView == Views.GalaxyView)
        {
            ZoomOutToGalaxy();
        }
    }
}
