using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public ResourceTypes Resource { get; private set; } = ResourceTypes.NONE;
    
    public void Init(bool hasResources)
    {
        GetComponent<SpriteRenderer>().color = hasResources ? Color.green : Color.gray;

        Resource = hasResources ? (ResourceTypes) Random.Range(1, 3) : ResourceTypes.NONE;
    }
}
