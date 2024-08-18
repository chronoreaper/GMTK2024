using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDragSystem : MonoBehaviour
{
    [Header("Edge Scrolling")]
    [SerializeField] private bool useEdgeScrolling;
    [Range(20, 50f)][SerializeField] float edgeScrollingSpeed;

    [Space]
    [Header("Drag Pan")]
    [SerializeField] private bool useDragPan;
    [Range(0.5f, 2f)][SerializeField] float dragPanSpeed;

    PlayerControls _playerControls;
    InputAction _click;
    InputAction _cursorPosition;


    private bool isDragPanMoveActive;
    private Vector2 lastMousePosition;

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
    }

    private void OnDisable()
    {
        _click.Disable();
    }

    private void Update()
    {
        HandleCameraMovement();
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

    public void LockDrag()
    {
        useDragPan = false;
    }

    public void UnlockDrag()
    {
        useDragPan = true;
    }
}
