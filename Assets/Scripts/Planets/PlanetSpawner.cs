using System;
using System.Collections.Generic;
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
        Unit.UnitTeam.Enemy,
        Unit.UnitTeam.Player,
        Unit.UnitTeam.Neutral
    };

    private readonly List<Board.PlanetType> planetTypes = new()
    {
        Board.PlanetType.Forest,
        Board.PlanetType.Lava,
        Board.PlanetType.Stone,
    };

    private void Start()
    {
        spawnArea.transform.position = Vector2.zero;
        _spawnAreaSize = new Vector2(spawnArea.bounds.size.x / 2, spawnArea.bounds.size.y / 2);
        spawnArea.gameObject.SetActive(false);

        SpawnPlanets();
    }

    private void SpawnPlanets()
    {
        for (int i = 0; i < numberOfPlanetsToSpawn; i++)
        {
            GameObject planetInstance = Instantiate(planet, GetRandomPosition(), transform.rotation);
            UnitPlanet unitPlanet = planetInstance.GetComponent<UnitPlanet>();

            unitPlanet.Spwaner(GetMaxHealth(), GetRandomTeam(), GetRandomRadius(), GetRandomPlanetType());
        }
    }

    private Vector2 GetRandomPosition()
    {
        float randomPositionX = Random.Range(-_spawnAreaSize.x, _spawnAreaSize.x);
        float randomPositionY = Random.Range(-_spawnAreaSize.y, _spawnAreaSize.y);
        Vector2 randomPosition = new(randomPositionX, randomPositionY);

        if (IsPositionOccupied(randomPosition, maxRadius * 2))
        {
            Debug.Log("looop");
            return GetRandomPosition();
        }
        else
        {
            Debug.Log("Random Position " + randomPosition);

            return randomPosition;
        }
    }

    private bool IsPositionOccupied(Vector2 pos, float radius)
    {
        //Change this at your own risk
        if (_spawnAreaSize.x < 150 && _spawnAreaSize.y < 100 || numberOfPlanetsToSpawn > 20)
        {
            throw new OutOfMemoryException("Make The Spawn Area Bigger Or There Will Be a Memory Leak!");
        }
        
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
        
        return unitTeams[randomTeamIndex];
    }

    private Board.PlanetType GetRandomPlanetType()
    {
        int randomTypeIndex = Random.Range(0, planetTypes.Count);
        
        return planetTypes[randomTypeIndex];
    }

}
