using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
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
    public PlanetType Type { get; set; }

    public PlanetAbstractTileResource[] TileSet;
    public Tilemap Background;
    public Tilemap ResourceTile;

    [SerializeField] private BoardTile tilePrefab;


    private Dictionary<Vector2, BoardTile> _board = new();
    public readonly Dictionary<Vector2, AbstractBaseBuilding> BuildingBoard = new();

    private void Start()
    {
        CreateBoard();
        DrawBoardTile();
    }

    private void CreateBoard()
    {
        for (int i = -Radius; i <= Radius; i++)
        {
            for (int j = -Radius; j <= Radius; j++)
            {
                // Make the grid like a circle
                if (Mathf.Round(new Vector2(i, j).magnitude) >= Radius)
                    continue;

                Vector3 position = transform.position + new Vector3(i, j, -2);
                BoardTile tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                _board.Add(position, tile);
            
                tile.Init(GetRandomPlanetResource());
            }
        }
        _board.Remove(Vector2.zero);
    }

    private void DrawBoardTile()
    {
        PlanetAbstractTileResource tiles = GetTileSet();
        if (tiles == null)
            return;
        for (int i = -Radius; i <= Radius; i++)
        {
            for (int j = -Radius; j <= Radius; j++)
            {
                // Make the grid like a circle
                if (Mathf.Round(new Vector2(i, j).magnitude) >= Radius + 1)
                    continue;
                Background.SetTile(new Vector3Int(i, j, 0), tiles.BaseTile);
            }
        }
    }

    private ResourceTypes GetRandomPlanetResource()
    {
        ResourceTypes type = ResourceTypes.None;
        float rand = Random.value;
        switch (Type)
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

    private PlanetAbstractTileResource GetTileSet()
    {
        PlanetAbstractTileResource resource = null;

        foreach(var tile in TileSet)
        {
            if (tile.Type == Type)
                return tile;
        }

        return resource;
    }
}
