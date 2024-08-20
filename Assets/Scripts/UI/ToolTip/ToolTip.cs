using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private LayoutElement element;
    [SerializeField] private int characterWrapLimit;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector2 offset;
    
    private PlayerControls _playerControls;
    private InputAction _currentMousePosition;
    
    private void Awake()
    {
        _playerControls = new PlayerControls();
        _currentMousePosition = _playerControls.Player.CusorPosition;
    }

    private void OnEnable() => _currentMousePosition.Enable();

    private void OnDisable() => _currentMousePosition.Disable();

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerText.gameObject.SetActive(false);   
            return;
        }
        
        headerText.gameObject.SetActive(true);
        headerText.text = header;

        contentText.text = content;
        
        var headerLength = headerText.text.Length;
        var contentLength = contentText.text.Length;

        element.enabled = headerLength > characterWrapLimit || contentLength > characterWrapLimit;
    }
    
    private void Update()
    {
        var mousePosition = _currentMousePosition.ReadValue<Vector2>();
        var pivotPoint = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);

        rectTransform.pivot = pivotPoint;
        transform.position = new Vector3(mousePosition.x + offset.x, mousePosition.y + offset.y, 0f);
    }
}
