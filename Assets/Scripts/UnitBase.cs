using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HealthBase))]
public class UnitBase : Unit
{
    [Header("Base Settings")] 
    [SerializeField] private float resetTime;
    [SerializeField] private float enemySpawnTime;
    [SerializeField] private float spawnRange = 1;
    
    public readonly UnityEvent<UnitTeam> OnBaseCaptured = new();
    
    [SerializeField] private SpriteRenderer playerBaseSprite;
    [SerializeField] private SpriteRenderer enemyBaseSprite;

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
        StartCoroutine(nameof(ResetEnemySpawn));
    }

    private IEnumerator ResetEnemySpawn()
    {
        yield return new WaitForSeconds(resetTime);
        StartCoroutine(nameof(SpawnUnit));
    }

    private IEnumerator SpawnUnit()
    {
        yield return new WaitForSeconds(enemySpawnTime);
        var ship = ShipSpawner.Instance.Get();
        
        var angle = Random.Range(0f, 360f);
        var position = new Vector2(transform.position.x + Mathf.Sin(Mathf.Deg2Rad * angle),transform.position.y + Mathf.Cos(Mathf.Deg2Rad * angle)) * spawnRange;
        var rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 361));
        
        ship.Init(position, rotation, Team);
    }
}
