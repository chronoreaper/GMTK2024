using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Turret : AbstractBaseBuilding
{
    [Header("Turret Settings")] 
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float offsetRotation;

    private Unit _unit;
    private float _timeSinceLastShot;
    
    public override void Build()
    {
        
    }

    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
        
        Attack();
        RotateToEnemy(GetClosetEnemy());
    }

    private void Attack()
    {
        if (!CanShoot())
        {
            return;
        }
        
        var enemyCollider = GetClosetEnemy();
        
        if (enemyCollider == null)
        {
            return;
        }

        var enemy = enemyCollider.GetComponent<Unit>();

        if (enemy.Team != Unit.UnitTeam.Enemy)
        {
            return;
        }
        
        if (Physics2D.Raycast(transform.position, transform.up, range))
        {
            enemy.GetComponent<Health>().Damage(damage, _unit);
        }

        _timeSinceLastShot = 0;
    }

    private void RotateToEnemy(Collider2D enemyCollider)
    {
        if (enemyCollider == null)
        {
            return;
        }

        var enemy = enemyCollider.GetComponent<Unit>();

        if (enemy.Team != Unit.UnitTeam.Enemy)
        {
            return;
        }
        
        var targetPosition = (Vector2) enemyCollider.transform.position;
        var direction = targetPosition - (Vector2)transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offsetRotation;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 6 * Time.deltaTime);
    }

    private Collider2D GetClosetEnemy()
    {
        return Physics2D.OverlapCircle(transform.position, range);
    }

    private bool CanShoot()
    {
        return _timeSinceLastShot > 1f / (fireRate / 60f);
    }
}
