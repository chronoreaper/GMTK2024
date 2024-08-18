using UnityEngine;

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
            return;
        _spawnedObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        _spawnedObject.ReferencedBoard = ReferencedBoard;
    }

    public void DisableUI(bool disable)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(!disable);
        }
    }
}