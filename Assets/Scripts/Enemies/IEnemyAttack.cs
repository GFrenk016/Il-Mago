using UnityEngine;

// Contratto comune per qualsiasi tipo di attacco nemico (melee, a distanza, ecc.).
// EnemyAI parla solo con questa interfaccia: per creare un nuovo archetipo con un
// comportamento d'attacco diverso basta aggiungere un nuovo componente che la implementa
// (es. EnemyRangedAttack per lo Skeleton Archer), senza toccare EnemyAI.
public interface IEnemyAttack
{
    float AttackRange { get; }

    void TryAttack(Transform target);
}
