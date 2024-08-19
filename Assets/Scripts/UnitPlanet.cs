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

    private CircleCollider2D myCollider;

    protected override void Awake()
    {
        base.Awake();
        _base = GetComponentInChildren<UnitBase>();
        ReferencedBoard = GetComponentInChildren<Board>();
        ReferencedBoard.Radius = Radius - 1;
        ReferencedBoard.Type = Type;
        myCollider = GetComponent<CircleCollider2D>();
        OnValidate();
    }

    protected override void Start()
    {
        base.Start();  
        myCollider.radius = ReferencedBoard.Radius;
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
        base.UpdateColor();
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
        Hp.SetMaxHealth(health);
        Hp.CurrentHealth = health;
        _base.Team = team;
        Radius = radius;
        Type = planetType;
        ReferencedBoard.Radius = Radius - 1;
        ReferencedBoard.Type = Type;
        ReferencedBoard.SetBaseMaxHealth(health);
    }
}
