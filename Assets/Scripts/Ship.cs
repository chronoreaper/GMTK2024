using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Ship : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float offsetRotation;
    
    public Bullet Shot;
    public float Speed = 5f;
    public float VisionRange = 5f;
    public float AtkRange = 3f;
    public float AtkRate = 1f;

    bool _canAttack = true;
    Unit _unit = null;
    Unit _target = null;
    SpriteRenderer _sr;

    Vector2 _targetPos = new Vector3();

    public void MoveTowards(Vector2 pos)
    {
        _targetPos = pos;
    }

    public void Init(Vector2 position, Quaternion rotation, Unit.UnitTeam team)
    {
        transform.position = position;
        transform.rotation = rotation;
        _unit.Team = team;
    }


    private void Awake()
    {
        _sr = transform.GetComponentInChildren<SpriteRenderer>();
        _unit = transform.GetComponent<Unit>();
    }

    void Start()
    {
        _targetPos = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        _target = GetClosestEnemy(VisionRange);
        

        // If you are in attack range and there is a target
        bool inAtkRange = Mathf.Abs(((Vector2)transform.position - _targetPos).magnitude) <= AtkRange;
        inAtkRange &= _target != null;
        if (inAtkRange)
        {
            Attack();
        }

        bool shouldMove = true;
        if (_unit.Team == Unit.UnitTeam.Enemy)
        {
            if (_target != null)
            {
                _targetPos = _target.transform.position;
                RotateToEnemy(_target);
            }
            if (inAtkRange)
                shouldMove = false;
        }
        else
        {
            shouldMove = Mathf.Abs(((Vector2)transform.position - _targetPos).sqrMagnitude) > 0.1;
        }



        if (shouldMove)
        {
            float movementMultiplier = 1;
            if (!_canAttack)
                movementMultiplier = 0.5f;
            _sr.transform.up = _targetPos - (Vector2)transform.position;
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, movementMultiplier * Speed * Time.deltaTime);
        }
    }

    private void RotateToEnemy(Unit enemy)
    {
        if (enemy == null)
        {
            return;
        }
        
        var targetPosition = (Vector2) enemy.transform.position;
        var direction = targetPosition - (Vector2)transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offsetRotation;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 6 * Time.deltaTime);
    }
    
    private Unit GetClosestEnemy(float radius = 3)
    {
        Unit closest = null;
        float dist = float.MaxValue;
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position,radius);
        // Get closest target that is not on team
        foreach(Collider2D result in results)
        {
            Unit other = result.GetComponent<Unit>();
            if (other == null)
                continue;
            float distTo = (transform.position - other.transform.position).sqrMagnitude;
            if (other.Team != _unit.Team && distTo < dist)
            {
                closest = other;
                dist = distTo;
            }
        }

        return closest;

    }

    private void Attack()
    {
        if (!_canAttack)
            return;
        if (_target == null)
            return;

        // May need to implement a resource manager that simply pools objects instead of instantiate them each time
        var bullet = BulletSpawner.Instance.Get();
        bullet.Init(_unit, _target, shootPoint.localPosition, Quaternion.identity);

        _canAttack = false;
        Invoke(nameof(ResetAttack), AtkRate);
    }

    private void ResetAttack()
    {
        _canAttack = true;
    }
}
