using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HealthBase))]
public class UnitBase : Unit, IGetCustomTip
{
    [Header("Base Settings")] 
    [SerializeField] private float resetTime;
    [SerializeField] private float enemySpawnTime;
    [SerializeField] private float spawnRange = 1;
    [SerializeField] private int maxSpawnAmount = 1;
    
    public readonly UnityEvent<UnitTeam> OnBaseCaptured = new();
    
    [SerializeField] private SpriteRenderer playerBaseSprite;
    [SerializeField] private SpriteRenderer enemyBaseSprite;
    private List<Ship> _spawned = new();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        UpdateColor();
    }

    protected override void UpdateColor()
    {
        if (Team == UnitTeam.Player)
        {
            playerBaseSprite.gameObject.SetActive(true);
            enemyBaseSprite.gameObject.SetActive(false);
        }

        if (Team == UnitTeam.Enemy)
        {
            enemyBaseSprite.gameObject.SetActive(true);
            playerBaseSprite.gameObject.SetActive(false);
        }
        
        OnBaseCaptured?.Invoke(Team);
        maxSpawnAmount = Random.Range(5, maxSpawnAmount);
        WinManager.Instance.UpdateConquerPercentage(Team);
        StartCoroutine(nameof(ResetEnemySpawn));
    }

    private IEnumerator ResetEnemySpawn()
    {
        yield return new WaitForSeconds(Random.value * resetTime);
        _spawned.Clear();
        StopCoroutine(nameof(SpawnUnit));
        StartCoroutine(nameof(SpawnUnit));
    }

    private IEnumerator SpawnUnit()
    {
        int i = 0;
        while (i < _spawned.Count)
        {
            if (!_spawned[i].isActiveAndEnabled)
            {
                _spawned.RemoveAt(i);
            }
            else
                i++;
        }

        //yield return new WaitUntil(() => _spawned.Count < maxSpawnAmount);
        yield return new WaitForSeconds(Random.Range(enemySpawnTime, enemySpawnTime * 2f));

        if (Team != UnitTeam.Player && _spawned.Count < maxSpawnAmount)
        {
            var ship = ShipSpawner.Instance.Get();

            var angle = Random.Range(0f, 360f);
            var position = new Vector2(transform.position.x + Random.Range(-spawnRange, spawnRange), transform.position.y + Random.Range(-spawnRange, spawnRange));
            var rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 361));

            ship.Init(position, rotation, Team);
            _spawned.Add(ship);
        }
        StartCoroutine(nameof(SpawnUnit));
    }

    public string GetCustomData()
    {
        return Team.ToString();
    }
}
