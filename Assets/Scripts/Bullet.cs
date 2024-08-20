using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    
    public Unit Source { get; private set; }
    public Unit Target { get; private set; }

    public int Speed = 6;

    SpriteRenderer _sr;
    TrailRenderer _tr;

    public delegate void OnHitTarget(Collider2D targetCollider, Bullet bullet);
    public static OnHitTarget onHitTarget;

    private void Awake()
    {
        _sr = transform.GetComponentInChildren<SpriteRenderer>();
        _tr = transform.GetComponentInChildren<TrailRenderer>();
    }

    public void Init(Unit source, Unit target, Vector2 startPosition, Quaternion rotation)
    {
        Source = source;
        Target = target;
        transform.position = startPosition;
        transform.rotation = rotation;
        _sr.color = Source.GetTeamColour();
        _tr.startColor = Source.GetTeamColour();
        _tr.endColor = Source.GetTeamColour();
        _tr.Clear();
        GetComponent<AudioSource>().Play();
        CancelInvoke();
        Invoke(nameof(Release), lifeTime);
    }

    private void Release()
    {
        BulletSpawner.Instance.Release(this); 
    }

    void Update()
    {
        // Home in on targets
        if (Target != null)
        {
            _sr.transform.up = (Vector2)(Target.transform.position - transform.position);
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + _sr.transform.up, Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D body)
    {
        body.TryGetComponent(out Unit UnitHit);

        if (UnitHit != null)
        {
            if (UnitHit.Team == Target.Team)
            {
                onHitTarget?.Invoke(UnitHit.GetComponent<Collider2D>(), this);
            }
        }
    }
}
