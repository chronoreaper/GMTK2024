using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Unit Source { get; set; }
    public Unit Target { get; set; }

    public int Speed = 6;

    SpriteRenderer _sr;

    private void Awake()
    {
        _sr = transform.GetComponentInChildren<SpriteRenderer>();
    }
    void Start()
    {
        _sr.color = Source.GetTeamColour();
        // Don't last more than a few seconds
        Destroy(gameObject, 2);
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
}
