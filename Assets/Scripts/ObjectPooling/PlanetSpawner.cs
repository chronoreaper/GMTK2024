using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetSpawner : MonoBehaviour
{
    [Header("Planet")]
    [SerializeField] private GameObject planet;
    [SerializeField] private int numberOfPlanetsToSpawn;

    [Header("Planet Health Range")]
    [SerializeField] private int minHealth;
    [SerializeField] private int maxHealth;

    [Header("Planet Radius Range")]
    [SerializeField] private int minRadius;
    [SerializeField] private int maxRadius;

    [Header("Spwan Area")]
    [SerializeField] private SpriteRenderer spawnArea;
    private Vector2 _spawnAreaSize;

    private readonly List<Unit.UnitTeam> unitTeams = new()
    {
        Unit.UnitTeam.Player,
        Unit.UnitTeam.Enemy,
        Unit.UnitTeam.Neutral
    };

    private readonly List<Board.PlanetType> planetTypes = new()
    {
        Board.PlanetType.Forest,
        Board.PlanetType.Lava,
        Board.PlanetType.Stone,
    };

    public delegate void SetMapBoundaries(Vector2 boundaries);
    public static SetMapBoundaries setMapBoundaries;

    private void Start()
    {
        spawnArea.transform.position = Vector2.zero;
        _spawnAreaSize = new Vector2Int((int)spawnArea.bounds.size.x / 2, (int)spawnArea.bounds.size.y / 2);
        spawnArea.gameObject.SetActive(false);

        setMapBoundaries?.Invoke(_spawnAreaSize);

        SpawnPlanets();
    }

    private void SpawnPlanets()
    {
        for (int i = 0; i < numberOfPlanetsToSpawn; i++)
        {
            Vector2? planetRandomPosition = GetRandomPosition();
            if (planetRandomPosition == null)
            {
                continue;
            }
            GameObject planetInstance = Instantiate(planet, (Vector3)planetRandomPosition, transform.rotation);
            UnitPlanet unitPlanet = planetInstance.GetComponent<UnitPlanet>();

            unitPlanet.Spwaner(GetMaxHealth(), GetRandomTeam(), GetRandomRadius(), GetRandomPlanetType());
            WinManager.Instance.spawnedPlanets.Add(unitPlanet);
        }
    }

    private Vector2Int? GetRandomPosition()
    {
        const int maxAttempts = 5; // Limit the number of attempts to find an unoccupied position
        int attempts = 0;
        Vector2Int randomPosition;

        while (attempts < maxAttempts)
        {
            int randomPositionX = (int)Random.Range(-_spawnAreaSize.x, _spawnAreaSize.x);
            int randomPositionY = (int)Random.Range(-_spawnAreaSize.y, _spawnAreaSize.y);
            randomPosition = new Vector2Int(randomPositionX, randomPositionY);

            if (!IsPositionOccupied(randomPosition, maxRadius * 1.5f))
            {
                return randomPosition;
            }

            attempts++;
        }

        Debug.LogWarning("Max Attempts Reached. You can't Spwan a Planet");
        return null;
    }

    private bool IsPositionOccupied(Vector2 pos, float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius);
        return colliders.Length > 0;
    }

    private float GetMaxHealth()
    {
        float randomHealth = Random.Range(minHealth, maxHealth);

        return randomHealth;
    }

    private int GetRandomRadius()
    {
        return Random.Range(minRadius, maxRadius);
    }

    private Unit.UnitTeam GetRandomTeam()
    {
        int randomTeamIndex = Random.Range(0, unitTeams.Count);

        return Unit.UnitTeam.Enemy;// unitTeams[randomTeamIndex];
    }

    private Board.PlanetType GetRandomPlanetType()
    {
        int randomTypeIndex = Random.Range(0, planetTypes.Count);
        return planetTypes[randomTypeIndex];
    }

    
}
