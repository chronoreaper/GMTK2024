using UnityEngine;

public class SpawnFromButton : MonoBehaviour
{
    public void Spawn(GameObject prefab) => Instantiate(prefab, Vector3.zero, Quaternion.identity);
}
