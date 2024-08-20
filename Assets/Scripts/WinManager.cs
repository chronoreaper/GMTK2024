using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    private static WinManager _instance;

    public static WinManager Instance
    {
        get => _instance;
        private set
        {
            if (_instance == null)
            {
                _instance = value;
            }

            if (_instance != value)
            {
                Destroy(value);
            }
        }
    }

    [SerializeField] private UnityEvent OnGalaxyConquered;
    [SerializeField] private UnityEvent OnGameLosed;
    
    [Header("Conquer UI")] 
    [SerializeField] private TextMeshProUGUI conquerPercentageText;
    [SerializeField] private Image _sprite;

    [Header("Lose Condition")] 
    [SerializeField] private UnitBase startPlanet;
    
    private float _numberOfPlanetsConquer = 1;
    public readonly List<UnitPlanet> spawnedPlanets = new();

    private void Awake() => Instance = this;

    public void UpdateConquerPercentage(Unit.UnitTeam team)
    {
        if (startPlanet.Team != Unit.UnitTeam.Player && spawnedPlanets.Exists(planet => planet.Team == Unit.UnitTeam.Player))
        {
            OnGameLosed?.Invoke();   
            return;
        }
        
        if (team == Unit.UnitTeam.Player)
        {
            _numberOfPlanetsConquer++;
        }

        if (team is Unit.UnitTeam.Enemy or Unit.UnitTeam.Neutral)
        {
            _numberOfPlanetsConquer--;
        }

        if (_numberOfPlanetsConquer < 0)
        {
            _numberOfPlanetsConquer = 0;
        }

        if (spawnedPlanets.Count == 0)
        {
            return;
        }

        // Check if selected objects are not destroyed
        int i = 0;
        while (i < spawnedPlanets.Count)
        {
            if (spawnedPlanets[i] == null)
            {
                spawnedPlanets.RemoveAt(i);
            }
            else
                i++;
        }

        var percentage = _numberOfPlanetsConquer / spawnedPlanets.Count;
        conquerPercentageText.text = $"{percentage * 100}%";
        _sprite.fillAmount = percentage;
        
        if (percentage == 1)
        {
            OnGalaxyConquered?.Invoke();
        }
    }
    
    public void UpdateConquerPercentage()
    {
        var percentage = _numberOfPlanetsConquer / spawnedPlanets.Count;
        conquerPercentageText.text = $"{percentage * 100}%";
        _sprite.fillAmount = percentage;
    }
}
