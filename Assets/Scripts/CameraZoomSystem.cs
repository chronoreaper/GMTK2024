using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoomSystem : WorldView
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Galaxy / Planet Zoom")]
    [SerializeField] private float zoomIncreaseAmount;
    [SerializeField] private float zoomSpeed;
    [SerializeField] CameraDragSystem cameraDragSystem;

    private float initialZoomAmount;
    private float targetZoomAmont;
    private Coroutine currentZoomCoroutine;
    private Coroutine currentCameraCoroutine;


    private float maxZoomDistance;
    private float minZoomDistance;

    private PlayerControls _playerControls;
    private InputAction _scrollWheel;
    private InputAction _cursorPosition;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void Start()
    {
        initialZoomAmount = virtualCamera.m_Lens.OrthographicSize;
        targetZoomAmont = initialZoomAmount - zoomIncreaseAmount;

        maxZoomDistance = initialZoomAmount * 2;
        minZoomDistance = targetZoomAmont;
    }

    private void OnEnable()
    {
        PlayerMouse.planetSelected += ZoomInToPlanet;
        PlayerMouse.planetDeselected += ZoomOutToGalaxy;
        
        _scrollWheel = _playerControls.Player.ScrollWheel;
        _scrollWheel.Enable();
        _scrollWheel.performed += ZoomInOut;

        _cursorPosition = _playerControls.Player.CusorPosition;
        _cursorPosition.Enable();
    }

    private void OnDisable()
    {
        PlayerMouse.planetSelected -= ZoomInToPlanet;
        PlayerMouse.planetDeselected -= ZoomOutToGalaxy;

        _scrollWheel.Disable();
        _cursorPosition.Disable();
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
        float tolerance = 0.02f;
        while (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - zoomAmount) > tolerance)
        {
            virtualCamera.m_Lens.OrthographicSize =
                Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, zoomAmount, Time.deltaTime * zoomSpeed);

            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = zoomAmount;
        
        currentZoomCoroutine = null;
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

    private void ZoomInOut(InputAction.CallbackContext context)
    {
        if (currentCameraCoroutine != null || currentZoomCoroutine != null)
        {
            return;
        }

        float zoomAmount = 2;
        float currentZoom = virtualCamera.m_Lens.OrthographicSize;

        if (context.ReadValue<Vector2>().y > 0.1)
        {
            currentZoom -= zoomAmount;
        }
        else if (context.ReadValue<Vector2>().y < 0.1)
        {
            currentZoom += zoomAmount;
        }

        currentZoom = Mathf.Clamp(currentZoom, minZoomDistance, maxZoomDistance);
        
        virtualCamera.m_Lens.OrthographicSize = currentZoom;
    }

    protected override void WorldViewChanged(Views newView)
    {
        if (newView == Views.GalaxyView)
        {
            ZoomOutToGalaxy();
        }
    }
}
