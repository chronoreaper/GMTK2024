using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlanet : Unit
{
    public int Radius = 5;

    public Board ReferencedBoard { get; private set; } = null;

    private UnitBase _base;

    protected override void Awake()
    {
        base.Awake();
        _base = GetComponentInChildren<UnitBase>();
        ReferencedBoard = GetComponentInChildren<Board>();
        ReferencedBoard.Radius = Radius - 1;

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.transform.localScale = new Vector2(Radius, Radius) * 2;
        }
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

    public void Spwaner(float health, UnitTeam team, int radius, Board.PlanetType planetType)
    {
        // this.Hp.CurrentHealth = health;
        this.Hp.SetMaxHealth(health);
        this.Team = team;
        this.Radius = radius;
        this.ReferencedBoard.SetPlanetType(planetType);
        this.ReferencedBoard.SetBaseMaxHealth(health);
    }
}
