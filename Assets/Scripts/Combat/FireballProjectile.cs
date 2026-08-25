using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(SpriteRenderer))]
public sealed class FireballProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0f)] private float speed = 8f;
    [SerializeField, Min(0.01f)] private float maxLifetime = 2.5f;

    [Header("Hit")]
    [SerializeField, Min(0f)] private float damage = 1f;
    [SerializeField, Min(0f)] private float burnDuration = 2.5f;
    [SerializeField] private GameObject impactEffectPrefab;

    private Rigidbody2D body;
    private Vector2 direction = Vector2.right;
    private Transform owner;
    private float remainingLifetime;
    private bool hasHit;

    private void Reset()
    {
        Rigidbody2D attachedBody = GetComponent<Rigidbody2D>();
        attachedBody.bodyType = RigidbodyType2D.Kinematic;
        attachedBody.gravityScale = 0f;
        attachedBody.freezeRotation = true;
        attachedBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        CircleCollider2D attachedCollider = GetComponent<CircleCollider2D>();
        attachedCollider.isTrigger = true;
        attachedCollider.radius = 0.2f;
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        remainingLifetime = maxLifetime;
    }

    public void Initialize(Vector2 shotDirection, Transform shotOwner)
    {
        direction = shotDirection.sqrMagnitude > 0.001f ? shotDirection.normalized : Vector2.right;
        owner = shotOwner;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Update()
    {
        remainingLifetime -= Time.deltaTime;
        if (remainingLifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit || BelongsToOwner(other.transform))
        {
            return;
        }

        EnemyHealth targetHealth = other.GetComponentInParent<EnemyHealth>();

        // Trigger di servizio, come LevelBounds, non devono bloccare la Fireball.
        if (other.isTrigger && targetHealth == null)
        {
            return;
        }

        hasHit = true;

        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);

            if (targetHealth.IsAlive && targetHealth.TryGetComponent(out BurnPanicEffect panicEffect))
            {
                panicEffect.Ignite(burnDuration);
            }
        }

        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private bool BelongsToOwner(Transform hitTransform)
    {
        return owner != null && (hitTransform == owner || hitTransform.IsChildOf(owner));
    }
}
