using UnityEngine;

// Movimento generico riutilizzabile da tutti gli archetipi di nemici.
// EnemyAI decide "dove" andare, questo componente si occupa solo del "come" muoversi,
// rispettando eventuali SlowEffect / KnockbackEffect già presenti sul nemico.
[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public sealed class EnemyMovement : MonoBehaviour
{
    [SerializeField, Min(0f)] private float moveSpeed = 2.2f;

    private Rigidbody2D body;
    private SlowEffect slowEffect;
    private KnockbackEffect knockbackEffect;
    private Vector2 desiredDirection;

    public Vector2 FacingDirection { get; private set; } = Vector2.down;
    public bool IsMoving => desiredDirection.sqrMagnitude > 0.001f;

    private void Reset()
    {
        Rigidbody2D attachedBody = GetComponent<Rigidbody2D>();
        attachedBody.gravityScale = 0f;
        attachedBody.freezeRotation = true;
        attachedBody.interpolation = RigidbodyInterpolation2D.Interpolate;

        CapsuleCollider2D attachedCollider = GetComponent<CapsuleCollider2D>();
        attachedCollider.direction = CapsuleDirection2D.Vertical;
        attachedCollider.size = new Vector2(0.8125f, 1f);
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        slowEffect = GetComponent<SlowEffect>();
        knockbackEffect = GetComponent<KnockbackEffect>();
    }

    // Chiamato ogni frame da EnemyAI con la posizione da inseguire (es. il player).
    public void MoveTowards(Vector2 targetPosition)
    {
        Vector2 offset = targetPosition - body.position;
        desiredDirection = offset.sqrMagnitude > 0.0004f ? offset.normalized : Vector2.zero;

        if (desiredDirection.sqrMagnitude > 0.001f)
        {
            FacingDirection = desiredDirection;
        }
    }

    public void Face(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.001f)
        {
            FacingDirection = direction.normalized;
        }
    }

    public void Stop()
    {
        desiredDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (knockbackEffect == null)
        {
            knockbackEffect = GetComponent<KnockbackEffect>();
        }

        if (knockbackEffect != null && knockbackEffect.IsKnockedBack)
        {
            return;
        }

        if (desiredDirection.sqrMagnitude <= 0.001f)
        {
            return;
        }

        if (slowEffect == null)
        {
            slowEffect = GetComponent<SlowEffect>();
        }

        float speedMultiplier = slowEffect != null ? slowEffect.MovementMultiplier : 1f;
        Vector2 nextPosition = body.position + desiredDirection * (moveSpeed * speedMultiplier * Time.fixedDeltaTime);
        body.MovePosition(nextPosition);
    }

    private void OnDisable()
    {
        desiredDirection = Vector2.zero;
    }
}
