using UnityEngine;
using static CostToBuild;

public class SpawnFromButton : MonoBehaviour
{    
    public Board ReferencedBoard 
    {   get => _board; 
        set
        {
            _board = value;
            if (ReferencedBoard == null)
            {
                DisableUI(true);
            }
            else
            {
                DisableUI(false);
            }
        }
    }
    private Board _board = null;
    private BuildingCreator _spawnedObject = null;

    private void Start()
    {
        DisableUI(true);
    }

    public void Spawn(BuildingCreator prefab)
    {
        if (_spawnedObject != null)
            Destroy(_spawnedObject.gameObject);
        _spawnedObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        _spawnedObject.ReferencedBoard = ReferencedBoard;
    }

    public void SpawnShip()
    {
        if (!PlayerMouse.Inst.CanPayFor(BuildTypes.Ship))
            return;
        PlayerMouse.Inst.PayFor(BuildTypes.Ship);
        Ship ship = ShipSpawner.Instance.Get();

        // This may cause error TODO check
        ship.Init(_board.transform.position, Quaternion.identity, _board.GetComponent<Unit>().Team);
    }

    public void DisableUI(bool disable)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(!disable);
        }
    }
}
