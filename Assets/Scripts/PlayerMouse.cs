using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouse : MonoBehaviour
{
    public GameObject SelectionBoxSprite;

    PlayerControls _playerControls;
    InputAction _click;
    InputAction _cursorPosition;

    List<Unit> _selected = new List<Unit>();
    Vector2 _mouseStart = new Vector2();
    Vector2 _mouseEnd = new Vector2();

    // Start is called before the first frame update
    void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _click = _playerControls.Player.Click;
        _cursorPosition = _playerControls.Player.CusorPosition;
        _click.Enable();
        _click.performed += Click;
        _click.canceled += Release;
        _cursorPosition.Enable();
    }

    private void OnDisable()
    {
        _click.Disable();
        _cursorPosition.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(_cursorPosition.ReadValue<Vector2>());
        var unit = CurrentlyHoveringOver();

        if (_click.IsInProgress())
        {
            _mouseEnd = transform.position;
            SelectionBoxSprite.transform.position = (_mouseStart + _mouseEnd) / 2;
            SelectionBoxSprite.transform.localScale = Abs(_mouseStart, _mouseEnd);
        }
    }

    private void Click(InputAction.CallbackContext obj)
    {
        _mouseStart = transform.position;
        _mouseEnd = transform.position;
        Unit hover = CurrentlyHoveringOver();
        if (hover != null && hover.Team == Unit.UnitTeam.Player)
            SelectUnits();
        else
        {
            // Move units
            foreach(var selected in _selected)
            {
                Ship ship = selected.GetComponent<Ship>();
                if (ship == null)
                    continue;
                ship.MoveTowards(transform.position);
            }
        }
    }

    private void Release(InputAction.CallbackContext obj)
    {
        _mouseEnd = transform.position;
        SelectUnits();
        _mouseEnd = _mouseStart;
        SelectionBoxSprite.transform.localScale = Vector2.zero;
    }

    private Unit CurrentlyHoveringOver()
    {
        Collider2D result = Physics2D.OverlapPoint(transform.position);
        if (!result)
            return null;
        return result.GetComponent<Unit>();
    }

    private void SelectUnits()
    {
        Debug.Log("Selection");
        _selected.Clear();
        Collider2D[] results = Physics2D.OverlapBoxAll(Vector2.Min(_mouseStart, _mouseEnd), Abs(_mouseStart, _mouseEnd), 0);
        // Get closest target that is not on team
        foreach (Collider2D result in results)
        {
            Unit other = result.GetComponent<Unit>();
            if (other == null)
                continue;
            if (other.Team != Unit.UnitTeam.Player)
                continue;
            Debug.Log(other);
            _selected.Add(other);
        }
    }

    private Vector2 Abs(Vector2 a, Vector2 b)
    {
        return new Vector2(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }
}
