using UnityEngine;
using UnityEngine.Events;

public sealed class EnemyHealth : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float maxHealth = 3f;
    [SerializeField] private UnityEvent onDamaged;
    [SerializeField] private UnityEvent onDied;

    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }
    public bool IsAlive => CurrentHealth > 0;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive || amount <= 0)
        {
            return;
        }

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        onDamaged?.Invoke();

        if (!IsAlive)
        {
            onDied?.Invoke();
            Destroy(gameObject);
        }
    }
}
