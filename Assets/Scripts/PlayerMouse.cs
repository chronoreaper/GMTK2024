using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouse : WorldView
{
    public static PlayerMouse Inst { get; private set; }

    public GameObject SelectionBoxSprite;
    public SpawnFromButton Spawner;

    private Views CurrentLayer;
    
    public UIDisplay UITop;

    private float _pressTime = 0;
    private PlayerControls _playerControls;
    private InputAction _click;
    private InputAction _cursorPosition;

    private Dictionary<ResourceTypes, int> _resources = new();
    private List<Unit> _selected = new();
    private Vector2 _mouseStart = new();
    private Vector2 _mouseEnd = new();

    public delegate void PlanetSelected(Vector2 planetPosition);
    public static PlanetSelected planetSelected;

    public delegate void PlanetDeselected();
    public static PlanetDeselected planetDeselected;

    // TODO maybe move it into its own class?
    public void GainResources(ResourceTypes type, int amount)
    {
        if (!_resources.ContainsKey(type))
            _resources.Add(type, 0);
        _resources[type] += amount;
        UITop.ChangeResourceValue(type, _resources[type]);
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(this);
        }
        else
        {
            Inst = this;
        }
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
            SelectionBoxSprite.SetActive(true);
            SelectionBoxSprite.transform.position = (_mouseStart + _mouseEnd) / 2;
            SelectionBoxSprite.transform.localScale = Abs(_mouseStart, _mouseEnd);
        }

        // Check if selected objects are not destroyed
        int i = 0;
        while(i < _selected.Count)
        {
            if (_selected[i] == null)
            {
                _selected.RemoveAt(i);
            }
            else
                i++;
        }
    }

    private void Click(InputAction.CallbackContext obj)
    {
        _mouseStart = transform.position;
        _mouseEnd = transform.position;
        _pressTime = 0;
    }

    private void Release(InputAction.CallbackContext obj)
    {
        _mouseEnd = transform.position;
        // Box must be larger than a certain value and Must hold longer than a certain time
        Unit hover = CurrentlyHoveringOver();
        if (hover != null && hover.Team == Unit.UnitTeam.Player)
            SelectUnits();
        else if ((_mouseEnd - _mouseStart).magnitude > 1 && _pressTime > 0.1f)
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

        _mouseEnd = _mouseStart;
        SelectionBoxSprite.transform.localScale = Vector2.zero;
        SelectionBoxSprite.SetActive(false);
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
