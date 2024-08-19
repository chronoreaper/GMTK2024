using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class AbstractBaseBuilding : MonoBehaviour
{
    public Board ReferencedBoard { get; set; } = null;

    public virtual void Build()
    {
        ReferencedBoard.GetComponent<UnitBase>().OnBaseCaptured.AddListener(OnBaseCaptured);
    }

    public abstract void OnBaseCaptured(Unit.UnitTeam team);
    
    public virtual bool CanBuild(Vector2 position)
    {
        var tilePosition = ReferencedBoard.GetTileByPosition(position);
        
        return tilePosition != null && ReferencedBoard.GetBuildingByPosition(position) == null && tilePosition.Resource != ResourceTypes.Mountain;
    }
}
