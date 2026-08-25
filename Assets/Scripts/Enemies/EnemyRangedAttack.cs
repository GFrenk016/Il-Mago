using UnityEngine;

// Attacco a distanza generico: implementa la stessa IEnemyAttack di EnemyMeleeAttack,
// quindi EnemyAI lo tratta esattamente allo stesso modo. Usato dallo Skeleton Archer,
// ma riutilizzabile da qualsiasi futuro archetipo ranged cambiando prefab e valori.
[RequireComponent(typeof(EnemyHealth))]
public sealed class EnemyRangedAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private EnemyArrowProjectile arrowPrefab;
    [SerializeField, Min(0f)] private float attackRange = 5f;
    [SerializeField, Min(0.01f)] private float attackCooldown = 1.5f;
    [SerializeField, Min(0f)] private float spawnDistance = 0.5f;

    private EnemyHealth health;
    private float nextAttackTime;

    public float AttackRange => attackRange;

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
    }

    public void TryAttack(Transform target)
    {
        if (target == null || !health.IsAlive || Time.time < nextAttackTime || arrowPrefab == null)
        {
            return;
        }

        Vector2 origin = transform.position;
        Vector2 aimDirection = (Vector2)target.position - origin;
        if (aimDirection.sqrMagnitude <= 0.001f)
        {
            return;
        }

        aimDirection.Normalize();
        Vector2 spawnPosition = origin + aimDirection * spawnDistance;

        EnemyArrowProjectile arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        arrow.Initialize(aimDirection, transform);

        nextAttackTime = Time.time + attackCooldown;
    }
}
