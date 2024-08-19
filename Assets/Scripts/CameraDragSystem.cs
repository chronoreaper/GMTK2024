using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDragSystem : MonoBehaviour
{
    [Header("Edge Scrolling")]
    [SerializeField] private bool useEdgeScrolling;
    [Range(150f, 300f)][SerializeField] float edgeScrollingSpeed = 80f;

    [Space]
    [Header("Drag Pan")]
    [SerializeField] private bool useDragPan;
    [Range(1f, 10f)][SerializeField] float dragPanSpeed = 5f;

    PlayerControls _playerControls;
    InputAction _click;
    InputAction _cursorPosition;


    private bool isDragPanMoveActive;
    private bool isEdgeScrollingActive;
    private Vector2 lastMousePosition;

    private Vector2 worldSize;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _click = _playerControls.Player.RightClick;
        _click.Enable();
        _click.performed += MouseDown;
        _click.canceled += MouseUp;

        _cursorPosition = _playerControls.Player.CusorPosition;
        _cursorPosition.Enable();

        PlanetSpawner.setMapBoundaries += SetWorldSize;

        isEdgeScrollingActive = true;
    }

    private void OnDisable()
    {
        _click.Disable();

        PlanetSpawner.setMapBoundaries -= SetWorldSize;
    }

    private void Update()
    {
        ClampCameraToWorld();
        HandleCameraMovement();
    }

    private void SetWorldSize(Vector2 boundaries)
    {
        worldSize = boundaries;
    }

    private void ClampCameraToWorld()
    {
        Vector2 clampPostion = transform.position;

        clampPostion.x = Mathf.Clamp(transform.position.x, -worldSize.x, worldSize.x);
        clampPostion.y = Mathf.Clamp(transform.position.y, -worldSize.y, worldSize.y);

        transform.position = clampPostion;
    }

    private void HandleCameraMovement()
    {
        if (useEdgeScrolling)
        {
            EdgeScrollingCameraMovement();
        }

        if (useDragPan)
        {
            DragPanCameraMovement();
        }
    }

    private void MouseDown(InputAction.CallbackContext context)
    {
        isDragPanMoveActive = true;
        lastMousePosition = _cursorPosition.ReadValue<Vector2>();
    }

    private void MouseUp(InputAction.CallbackContext context)
    {
        isDragPanMoveActive = false;
    }

    private void DragPanCameraMovement()
    {
        if (isDragPanMoveActive)
        {
            Vector2 inputDir = Vector2.zero;
            Vector2 mouseMovmentDelta = _cursorPosition.ReadValue<Vector2>() - lastMousePosition;

            inputDir.x = -mouseMovmentDelta.x * dragPanSpeed;
            inputDir.y = -mouseMovmentDelta.y * dragPanSpeed;

            lastMousePosition = _cursorPosition.ReadValue<Vector2>();

            Vector3 moveDir = transform.up * inputDir.y + transform.right * inputDir.x;

            transform.position += moveDir * Time.deltaTime;
        }

    }

    private void EdgeScrollingCameraMovement()
    {
        if (isEdgeScrollingActive)
        {

            Vector2 inputDir = Vector2.zero;
            Vector2 mousePosition = _cursorPosition.ReadValue<Vector2>();

            int edgeScrollingSize = 50;

            if (mousePosition.x < edgeScrollingSize)
            {
                inputDir.x = -1f;
            }
            if (mousePosition.y < edgeScrollingSize)
            {
                inputDir.y = -1f;
            }
            if (mousePosition.x > Screen.width - edgeScrollingSize)
            {
                inputDir.x = 1f;
            }
            if (mousePosition.y > Screen.height - edgeScrollingSize)
            {
                inputDir.y = 1f;
            }

            Vector3 moveDir = transform.up * inputDir.y + transform.right * inputDir.x;

            transform.position += edgeScrollingSpeed * Time.deltaTime * moveDir;
        }
    }

    public void LockDrag()
    {
        useDragPan = false;

        isEdgeScrollingActive = false;
    }

    public void UnlockDrag()
    {
        useDragPan = true;

        isEdgeScrollingActive = true;
    }

    public void EnableEdgeScroll()
    {
        useEdgeScrolling = true;
    }

    public void DisableEdgeScroll()
    {
        useEdgeScrolling = false;
    }

    public void SetDragPanSpeed(float value)
    {
        dragPanSpeed = value;
    }
}
