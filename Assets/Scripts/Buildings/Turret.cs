using System.Linq;
using UnityEngine;

public class Turret : AbstractBaseBuilding
{
    [Header("Turret Settings")] 
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float offsetRotation;

    private Unit _target;
    private float _timeSinceLastShot;
    
    public override void Build()
    {
        
    }

    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
        
        Attack();
        RotateToEnemy(GetClosetEnemy().transform.position);
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
        
        var direction = enemy.transform.position - transform.position;

        var raycast = Physics2D.Raycast(transform.position, direction, range);
        
        if (raycast.collider.GetComponent<Unit>())
        {
            //TODO: Make Enemies Take Damage
            //enemy.
            print("gfgfdfgdgfdgf");
        }

        _timeSinceLastShot = 0;
    }

    private void RotateToEnemy(Vector2 enemyPosition)
    {
        var direction = enemyPosition - (Vector2)transform.position;
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
