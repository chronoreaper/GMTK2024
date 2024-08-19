using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public enum PlanetType
    {
        Forest,
        Stone,
        Lava
    }

    [Header("Board Settings")]
    public int Radius;

    [SerializeField] private BoardTile tilePrefab;
    [SerializeField] private PlanetType _planetType;


    private Dictionary<Vector2, BoardTile> _board = new();
    public readonly Dictionary<Vector2, AbstractBaseBuilding> BuildingBoard = new();

    private void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        for (int i = -Radius; i < Radius; i++)
        {
            for (int j = -Radius; j < Radius; j++)
            {
                // Make the grid like a circle
                if (Mathf.Round(new Vector2(i, j).magnitude) >= Radius)
                    continue;

                Vector3 position = transform.position + new Vector3(i, j);
                BoardTile tile = Instantiate(tilePrefab, (Vector2)position, Quaternion.identity, transform);
                _board.Add(position, tile);
            
                tile.Init(GetRandomPlanetResource());
            }
        }
        _board.Remove(Vector2.zero);
    }

    private ResourceTypes GetRandomPlanetResource()
    {
        ResourceTypes type = ResourceTypes.None;
        float rand = Random.value;
        switch (_planetType)
        {
            case PlanetType.Forest:
                if (rand <= 0.2)
                    type = ResourceTypes.Wood;
                else if (rand <= 0.4)
                    type = ResourceTypes.Water;
                else if (rand <= 0.6)
                    type = ResourceTypes.Mountain;
                break;
            case PlanetType.Stone:
                if (rand <= 0.5)
                    type = ResourceTypes.Stone;
                else if (rand <= 0.7)
                    type = ResourceTypes.Mountain;
                break;
            case PlanetType.Lava:
                if (rand <= 0.6)
                    type = ResourceTypes.Lava;
                else if (rand <= 0.8)
                    type = ResourceTypes.Mountain;
                break;
        }
        return type;
    }
    
    
    public BoardTile GetTileByPosition(Vector2 position) => _board.GetValueOrDefault(position);

    public AbstractBaseBuilding GetBuildingByPosition(Vector2 position) => BuildingBoard.GetValueOrDefault(position);

    public void SetPlanetType(PlanetType planetType) => _planetType = planetType;

    public void SetBaseMaxHealth(float hp) => GetComponent<HealthBase>().SetMaxHealth(hp);
}
