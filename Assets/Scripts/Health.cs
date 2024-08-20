using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;

public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected GameObject deathParticles;

    private float _currentHealth;
    private SpriteRenderer[] _sr;
    private Material _defaultMaterial;
    private Material _hitMaterial;
    private AudioClip _hitSound;
    private AudioClip _defeatSound;
    private AudioSource _audioSource;

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
                AudioManager.Inst.Play(_defeatSound);
                Kill();
                return;
            }

            _currentHealth = value;
        }
    }

    private void Awake()
    {
        _sr = GetComponentsInChildren<SpriteRenderer>();
        _defaultMaterial = new Material(Shader.Find("Sprites/Default"));
        _hitMaterial = Resources.Load<Material>("SetColor"); //new Material(Shader.Find("Shader Graphs/SetColour"));
        _hitSound = Resources.Load<AudioClip>("sHitSlash");
        _defeatSound = Resources.Load<AudioClip>("sGroundPound");
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.outputAudioMixerGroup = Resources.Load<AudioMixerGroup>("Master");
        _audioSource.volume = 0.7f;
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
        foreach(var sr in _sr)
            sr.material = _hitMaterial;
        _audioSource.PlayOneShot(_hitSound);
        CancelInvoke();
        Invoke(nameof(ResetMaterial), 0.2f);
    }

    public void Heal(float amount) => CurrentHealth += amount;

    protected virtual void Kill()
    {
        //TODO: Spawn some particles, play sound
        var ship = GetComponent<Ship>();
        if (ship)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            ShipSpawner.Instance.Release(ship);
            return;
        }
        
        Destroy(gameObject);
    }

    public void SetMaxHealth(float hp)
    {
        maxHealth = hp;
    }

    private void ResetMaterial()
    {
        foreach (var sr in _sr)
            sr.material = _defaultMaterial;
    }
}
