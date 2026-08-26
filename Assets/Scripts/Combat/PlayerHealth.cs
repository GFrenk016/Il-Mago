using UnityEngine;
using UnityEngine.Events;

// Gestisce gli HP del player: danno, breve invulnerabilità dopo un colpo e morte.
// Usa un Instance statico (come PlayerMovement) così la barra della vita e la
// schermata di Game Over lo trovano da sole, senza collegamenti manuali.
public sealed class PlayerHealth : MonoBehaviour
{
    [Header("Vita")]
    [SerializeField, Min(0.01f)] private float maxHealth = 5f;

    [Header("Invulnerabilità dopo un colpo")]
    [SerializeField, Min(0f)] private float invulnerabilityDuration = 1f;

    [Header("Eventi")]
    [SerializeField] private UnityEvent onDamaged;
    [SerializeField] private UnityEvent onDied;
    [SerializeField] private UnityEvent onHealed;

    private float invulnerableUntilTime;

    public static PlayerHealth Instance { get; private set; }

    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }
    public bool IsAlive => CurrentHealth > 0f;
    public bool IsInvulnerable => IsAlive && Time.time < invulnerableUntilTime;

    private void Awake()
    {
        Instance = this;
        CurrentHealth = maxHealth;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive || amount <= 0f || IsInvulnerable)
        {
            return;
        }

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);

        if (IsAlive)
        {
            invulnerableUntilTime = Time.time + invulnerabilityDuration;
        }

        onDamaged?.Invoke();

        if (!IsAlive)
        {
            onDied?.Invoke();
        }
    }

    // Usato dai pickup (es. HealthPickup) per ripristinare HP. Non fa nulla se
    // il player è già morto.
    public void Heal(float amount)
    {
        if (!IsAlive || amount <= 0f)
        {
            return;
        }

        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        onHealed?.Invoke();
    }
}
