using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{   
    [Header("Board Settings")] 
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private BoardTile tilePrefab;
    [SerializeField] private Camera mainCamera;

    private static Dictionary<Vector2, BoardTile> _board = new();
    public static readonly Dictionary<Vector2, AbstractBaseBuilding> BuildingBoard = new();

    private void Awake()
    {
        CreateBoard();
        SetUpCamera();
    }

    private void CreateBoard()
    {       
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var tile = Instantiate(tilePrefab, new Vector3(i, j), Quaternion.identity, transform);
                _board.Add(new Vector3(i, j), tile);

                if (i == 5 && j == 2)
                {
                    tile.Init(true);
                    continue;
                }
                
                tile.Init(false);
            }
        }
    }
    
    private void SetUpCamera()
    {
        mainCamera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, mainCamera.transform.position.z);

        mainCamera.orthographicSize = (float)height / 2 + 1f;
    }
    
    public static BoardTile GetTileByPosition(Vector2 position) => _board.GetValueOrDefault(position);

    public static AbstractBaseBuilding GetBuildingByPosition(Vector2 position) => BuildingBoard.GetValueOrDefault(position);
}
