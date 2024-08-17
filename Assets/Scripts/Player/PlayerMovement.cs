using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Health))]
public class PlayerMovement : MonoBehaviour
{
    [Header("PlayerMovement Settings")] 
    [SerializeField] private float speed;
    
    private Rigidbody2D _rb;
    private PlayerControls _playerControls;
    private InputAction _movementInput;
    private Vector2 _movementDirection;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        _movementInput = _playerControls.Player.Move;
    }
    
    private void OnEnable() => _movementInput.Enable();

    private void OnDisable() => _movementInput.Disable();

    private void Update()
    {
        _movementDirection = _movementInput.ReadValue<Vector2>();
        
        _rb.velocity = _movementDirection * speed;
    }

    //TODO: Use Actual Animations
    private void RotatePlayer()
    {
        if (_movementDirection == Vector2.zero)
        {
            return;
        }

        var angle = Mathf.Atan2(_movementDirection.y, _movementDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
