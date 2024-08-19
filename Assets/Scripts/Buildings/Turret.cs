using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Turret : AbstractBaseBuilding
{
    [Header("Turret Settings")] 
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float offsetRotation;

    [Header("Turret Sprites")]
    public Sprite PlayerGun;
    public Sprite PlayerBase;
    public Sprite EnemyGun;
    public Sprite EnemyBase;
    [SerializeField] SpriteRenderer _gunSr;
    [SerializeField] SpriteRenderer _baseSr;

    private Unit _unit;
    private float _timeSinceLastShot;
    
    public override void Build()
    {
        _unit = GetComponent<Unit>();
        UpdateSprite();
        base.Build();
    }

    public override void OnBaseCaptured(Unit.UnitTeam team)
    {
        _unit.Team = _unit.Team == Unit.UnitTeam.Player ? Unit.UnitTeam.Enemy : Unit.UnitTeam.Player;
        UpdateSprite();
    }

    public override bool CanBuild(Vector2 position)
    {
        var tilePosition = ReferencedBoard.GetTileByPosition(position);

        return tilePosition != null && ReferencedBoard.GetBuildingByPosition(position) == null && (tilePosition.Resource == ResourceTypes.None || tilePosition.Resource == ResourceTypes.Water);
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
        
        var targetPosition = (Vector2) enemyCollider.transform.position;
        var direction = targetPosition - (Vector2)transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offsetRotation;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 6 * Time.deltaTime);
    }

    private void UpdateSprite()
    {
        if (_unit.Team == Unit.UnitTeam.Player)
        {
            _gunSr.sprite = PlayerGun;
            _baseSr.sprite = PlayerBase;
        }
        else
        {
            _gunSr.sprite = EnemyGun;
            _baseSr.sprite = EnemyBase;
        }
    }

    private Collider2D GetClosetEnemy()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, range);

        if (colliders.Length == 0)
        {
            return null;
        }
        
        var enemy = colliders.ToList().Find(enemyCollider =>
            enemyCollider.GetComponent<Unit>() != null && enemyCollider.GetComponent<Unit>().Team != _unit.Team);

        if (!enemy)
        {
            return null;
        }
        
        if (enemy.GetComponent<UnitBase>())
        {
            return null;
        }
        
        return enemy;
    }

    private bool CanShoot()
    {
        return _timeSinceLastShot > 1f / (fireRate / 60f);
    }
}
