using UnityEngine;
using UnityEngine.Serialization;

public class BulletSpawner : PoolerBase<Bullet>
{
    private static BulletSpawner _instance;

    public static BulletSpawner Instance
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

    [FormerlySerializedAs("_bulletPrefab")] [SerializeField] private Bullet bulletPrefab;
    
    private void Awake()
    {
        Instance = this;
        
        InitPool(bulletPrefab, 50, 75);
    }
}
