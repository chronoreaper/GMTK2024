using UnityEngine;

public class SpawnFromButton : MonoBehaviour
{
    private GameObject _spawnedObject = null;
    public void Spawn(GameObject prefab)
    {
        if (_spawnedObject != null)
            return;
        _spawnedObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

    public void DisableUI(bool disable)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(!disable);
        }
    }
}
