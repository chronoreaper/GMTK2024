using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    
    public Unit Source { get; private set; }
    public Unit Target { get; private set; }

    public int Speed = 6;

    SpriteRenderer _sr;

    private void Awake()
    {
        _sr = transform.GetComponentInChildren<SpriteRenderer>();
    }

    public void Init(Unit source, Unit target, Vector2 startPosition, Quaternion rotation)
    {
        Source = source;
        Target = target;
        transform.position = startPosition;
        transform.rotation = rotation;
        _sr.color = Source.GetTeamColour();
        Invoke(nameof(Release), lifeTime);
    }
    
    private void Release() => BulletSpawner.Instance.Release(this);

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
}
