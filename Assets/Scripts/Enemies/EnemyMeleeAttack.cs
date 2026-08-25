using UnityEngine;

// Attacco corpo a corpo generico. Usato dal Goblin, ma riutilizzabile da qualsiasi
// archetipo melee (es. un futuro Orc) semplicemente cambiando i valori nell'Inspector.
[RequireComponent(typeof(EnemyHealth))]
public sealed class EnemyMeleeAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField, Min(0f)] private float attackRange = 0.9f;
    [SerializeField, Min(0f)] private float damage = 1f;
    [SerializeField, Min(0.01f)] private float attackCooldown = 1f;

    private EnemyHealth health;
    private float nextAttackTime;

    public float AttackRange => attackRange;

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
    }

    public void TryAttack(Transform target)
    {
        if (target == null || !health.IsAlive || Time.time < nextAttackTime)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance > attackRange)
        {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;

        if (target.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
