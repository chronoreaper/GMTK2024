using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Unit : MonoBehaviour
{
    public enum UnitTeam
    {
        Player,
        Enemy,
        Neutral
    }

    public Health Hp { get; set; }
    public UnitTeam Team
    {
        get => _initialTeam;
        set
        {
            _initialTeam = value;
            UpdateColor();
        }
    }

    [SerializeField]
    protected UnitTeam _initialTeam;

    public Color GetTeamColour()
    {
        Color colour = Color.white;
        switch (Team)
        {
            case UnitTeam.Player:
                colour = Color.white;
                break;
            case UnitTeam.Enemy:
                colour = Color.red;
                break;
            case UnitTeam.Neutral:
                colour = Color.green;
                break;
            default:
                colour = Color.white;
                break;
        }
        return colour;
    }

    protected virtual void Awake()
    {
        Hp = GetComponent<Health>();
    }
    
    protected virtual void Start()
    {
        Team = _initialTeam;
    }

    protected virtual void OnEnable()
    {
        Bullet.onHitTarget += OnHit;
    }

    protected virtual void OnDisable()
    {
        Bullet.onHitTarget -= OnHit;
    }

    protected virtual void OnValidate()
    {
        
    }

    private void OnHit(Collider2D collider, Bullet bullet)
    {
        // if (bullet.Source.Team == Team)
        // {
        //     BulletSpawner.Instance.Release(bullet);
        //     return;
        // }
        
        if (collider == GetComponent<Collider2D>() && bullet.Source.Team != Team)
        {
            Hp.Damage(1, bullet.Source);
            BulletSpawner.Instance.Release(bullet);
        }
    }

    protected virtual void UpdateColor()
    {
        var sr = transform.GetComponentInChildren<SpriteRenderer>();
        sr.color = GetTeamColour();
    }
}
