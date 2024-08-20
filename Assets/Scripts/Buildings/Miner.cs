using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static CostToBuild;

public class Miner : AbstractBaseBuilding
{
    [Header("Miner Settings")] 
    [SerializeField] private float timeToGenerate;
    [SerializeField] private int amountToGenerate;
    
    protected ResourceTypes _resource = ResourceTypes.Stone;
    private int _amount;
    
    public override void Build()
    {
        base.Build();
        _resource = ReferencedBoard.GetTileByPosition(transform.position).Resource;

        // Some resource changes
        if (_resource == ResourceTypes.Mountain)
            _resource = ResourceTypes.Stone;

        StartCoroutine(nameof(Process));
    }

    public override void OnBaseCaptured(Unit.UnitTeam team)
    {
        if (team == Unit.UnitTeam.Enemy)
        {
            StopAllCoroutines();
        }

        if (team == Unit.UnitTeam.Player)
        {
            StartCoroutine(nameof(Process));
        }
    }

    public override bool CanBuild(Vector2 position)
    {
        var tilePosition = ReferencedBoard.GetTileByPosition(position);
        
        return base.CanBuild(position) && tilePosition.Resource != ResourceTypes.None;
    }

    private IEnumerator Process()
    {
        PlayerMouse.Inst.GainResources(_resource, amountToGenerate);
        yield return new WaitForSeconds(timeToGenerate);
        
        StartCoroutine(nameof(Process));
    }
}
