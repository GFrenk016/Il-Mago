using UnityEngine;

// Il "cervello" del nemico: collega rilevamento, movimento, salute e attacco.
// Questo script NON cambia mai tra un archetipo e l'altro: quello che cambia sono
// i valori nell'Inspector (velocità, raggio, danno...) e quale componente di attacco
// viene usato (es. EnemyMeleeAttack per il Goblin, un futuro EnemyRangedAttack per lo
// Skeleton Archer). Per questo il sistema è "generico e riutilizzabile".
[RequireComponent(typeof(EnemyMovement), typeof(EnemyDetection), typeof(EnemyHealth))]
public sealed class EnemyAI : MonoBehaviour
{
    [SerializeField, Min(0f)] private float attackRangeBuffer = 0.05f;

    private EnemyMovement movement;
    private EnemyDetection detection;
    private EnemyHealth health;
    private IEnemyAttack attack;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        detection = GetComponent<EnemyDetection>();
        health = GetComponent<EnemyHealth>();
        attack = GetComponent<IEnemyAttack>();
    }

    private void Update()
    {
        if (!health.IsAlive)
        {
            movement.Stop();
            return;
        }

        if (!detection.CanSeePlayer)
        {
            movement.Stop();
            return;
        }

        Transform player = detection.PlayerTransform;
        float attackRange = attack != null ? attack.AttackRange : 0f;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (attack != null && distanceToPlayer <= attackRange + attackRangeBuffer)
        {
            movement.Stop();
            movement.Face(player.position - transform.position);
            attack.TryAttack(player);
        }
        else
        {
            movement.MoveTowards(player.position);
        }
    }
}
