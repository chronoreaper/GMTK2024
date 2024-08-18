using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Board;

public class UnitPlanet : Unit
{
    public int Radius = 5;
    public PlanetType Type;

    public Board ReferencedBoard { get; private set; } = null;

    private UnitBase _base;

    protected override void Awake()
    {
        base.Awake();
        _base = GetComponentInChildren<UnitBase>();
        ReferencedBoard = GetComponentInChildren<Board>();
        ReferencedBoard.Radius = Radius - 1;
        ReferencedBoard.Type = Type;
        OnValidate();
    }

    protected override void OnValidate()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.transform.localScale = new Vector2(Radius, Radius) * 2;
        }
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
            collider.radius = Radius;

        _base = GetComponentInChildren<UnitBase>();
        _base.Team = Team;
    }

    protected void Update()
    {
        if (_base != null)
        {
            Hp = _base.Hp;
            Team = _base.Team;
        }
    }

    protected override void UpdateColor()
    {
        // TODO? do an outline?
    }
}
