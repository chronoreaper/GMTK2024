using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public ResourceTypes Resource { get; private set; } = ResourceTypes.None;
    
    public void Init(ResourceTypes type)
    {
        Resource = type;
        GetComponent<SpriteRenderer>().color = GetResourceColour();
    }

    private Color GetResourceColour()
    {
        switch(Resource)
        {
            case ResourceTypes.None:
                return Color.gray;
            case ResourceTypes.Wood:
                return Color.yellow;
            case ResourceTypes.Lava:
                return Color.red;
            case ResourceTypes.Mountain:
                return Color.black;
            case ResourceTypes.Stone:
                return Color.cyan;
            case ResourceTypes.Water:
                return Color.blue;
        }
        return Color.gray;
    }
}
