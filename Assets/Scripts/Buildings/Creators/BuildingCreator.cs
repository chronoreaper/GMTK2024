using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingCreator : MonoBehaviour
{
    public Board ReferencedBoard 
    {
        get => _board;
        set
        {
            _board = value;
            buildingPrefab.ReferencedBoard = value;
        }
    }

    private Board _board;
    [SerializeField] private AbstractBaseBuilding buildingPrefab;
    [SerializeField] private float tileSize;
    [SerializeField] private Vector2 tileOffset;

    [Header("Rendering")] 
    [SerializeField] private SpriteRenderer[] spriteRenderers;
    [SerializeField] private Color canBuildColor;
    [SerializeField] private Color cantBuildColor;

    private PlayerControls _playerControls;
    private InputAction _cursorPosition;
    private InputAction _rightClick;
    private float _currentAngle;
    
    private AbstractBaseBuilding Create(Vector2 position, Quaternion rotation)
    {
        AbstractBaseBuilding building = Instantiate(buildingPrefab, position, rotation);
        building.ReferencedBoard = ReferencedBoard;
        ReferencedBoard.BuildingBoard.Add(transform.position, building);
        
        return building;
    }

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _cursorPosition = _playerControls.Player.CusorPosition;
        _rightClick = _playerControls.Player.RightClick;
        _rightClick.performed += Cancel;
        _playerControls.Player.Click.performed += TryBuild;
        _playerControls.Player.RotateBuilding.performed += Rotate;
    }

    private void Update()
    {
        SnapObject();
        ChangeColor();
    }

    private void OnEnable()
    {
        _cursorPosition.Enable();
        _rightClick.Enable();
        _playerControls.Player.Click.Enable();
        _playerControls.Player.RotateBuilding.Enable();
    }

    private void OnDisable()
    {
        _cursorPosition.Disable();
        _rightClick.Disable();
        _playerControls.Player.Click.Disable();
        _playerControls.Player.RotateBuilding.Disable();
    }

    private Vector2 GetGridPosition(Vector2 currentPosition)
    {
        var snappedX = Mathf.Round(currentPosition.x / tileSize) * tileSize + tileOffset.x;
        var snappedY = Mathf.Round(currentPosition.y / tileSize) * tileSize + tileOffset.y;

        return new Vector2(snappedX, snappedY);
    }

    private void SnapObject()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(_cursorPosition.ReadValue<Vector2>());
        
        transform.position = GetGridPosition(mousePosition);
    }

    private void ChangeColor()
    {
        Color color = cantBuildColor;
        if (buildingPrefab.CanBuild(transform.position))
        {
            color = canBuildColor;
        }

        foreach(var sr in spriteRenderers)
        {
            sr.color = color;
        }
    }

    private void Cancel(InputAction.CallbackContext callbackContext) => Destroy(gameObject);

    private void TryBuild(InputAction.CallbackContext callbackContext)
    {
        if (!buildingPrefab.CanBuild(transform.position))
        {
            return;
        }
        PlayerMouse.Inst.PayFor(buildingPrefab.Type);

        var building = Create(transform.position, transform.rotation);

        building.Build();
    }

    private void Rotate(InputAction.CallbackContext callbackContext)
    {
        _currentAngle += 90;
        
        transform.rotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);
    }
}
