using UnityEngine;

public class ShipSpawner : PoolerBase<Ship>
{
    private static ShipSpawner _instance;

    public static ShipSpawner Instance
    {
        get => _instance;
        private set
        {
            if (_instance == null)
            {
                _instance = value;
            }

            if (_instance != value)
            {
                Destroy(value);
            }
        }
    }

    [SerializeField] private Ship shipPrefab;
    
    private void Awake()
    {
        Instance = this;
        
        InitPool(shipPrefab, 15, 25);
    }
}
