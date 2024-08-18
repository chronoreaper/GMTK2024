using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public ResourceTypes Resource { get; private set; } = ResourceTypes.None;
    public Sprite None;
    public Sprite Wood;
    public Sprite Stone;
    public Sprite Lava;
    public Sprite Mountain;
    public Sprite Water;

    public void Init(ResourceTypes type)
    {
        Resource = type;
        //GetComponent<SpriteRenderer>().color = GetResourceColour();
        GetComponent<SpriteRenderer>().sprite = GetSprite();
    }

    private Sprite GetSprite()
    {
        switch (Resource)
        {
            case ResourceTypes.None:
                return None;
            case ResourceTypes.Wood:
                return Wood;
            case ResourceTypes.Lava:
                return Lava;
            case ResourceTypes.Mountain:
                return Mountain;
            case ResourceTypes.Stone:
                return Stone;
            case ResourceTypes.Water:
                return Water;
        }
        return null;

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
