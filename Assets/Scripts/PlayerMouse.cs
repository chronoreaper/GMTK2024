using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouse : MonoBehaviour
{
    public static PlayerMouse Inst { get; private set; }
    public enum Layer
    {
        Planet,
        System
    }

    public GameObject SelectionBoxSprite;
    public SpawnFromButton Spawner;
    public Layer CurrentLayer;
    public UIDisplay UITop;

    float _pressTime = 0;
    PlayerControls _playerControls;
    InputAction _click;
    InputAction _cursorPosition;

    Dictionary<ResourceTypes, int> _resources = new();
    List<Unit> _selected = new List<Unit>();
    Vector2 _mouseStart = new Vector2();
    Vector2 _mouseEnd = new Vector2();


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

        // Check if you selected a single planet
        if (_selected.Count == 1)
        {
            UnitPlanet planet = _selected[0].GetComponent<UnitPlanet>();
            if (planet != null)
            {
                // Only show ui if you can build on planet
                if (planet.Team == Unit.UnitTeam.Player)
                    Spawner.ReferencedBoard = planet.ReferencedBoard;
                else
                    Spawner.ReferencedBoard = null;
            }
            else
                Spawner.ReferencedBoard = null;
        }
        else
            Spawner.ReferencedBoard = null;
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
            // Move units
            foreach (var selected in _selected)
            {
                Ship ship = selected.GetComponent<Ship>();
                if (ship == null)
                    continue;
                ship.MoveTowards(transform.position);
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
    }

    private Vector2 Abs(Vector2 a, Vector2 b)
    {
        return new Vector2(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }
}
