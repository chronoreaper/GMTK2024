using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private Miner minerPrefab;
    
    [Header("Board Settings")] 
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private BoardTile tilePrefab;
    [SerializeField] private Camera mainCamera;

    private Dictionary<Vector2, BoardTile> _board = new();

    private void Awake()
    {
        CreateBoard();
        SetUpCamera();
    }

    private void CreateBoard()
    {
        var randomX = Random.Range(0, width);
        var randomY = Random.Range(0, height);
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var tile = Instantiate(tilePrefab, new Vector3(i, j), Quaternion.identity);
                _board.Add(new Vector3(i, j), tile);

                if (i == 5 && j == 2)
                {
                    tile.Init(true);

                    var position = new Vector3(i, j);
                    
                    var miner = Instantiate(minerPrefab, position, Quaternion.identity);
                    miner.Init(GetTileByPosition(position).Resource);
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
    
    public BoardTile GetTileByPosition(Vector2 position) => _board.GetValueOrDefault(position);
}
