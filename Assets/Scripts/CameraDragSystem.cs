using UnityEngine;

public class CameraDragSystem : MonoBehaviour
{
    [Header("Edge Scrolling")]
    [SerializeField] private bool useEdgeScrolling;
    [Range(20, 50f)][SerializeField] float edgeScrollingSpeed;

    [Space]
    [Header("Drag Pan")]
    [SerializeField] private bool useDragPan;
    [Range(0.5f, 2f)][SerializeField] float dragPanSpeed;


    private bool isDragPanMoveActive;
    private Vector2 lastMousePosition;

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

    private void DragPanCameraMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragPanMoveActive = false;
        }

        if (isDragPanMoveActive)
        {
            Vector3 inputDir = Vector3.zero;
            Vector2 mouseMovmentDelta = (Vector2)Input.mousePosition - lastMousePosition;

            inputDir.x = -mouseMovmentDelta.x * dragPanSpeed;
            inputDir.y = -mouseMovmentDelta.y * dragPanSpeed;

            lastMousePosition = Input.mousePosition;

            Vector3 moveDir = transform.up * inputDir.y + transform.right * inputDir.x;

            transform.position += moveDir * Time.deltaTime;
        }

    }

    private void EdgeScrollingCameraMovement()
    {
        Vector3 inputDir = Vector3.zero;

        int edgeScrollingSize = 50;

        if (Input.mousePosition.x < edgeScrollingSize)
        {
            inputDir.x = -1f;
        }
        if (Input.mousePosition.y < edgeScrollingSize)
        {
            inputDir.y = -1f;
        }
        if (Input.mousePosition.x > Screen.width - edgeScrollingSize)
        {
            inputDir.x = 1f;
        }
        if (Input.mousePosition.y > Screen.height - edgeScrollingSize)
        {
            inputDir.y = 1f;
        }

        Vector3 moveDir = transform.up * inputDir.y + transform.right * inputDir.x;

        transform.position += edgeScrollingSpeed * Time.deltaTime * moveDir;
    }
}
