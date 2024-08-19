using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Board;

public class UnitPlanet : Unit
{
    public int Radius = 5;
    public PlanetType Type;

    public Board ReferencedBoard { get; private set; } = null;
    public PlanetSpriteResource[] Sprites;

    private UnitBase _base;
    private SpriteRenderer _sr;

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
        _sr = GetComponentInChildren<SpriteRenderer>();
        if (_sr != null)
        {
            _sr.transform.localScale = new Vector2(Radius, Radius) * 2;
        }
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
            collider.radius = Radius;
        
        UpdateSprite();
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

    void UpdateSprite()
    {
        // Get the sprite
        PlanetSpriteResource sprite = null;
        foreach (var i in Sprites)
        {
            if (i.Type == Type)
                sprite = i;
        }
        if (sprite == null)
            return;

        _sr.sprite = sprite.Sprite;
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
