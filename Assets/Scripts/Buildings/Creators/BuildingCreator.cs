using UnityEngine;

public class BuildingCreator : MonoBehaviour
{
    [SerializeField] private AbstractBaseBuilding buildingPrefab;
    [SerializeField] private float tileSize;
    [SerializeField] private Vector2 tileOffset;

    [Header("Rendering")] 
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color canBuildColor;
    [SerializeField] private Color cantBuildColor;
    
    private AbstractBaseBuilding Create(Vector2 position, Quaternion rotation)
    {
        var building = Instantiate(buildingPrefab, position, rotation);
        Board.BuildingBoard.Add(transform.position, building);
        
        return building;
    }

    private void Update()
    {
        SnapObject();
        ChangeColor();

        if (Input.GetMouseButtonDown(0) && CanBuild())
        {
            var building = Create(transform.position, Quaternion.identity);

            if (building.Type == BuildingType.Miner)
            {
                var miner = (Miner)building;
                miner.Init(Board.GetTileByPosition(miner.transform.position).Resource);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
    }

    private Vector2 GetGridPosition(Vector2 currentPosition)
    {
        var snappedX = Mathf.Round(currentPosition.x / tileSize) * tileSize + tileOffset.x;
        var snappedY = Mathf.Round(currentPosition.y / tileSize) * tileSize + tileOffset.y;

        return new Vector2(snappedX, snappedY);
    }

    private void SnapObject()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            transform.position.z - Camera.main.transform.position.z));
        
        transform.position = GetGridPosition(mousePosition);
    }

    private void ChangeColor()
    {
        if (CanBuild())
        {
            spriteRenderer.color = canBuildColor;
            return;
        }

        spriteRenderer.color = cantBuildColor;
    }
    
    private bool CanBuild()
    {
        var tilePosition = Board.GetTileByPosition(transform.position);
        
        return tilePosition != null && tilePosition.Resource != ResourceTypes.None && Board.GetBuildingByPosition(transform.position) == null;
    }
}
