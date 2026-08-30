using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(SpriteRenderer))]
public sealed class ArcaneBlastProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0f)] private float speed = 7f;
    [SerializeField, Min(0.01f)] private float maxLifetime = 2.5f;

    [Header("Explosion")]
    [SerializeField, Min(0.1f)] private float blastRadius = 2.5f;
    [SerializeField, Min(0f)] private float damage = 1.5f;
    [SerializeField, Min(0f)] private float knockbackSpeed = 14f;
    [SerializeField, Min(0.01f)] private float knockbackDuration = 0.28f;
    [SerializeField] private LayerMask enemyLayers = ~0;
    [SerializeField] private GameObject impactEffectPrefab;
    [SerializeField, Min(0.01f)] private float impactEffectLifetime = 0.6f;

    [Header("Pixel trail")]
    [SerializeField, Min(0.01f)] private float trailInterval = 0.06f;
    [SerializeField, Min(0.01f)] private float trailLifetime = 0.3f;
    [SerializeField, Range(0.1f, 1f)] private float trailScale = 0.75f;
    [SerializeField] private Color trailColor = new(0.75f, 0.2f, 1f, 0.7f);

    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private Vector2 direction = Vector2.right;
    private Transform owner;
    private float remainingLifetime;
    private float nextTrailTime;
    private bool hasExploded;

    private void Reset()
    {
        Rigidbody2D attachedBody = GetComponent<Rigidbody2D>();
        attachedBody.bodyType = RigidbodyType2D.Kinematic;
        attachedBody.gravityScale = 0f;
        attachedBody.freezeRotation = true;
        attachedBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        CircleCollider2D attachedCollider = GetComponent<CircleCollider2D>();
        attachedCollider.isTrigger = true;
        attachedCollider.radius = 0.25f;
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            Explode();
            return;
        }

        if (Time.time >= nextTrailTime)
        {
            CreateTrailSprite();
            nextTrailTime = Time.time + trailInterval;
        }
    }

    private void FixedUpdate()
    {
        if (!hasExploded)
        {
            body.MovePosition(body.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded || BelongsToOwner(other.transform))
        {
            return;
        }

        EnemyHealth targetHealth = other.GetComponentInParent<EnemyHealth>();
        if (other.isTrigger && targetHealth == null)
        {
            return;
        }

        Explode();
    }

    private void Explode()
    {
        if (hasExploded)
        {
            return;
        }

        hasExploded = true;
        Vector2 center = transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, blastRadius, enemyLayers);
        HashSet<EnemyHealth> affectedEnemies = new();

        foreach (Collider2D hit in hits)
        {
            EnemyHealth health = hit.GetComponentInParent<EnemyHealth>();
            if (health == null || !affectedEnemies.Add(health))
            {
                continue;
            }

            // Il moltiplicatore vale 1 se il player non ha il buff attivo.
            health.TakeDamage(PlayerDamageBuff.Scale(damage));
            if (!health.IsAlive)
            {
                continue;
            }

            Vector2 awayFromImpact = (Vector2)health.transform.position - center;
            KnockbackEffect knockback = health.GetComponent<KnockbackEffect>();
            if (knockback == null)
            {
                knockback = health.gameObject.AddComponent<KnockbackEffect>();
            }

            knockback.ApplyKnockback(awayFromImpact, knockbackSpeed, knockbackDuration);
        }

        if (impactEffectPrefab != null)
        {
            GameObject impact = Instantiate(impactEffectPrefab, center, Quaternion.identity);
            Destroy(impact, impactEffectLifetime);
        }

        Destroy(gameObject);
    }

    private void CreateTrailSprite()
    {
        if (spriteRenderer.sprite == null)
        {
            return;
        }

        GameObject trailObject = new("ArcaneTrail");
        trailObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
        trailObject.transform.localScale = transform.lossyScale * trailScale;

        SpriteRenderer trailRenderer = trailObject.AddComponent<SpriteRenderer>();
        trailRenderer.sprite = spriteRenderer.sprite;
        trailRenderer.sharedMaterial = spriteRenderer.sharedMaterial;
        trailRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
        trailRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;

        ArcaneTrailFade fade = trailObject.AddComponent<ArcaneTrailFade>();
        fade.Initialize(trailColor, trailLifetime);
    }

    private bool BelongsToOwner(Transform hitTransform)
    {
        return owner != null && (hitTransform == owner || hitTransform.IsChildOf(owner));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.75f, 0.25f, 1f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
