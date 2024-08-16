using System;
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
    
    private ResourceTypes _resource = ResourceTypes.STONE;
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

    private void Awake()
    {
        StartCoroutine(nameof(Process));
        OnResourceGenerated?.Invoke(AmountGenerated);
    }

    public void Init(ResourceTypes resourceType)
    {
        _resource = resourceType;
        print(_resource.ToString());
    }

    private IEnumerator Process()
    {
        yield return new WaitForSeconds(timeToGenerate);
        AmountGenerated += amountToGenerate;
        OnResourceGenerated?.Invoke(AmountGenerated);
        
        StartCoroutine(nameof(Process));
    }
}
