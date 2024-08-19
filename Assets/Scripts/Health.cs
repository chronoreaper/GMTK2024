using Unity.Mathematics;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth;

    private float _currentHealth;
    protected Unit _lastDamagedBy;

    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            if (value > maxHealth)
            {
                _currentHealth = maxHealth;
                return;
            } 

            if (value <= 0)
            {
                _currentHealth = 0;
                Kill();
                return;
            }

            _currentHealth = value;
        }
    }

    private void Start()
    {
        CurrentHealth = maxHealth;     
    }

    private void Damage(float damage) => CurrentHealth -= damage;
    public void Damage(float damage, Unit source)
    {
        Damage(damage);
        _lastDamagedBy = source;
    }

    public void Heal(float amount) => CurrentHealth += amount;

    protected virtual void Kill()
    {
        //TODO: Spawn some particles, play sound
        Destroy(gameObject);
    }

    public void SetMaxHealth(float hp)
    {
        maxHealth = hp;
    }
}
