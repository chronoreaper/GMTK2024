using System.Collections;
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

    private float _numberOfShipsSpawned;

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
        maxSpawnAmount = Random.Range(1, maxSpawnAmount);
        WinManager.Instance.UpdateConquerPercentage(Team);
        StartCoroutine(nameof(ResetEnemySpawn));
    }

    private IEnumerator ResetEnemySpawn()
    {
        yield return new WaitForSeconds(Random.value * resetTime);
        _numberOfShipsSpawned = 0;
        StopCoroutine(nameof(SpawnUnit));
        StartCoroutine(nameof(SpawnUnit));
    }

    private IEnumerator SpawnUnit()
    {
        yield return new WaitUntil(() => _numberOfShipsSpawned < maxSpawnAmount);
        yield return new WaitForSeconds(Random.Range(enemySpawnTime, enemySpawnTime * 1.5f));
        if (Team != UnitTeam.Player)
        {
            var ship = ShipSpawner.Instance.Get();
            _numberOfShipsSpawned++;

            var angle = Random.Range(0f, 360f);
            var position = new Vector2(transform.position.x + Mathf.Sin(angle) * Mathf.Deg2Rad, transform.position.y + Mathf.Cos(angle) * Mathf.Deg2Rad) * spawnRange;
            var rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 361));

            ship.Init(position, rotation, Team);
        }
        StartCoroutine(nameof(SpawnUnit));
    }

    public string GetCustomData()
    {
        return Team.ToString();
    }
}
