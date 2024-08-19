using System;
using UnityEngine;
using static CostToBuild;

[RequireComponent(typeof(Health))]
public abstract class AbstractBaseBuilding : MonoBehaviour
{
    public BuildTypes type;
    public Board ReferencedBoard { get; set; } = null;
    public virtual void Build()
    {
        ReferencedBoard.GetComponent<UnitBase>().OnBaseCaptured.AddListener(OnBaseCaptured);
    }

    public abstract void OnBaseCaptured(Unit.UnitTeam team);
    
    public virtual bool CanBuild(Vector2 position)
    {
        var tilePosition = ReferencedBoard.GetTileByPosition(position);
        if (!PlayerMouse.Inst.CanPayFor(type)) return false;
        return tilePosition != default(BoardTile) && ReferencedBoard.GetBuildingByPosition(position) == null;
    }
}
