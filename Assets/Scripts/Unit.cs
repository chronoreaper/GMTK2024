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
                colour = Color.blue;
                break;
            case UnitTeam.Enemy:
                colour = Color.red;
                break;
            case UnitTeam.Neutral:
                colour = Color.gray;
                break;
            default:
                colour = Color.white;
                break;
        }
        return colour;
    }

    private void Awake()
    {
        Hp = GetComponent<Health>();
        Team = _initialTeam;
    }

    protected void Start()
    {
        UpdateColor();
    }

    protected void OnValidate()
    {
        Team = _initialTeam;
        UpdateColor();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.collider.GetComponent<Bullet>();
        // This may cause an error if the bullet Soruce is killed before the bullet is hit.
        if (bullet != null && bullet.Source.Team != Team)
        {
            Hp.Damage(1, bullet.Source);
            Destroy(bullet.gameObject);
        }
    }

    protected virtual void UpdateColor()
    {
        var sr = transform.GetComponentInChildren<SpriteRenderer>();
        sr.color = GetTeamColour();
    }
}
