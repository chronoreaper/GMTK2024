using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    private float _currentHealth;

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
            
            if (value < 0)
            {
                _currentHealth = 0;
                Kill();
                return;
            }

            if (value == 0)
            {
                Kill();
            }

            _currentHealth = value;
        }
    }

    private void Awake() => CurrentHealth = maxHealth;
    
    public void Damage(float damage) => CurrentHealth -= damage;

    public void Heal(float amount) => CurrentHealth += amount;

    private void Kill()
    {
        //TODO: Spawn some particles, play sound
        Destroy(gameObject);
    }
}
