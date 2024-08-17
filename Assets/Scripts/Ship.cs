using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Unit))]
public class Ship : MonoBehaviour
{
    public Bullet Shot;
    public PlayerControls PlayerControls;
    public float Speed = 5f;
    public float VisionRange = 5f;
    public float AtkRange = 3f;
    public float AtkRate = 1f;

    bool _canAttack = true;
    Unit _unit = null;
    Unit _target = null;
    InputAction _click;
    InputAction _cursorPosition;
    SpriteRenderer _sr;

    Vector2 _targetPos = new Vector3();

    private void Awake()
    {
        PlayerControls = new PlayerControls();
        _sr = transform.GetComponentInChildren<SpriteRenderer>();
        _unit = transform.GetComponent<Unit>();
    }

    void Start()
    {
        _targetPos = transform.position;
    }

    private void OnEnable()
    {
        _click = PlayerControls.Player.Click;
        _cursorPosition = PlayerControls.Player.CusorPosition;
        _click.Enable();
        _click.performed += Click;
        _cursorPosition.Enable();
    }

    private void OnDisable()
    {
        _click.Disable();
        _cursorPosition.Disable();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.collider.GetComponent<Bullet>();
        // This may cause an error if the bullet Soruce is killed before the bullet is hit.
        if (bullet != null && bullet.Source.Team != _unit.Team)
        {
            _unit.Hp -= 1;

            if (_unit.Hp <= 0)
                Destroy(gameObject);
            Destroy(bullet.gameObject);
        }
    }

    private void Click(InputAction.CallbackContext obj)
    {
        if (_unit.Team == Unit.UnitTeam.Player)
            _targetPos = Camera.main.ScreenToWorldPoint(_cursorPosition.ReadValue<Vector2>());
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
        Bullet bullet = Instantiate(Shot, transform.position, Quaternion.identity);
        bullet.Source = _unit;
        bullet.Target = _target;

        _canAttack = false;
        Invoke(nameof(ResetAttack), AtkRate);
    }

    private void ResetAttack()
    {
        _canAttack = true;
    }
}
