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
    private Animator _animator;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _movementInput = _playerControls.Player.Move;
    }
    
    private void OnEnable() => _movementInput.Enable();

    private void OnDisable() => _movementInput.Disable();

    private void Update()
    {
        Movement();
        PlayAnimations();
        RotatePlayer();
    }

    private void Movement()
    {
        _movementDirection = _movementInput.ReadValue<Vector2>();
        
        _rb.velocity = _movementDirection * speed;
    }

    private void PlayAnimations()
    {
        _animator.SetFloat("Horizontal", _movementDirection.x);
        _animator.SetFloat("Vertical", _movementDirection.y);
        _animator.SetBool("Walking", _movementDirection != Vector2.zero);
    }

    //TODO: Use Actual Animations
    private void RotatePlayer()
    {
        var playerScale = transform.localScale;
        
        if (_movementDirection != Vector2.left)
        {
            transform.localScale = new Vector3(1f, playerScale.y, playerScale.z);
            return;
        }

        transform.localScale = new Vector3(-1f, playerScale.y, playerScale.z);
    }
}
