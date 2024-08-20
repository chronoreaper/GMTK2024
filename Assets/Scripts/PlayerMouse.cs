using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static CostToBuild;

public class PlayerMouse : WorldView
{
    public static PlayerMouse Inst { get; private set; }
    public Dictionary<ResourceTypes, int> Resources { get; private set; } = new();

    public CostToBuild[] BuildCosts;

    public GameObject SelectionBoxSprite;
    public SpawnFromButton Spawner;
    public AudioClip BuildClip;

    private Views CurrentLayer;
    
    public UIDisplay UITop;

    private float _pressTime = 0;
    private PlayerControls _playerControls;
    private InputAction _click;
    private InputAction _back;
    private InputAction _cursorPosition;

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
        if (!Resources.ContainsKey(type))
            Resources.Add(type, 0);
        Resources[type] += amount;
        UITop.ChangeResourceValue(type, Resources[type]);
    }

    public bool CanPayFor(BuildTypes unit)
    {
        foreach(var cost in BuildCosts)
        {
            if (cost.Building != unit)
                continue;
            foreach (ResourceCost rc in cost.Cost)
            {
                if (!Inst.Resources.ContainsKey(rc.Type)) return false;
                if (Inst.Resources[rc.Type] < rc.Amount) return false;
            }
        }
        return true;
    }

    public void PayFor(BuildTypes unit)
    {
        //AudioManager.Inst.Play(BuildClip);
        foreach (var cost in BuildCosts)
        {
            if (cost.Building != unit)
                continue;
            foreach (ResourceCost rc in cost.Cost)
            {
                if (Inst.Resources.ContainsKey(rc.Type))
                {
                    Inst.Resources[rc.Type] -= rc.Amount;

                    // Make things more expensive
                    rc.Amount = Mathf.Min(10, rc.Amount + 1);
                    UITop.ChangeResourceValue(rc.Type, Resources[rc.Type]);
                }
            }

            break;
        }
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
        GainResources(ResourceTypes.Water, 10);
        GainResources(ResourceTypes.Wood, 10);
        GainResources(ResourceTypes.Lava, 20);
        GainResources(ResourceTypes.Stone, 10);
    }

    private void OnEnable()
    {
        _click = _playerControls.Player.Click;
        _click.Enable();
        _click.performed += Click;
        _click.canceled += Release;

        _back = _playerControls.Player.RightClick;
        _back.Enable();
        _back.performed += Cancel;

        _cursorPosition = _playerControls.Player.CusorPosition;
        _cursorPosition.Enable();
    }

    private void OnDisable()
    {
        _click.Disable();
        _cursorPosition.Disable();
        _back.Disable();
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
            SelectionBoxSprite.transform.localScale = Abs(_mouseEnd - _mouseStart);
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

        if (_selected.Count == 0)
        {
            Cancel(obj);
        }
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
        else if (_selected.Count > 0)
        {
            // Move units
            foreach (var selected in _selected)
            {
                if (!selected.TryGetComponent<Ship>(out var ship))
                    continue;
                ship.MoveTowards(transform.position);
            }
        }

        _mouseEnd = _mouseStart;
        SelectionBoxSprite.transform.localScale = Vector2.zero;
        SelectionBoxSprite.SetActive(false);
    }

    private void Cancel(InputAction.CallbackContext obj)
    {
        planetDeselected?.Invoke();
        Spawner.ReferencedBoard = null;
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

        Collider2D[] results = Physics2D.OverlapBoxAll((_mouseStart + _mouseEnd) / 2, Abs(_mouseEnd - _mouseStart), 0);

        // Check if you selected a single planet
        if (results.Length == 1)
        {
            results[0].TryGetComponent(out Unit other);

            if (other != null && other.Team != Unit.UnitTeam.Player)
            {
                //planetDeselected?.Invoke();
                //Spawner.ReferencedBoard = null;
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
        else if (results.Length == 0)
        {
            planetDeselected?.Invoke();
            Spawner.ReferencedBoard = null;
        }

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

    private Vector2 Abs(Vector2 a)
    {
        return new Vector2(Mathf.Abs(a.x), Mathf.Abs(a.y));
    }

    private Vector2 Min(Vector2 a, Vector2 b)
    {
        return new Vector2(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
    }

    protected override void WorldViewChanged(Views newView)
    {
        CurrentLayer = newView;
    }
}
