using UnityEngine;

// Rileva il player generico per qualsiasi archetipo di nemico.
// Usa PlayerMovement.Instance, quindi non servono tag/layer da configurare a mano:
// basta mettere questo componente su qualsiasi nemico e funziona subito.
public sealed class EnemyDetection : MonoBehaviour
{
    [Header("Raggio di rilevamento")]
    [SerializeField, Min(0f)] private float detectionRange = 5f;
    [SerializeField, Min(0f)] private float loseSightRange = 6.5f;

    private bool canSeePlayer;

    public Transform PlayerTransform { get; private set; }
    public bool CanSeePlayer => canSeePlayer && PlayerTransform != null;

    private void Awake()
    {
        if (loseSightRange < detectionRange)
        {
            loseSightRange = detectionRange;
        }
    }

    private void Update()
    {
        PlayerMovement player = PlayerMovement.Instance;
        if (player == null)
        {
            canSeePlayer = false;
            PlayerTransform = null;
            return;
        }

        PlayerTransform = player.transform;
        float distance = Vector2.Distance(transform.position, PlayerTransform.position);

        // Isteresi: usa un raggio più ampio per "perdere" il player rispetto a quello
        // usato per individuarlo, così non oscilla continuamente al bordo del raggio.
        canSeePlayer = canSeePlayer ? distance <= loseSightRange : distance <= detectionRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.65f, 0.1f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = new Color(1f, 0.2f, 0.1f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, loseSightRange);
    }
}
