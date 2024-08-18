using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class AbstractBaseBuilding : MonoBehaviour
{
    public Board ReferencedBoard { get; set; } = null;

    public abstract void Build();
    
    public virtual bool CanBuild(Vector2 position)
    {
        var tilePosition = ReferencedBoard.GetTileByPosition(position);
        
        return tilePosition != null && ReferencedBoard.GetBuildingByPosition(position) == null && tilePosition.Resource != ResourceTypes.Mountain;
    }
}
