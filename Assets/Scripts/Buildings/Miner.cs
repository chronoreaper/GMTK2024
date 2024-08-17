using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Miner : AbstractBaseBuilding
{
    [SerializeField] private UnityEvent<int> OnResourceGenerated = new();
    
    [Header("Miner Settings")] 
    [SerializeField] private float timeToGenerate;
    [SerializeField] private int amountToGenerate;
    [SerializeField] private int maxAmount;

    //public override BuildingType Type { get; protected set; } = BuildingType.Miner;
    
    protected ResourceTypes _resource = ResourceTypes.Stone;
    private int _amount;

    public int AmountGenerated
    {
        get => _amount;
        private set
        {
            if (_amount >= maxAmount)
            {
                _amount = maxAmount;
                return;
            }
            
            _amount = value;
        }
    }
    
    public override void Build()
    {
        _resource = Board.GetTileByPosition(transform.position).Resource;
        StartCoroutine(nameof(Process));
    }

    private void Awake() => OnResourceGenerated?.Invoke(AmountGenerated);

    public override bool CanBuild(Vector2 position)
    {
        var tilePosition = Board.GetTileByPosition(position);
        
        return tilePosition != null && tilePosition.Resource != ResourceTypes.None && Board.GetBuildingByPosition(position) == null;
    }

    private IEnumerator Process()
    {
        yield return new WaitForSeconds(timeToGenerate);
        AmountGenerated += amountToGenerate;
        OnResourceGenerated?.Invoke(AmountGenerated);
        
        StartCoroutine(nameof(Process));
    }
}
