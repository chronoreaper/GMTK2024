using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitTeam
    {
        Player,
        Enemy,
        Neutral
    }

    public int Hp;
    public int MaxHp;
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

    private void Start()
    {
        Hp = MaxHp;
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
