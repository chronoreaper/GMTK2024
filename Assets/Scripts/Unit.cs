using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Unit : MonoBehaviour
{
    public enum UnitTeam
    {
        Player,
        Enemy,
        Neutral
    }

    public Health Hp;
    public UnitTeam Team;


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
    }

    private void Start()
    {
        UpdateColor();
    }

    private void OnValidate()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        var sr = transform.GetComponentInChildren<SpriteRenderer>();
        sr.color = GetTeamColour();
    }
}
