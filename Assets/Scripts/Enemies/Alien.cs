using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Unit))]
public class Alien : MonoBehaviour
{
    [Header("Alien Settings")]
    [SerializeField] private float range;
    [SerializeField] private float speed;
    
    private Unit _unit;
    private Rigidbody2D _rb;
    private Vector2 _targetDestination;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _unit = GetComponent<Unit>();
        StartCoroutine(nameof(RandomDestination));
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        var direction = _targetDestination - (Vector2)transform.position;

        _rb.velocity = direction * speed;
    }
    
    private IEnumerator RandomDestination()
    {
        while (true)
        {
            yield return new WaitUntil(() => (Vector2)transform.position != _targetDestination);
            yield return new WaitForSeconds(3);
            _targetDestination = new Vector2(Random.Range(0, 4), Random.Range(0, 4));
        }
    }
    
    /// <summary>
    /// Gets closet enemy with priority (first = unit spawned from base, second = enemy's base, third = player)
    /// </summary>
    /// <returns></returns>
    private Collider2D GetClosetEnemy()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, range);

        if (colliders.Length == 0)
        {
            return null;
        }
        
        var enemies = colliders.ToList().FindAll(enemyCollider =>
            enemyCollider.GetComponent<Unit>() != null && enemyCollider.GetComponent<Unit>().Team != _unit.Team);

        foreach (var enemy in enemies)
        {
            //TODO: Add check for a unit spawned from base

            if (enemy.GetComponent<UnitBase>())
            {
                return enemy;
            }

            if (enemy.GetComponent<PlayerMovement>())
            {
                return enemy;
            }
        }
        
        return enemies[0];
    }
}
