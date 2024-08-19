using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static CostToBuild;

public class Miner : AbstractBaseBuilding
{
    [Header("Miner Settings")] 
    [SerializeField] private float timeToGenerate;
    [SerializeField] private int amountToGenerate;
    [SerializeField] private int maxAmount;
    
    protected ResourceTypes _resource = ResourceTypes.Stone;
    private int _amount;

    public int AmountGenerated
    {
        get => _amount;
        private set
        {
            if (_amount >= maxAmount)
            {
                _amount = 0;
                return;
            }
            
            _amount = value;
        }
    }
    
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
        yield return new WaitForSeconds(timeToGenerate);
        AmountGenerated += amountToGenerate;
        PlayerMouse.Inst.GainResources(_resource, amountToGenerate);
        
        StartCoroutine(nameof(Process));
    }
}
