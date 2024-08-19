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
    [SerializeField] private float fireRate;
    [SerializeField] private float offsetRotation;
    [SerializeField] private Transform shootPoint;
    
    private Unit _unit;
    private Rigidbody2D _rb;
    private Vector2 _targetDestination;
    private Unit _target;
    private bool _canAttack = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _unit = GetComponent<Unit>();
        StartCoroutine(nameof(RandomDestination));
    }

    private void Update()
    {
        Movement();
        Attack();
        RotateToEnemy(GetClosetEnemy());
    }

    private void Movement()
    {
        if (_target != null && Mathf.Abs(Vector2.Distance(transform.position, _target.transform.position)) <= 5)
        {
            _rb.velocity = new Vector2(0f, 0f);
        }
        
        var direction = _targetDestination - (Vector2)transform.position;

        _rb.velocity = direction.normalized * speed;
    }
    
    private void RotateToEnemy(Collider2D enemyCollider)
    {
        if (enemyCollider == null)
        {
            return;
        }
        
        var targetPosition = (Vector2) enemyCollider.transform.position;
        var direction = targetPosition - (Vector2)transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offsetRotation;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 6 * Time.deltaTime);
    }
    
    private void Attack()
    {
        var enemy = GetClosetEnemy();
        
        if (!enemy)
        {
            _target = null;
            return;
        }

        if (!_canAttack)
        {
            return;
        }

        _target = enemy.GetComponent<Unit>();
        _targetDestination = _target.transform.position;
        
        var bullet = BulletSpawner.Instance.Get();
        bullet.Init(_unit, _target, shootPoint.position, Quaternion.identity);
        
        _canAttack = false;
        Invoke(nameof(ResetAttack), fireRate);
    }
    
    private IEnumerator RandomDestination()
    {
        while (true)
        {
            yield return new WaitUntil(() => !_target);
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

            if (enemy.GetComponent<Ship>())
            {
                return enemy;
            }
            
            if (enemy.GetComponent<UnitBase>())
            {
                return enemy;
            }

            if (enemy.GetComponent<PlayerMovement>())
            {
                return enemy;
            }
        }
        
        return null;
    }
    
    private void ResetAttack()
    {
        _canAttack = true;
    }
}
