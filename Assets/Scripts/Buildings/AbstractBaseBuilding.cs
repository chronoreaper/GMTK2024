using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class AbstractBaseBuilding : MonoBehaviour
{
    public abstract void Build();
    
    public virtual bool CanBuild(Vector2 position)
    {
        var tilePosition = Board.GetTileByPosition(position);
        
        return tilePosition != null && Board.GetBuildingByPosition(position) == null;
    }
}
