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
    
    [Header("Conquer UI")] 
    [SerializeField] private TextMeshProUGUI conquerPercentageText;
    [SerializeField] private Image _sprite;
    
    private int _numberOfPlanetsConquer;
    public readonly List<UnitPlanet> spawnedPlanets = new();
    
    private void Awake()
    {
        Instance = this;
        UpdateConquerPercentage();
    }

    public void UpdateConquerPercentage(Unit.UnitTeam team)
    {
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
        
        var percentage = _numberOfPlanetsConquer / spawnedPlanets.Count;
        conquerPercentageText.text = $"{percentage * 100}%";
        _sprite.fillAmount = percentage;
        
        if (percentage == 1)
        {
            OnGalaxyConquered?.Invoke();
        }
    }
    
    private void UpdateConquerPercentage()
    {
        var percentage = 0;
        conquerPercentageText.text = $"{percentage * 100}%";
        _sprite.fillAmount = percentage;
    }
}