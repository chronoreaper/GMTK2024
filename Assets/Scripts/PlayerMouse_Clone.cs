using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouse_Clone : WorldView
{
    public GameObject SelectionBoxSprite;
    public SpawnFromButton Spawner;

    private Views CurrentLayer;

    private float _pressTime = 0;
    private PlayerControls _playerControls;
    private InputAction _click;
    private InputAction _cursorPosition;

    private List<Unit> _selected = new();
    private Vector2 _mouseStart = new();
    private Vector2 _mouseEnd = new();

    public delegate void PlanetSelected(Vector2 planetPosition);
    public static PlanetSelected planetSelected;

    public delegate void PlanetDeselected();
    public static PlanetDeselected planetDeselected;

    // Start is called before the first frame update
    void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _click = _playerControls.Player.Click;
        _click.Enable();
        _click.performed += Click;
        _click.canceled += Release;

        _cursorPosition = _playerControls.Player.CusorPosition;
        _cursorPosition.Enable();
    }

    private void OnDisable()
    {
        _click.Disable();
        _cursorPosition.Disable();
    }

    private void Update()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(_cursorPosition.ReadValue<Vector2>());
        // var unit = CurrentlyHoveringOver();

        if (_click.IsInProgress())
        {
            _mouseEnd = transform.position;
            _pressTime += Time.deltaTime;
            SelectionBoxSprite.transform.position = (_mouseStart + _mouseEnd) / 2;
            SelectionBoxSprite.transform.localScale = Abs(_mouseStart, _mouseEnd);
        }
    }

    private void Click(InputAction.CallbackContext obj)
    {
        _mouseStart = transform.position;
        _mouseEnd = transform.position;
        _pressTime = 0;
        Unit hover = CurrentlyHoveringOver();
        if (hover != null && hover.Team == Unit.UnitTeam.Player)
            SelectUnits();
        else
        {
            planetDeselected?.Invoke();
            Spawner.ReferencedBoard = null;
            
            if (_selected.Count > 0)
            {
                // Move units
                foreach(var selected in _selected)
                {
                    if (!selected.TryGetComponent<Ship>(out var ship))
                        continue;
                    ship.MoveTowards(transform.position);
                }
            }
        }
    }

    private void Release(InputAction.CallbackContext obj)
    {
        _mouseEnd = transform.position;
        // Box must be larger than a certain value and Must hold longer than a certain time
        if ((_mouseEnd - _mouseStart).magnitude > 1 && _pressTime > 0.2f)
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
        _selected.Clear();
        Collider2D[] results = Physics2D.OverlapBoxAll(Vector2.Min(_mouseStart, _mouseEnd), Abs(_mouseStart, _mouseEnd), 0);

        // Check if you selected a single planet
        if (results.Length == 1)
        {
            results[0].TryGetComponent(out Unit other);

            if (other == null || other.Team != Unit.UnitTeam.Player)
            {
                planetDeselected?.Invoke();
                return;
            }

            _selected.Add(other);

            if (_selected[0].TryGetComponent<UnitPlanet>(out var planet))
            {
                planetSelected?.Invoke(planet.transform.position);
                // Only show ui if you can build on planet
                if (planet.Team == Unit.UnitTeam.Player)
                    Spawner.ReferencedBoard = planet.ReferencedBoard;
            }
            else
            {
                planetDeselected?.Invoke();

                Spawner.ReferencedBoard = null;
            }
            return;
        }

        planetDeselected?.Invoke();

        // Get closest target that is not on team
        foreach (Collider2D result in results)
        {
            Unit other = result.GetComponent<Unit>();
            if (other == null)
                continue;
            if (other.Team != Unit.UnitTeam.Player)
                continue;
            _selected.Add(other);
        }

        // Check if you selected a single planet
        // if (_selected.Count == 1)
        // {
        //     Spawner.ReferencedBoard = null;
        //     if (_selected[0].TryGetComponent<UnitPlanet>(out var planet))
        //     {
        //         planetSelected?.Invoke(planet.transform.position);
        //         // Only show ui if you can build on planet
        //         if (planet.Team == Unit.UnitTeam.Player)
        //             Spawner.ReferencedBoard = planet.ReferencedBoard;
        //     };
        // }
        // else
        // {
        //     planetDeselected?.Invoke();

        //     Spawner.ReferencedBoard = null;
        // }
    }

    private Vector2 Abs(Vector2 a, Vector2 b)
    {
        return new Vector2(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }

    protected override void WorldViewChanged(Views newView)
    {
        CurrentLayer = newView;
    }
}
