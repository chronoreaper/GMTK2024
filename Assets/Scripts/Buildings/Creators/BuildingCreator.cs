using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingCreator : MonoBehaviour
{
    [SerializeField] private AbstractBaseBuilding buildingPrefab;
    [SerializeField] private float tileSize;
    [SerializeField] private Vector2 tileOffset;

    [Header("Rendering")] 
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color canBuildColor;
    [SerializeField] private Color cantBuildColor;

    private PlayerControls _playerControls;
    private InputAction _cursorPosition;
    private InputAction _rightClick;
    
    private AbstractBaseBuilding Create(Vector2 position, Quaternion rotation)
    {
        var building = Instantiate(buildingPrefab, position, rotation);
        Board.BuildingBoard.Add(transform.position, building);
        
        return building;
    }

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _cursorPosition = _playerControls.Player.CusorPosition;
        _rightClick = _playerControls.Player.RightClick;
        _rightClick.Enable();
        _rightClick.performed += Cancel;
        _playerControls.Player.Click.performed += TryBuild;
    }

    private void Update()
    {
        SnapObject();
        ChangeColor();
    }

    private void OnEnable()
    {
        _cursorPosition.Enable();
        _playerControls.Player.RightClick.Enable();
        _playerControls.Player.Click.Enable();
    }

    private void OnDisable()
    {
        _cursorPosition.Disable();
        _playerControls.Player.RightClick.Disable();
        _playerControls.Player.Click.Disable();
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
        if (buildingPrefab.CanBuild(transform.position))
        {
            spriteRenderer.color = canBuildColor;
            return;
        }

        spriteRenderer.color = cantBuildColor;
    }

    private void Cancel(InputAction.CallbackContext callbackContext) => Destroy(gameObject);

    private void TryBuild(InputAction.CallbackContext callbackContext)
    {
        if (!buildingPrefab.CanBuild(transform.position))
        {
            return;
        }
        
        var building = Create(transform.position, Quaternion.identity);

        building.Build();
    }
}
