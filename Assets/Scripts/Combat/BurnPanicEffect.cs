using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(EnemyHealth))]
public sealed class BurnPanicEffect : MonoBehaviour
{
    [Header("Visual selected in the Inspector")]
    [SerializeField] private GameObject flamesVisual;

    [Header("Panic movement")]
    [SerializeField, Min(0f)] private float panicSpeed = 8f;
    [SerializeField, Min(0.01f)] private float minDirectionTime = 0.12f;
    [SerializeField, Min(0.01f)] private float maxDirectionTime = 0.35f;

    [Header("Burn damage")]
    [SerializeField, Min(0f)] private float damagePerTick = 0.25f;
    [SerializeField, Min(0.05f)] private float damageTickInterval = 0.5f;

    private Rigidbody2D body;
    private EnemyHealth health;
    private SlowEffect slowEffect;
    private KnockbackEffect knockbackEffect;
    private Vector2 panicDirection;
    private float burnEndTime;
    private float nextDirectionTime;
    private float nextDamageTime;

    public bool IsBurning { get; private set; }

    private void Reset()
    {
        Rigidbody2D attachedBody = GetComponent<Rigidbody2D>();
        attachedBody.gravityScale = 0f;
        attachedBody.freezeRotation = true;
        attachedBody.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();
        slowEffect = GetComponent<SlowEffect>();
        knockbackEffect = GetComponent<KnockbackEffect>();
        SetFlamesVisible(false);
    }

    private void Update()
    {
        if (!IsBurning)
        {
            return;
        }

        if (Time.time >= nextDamageTime)
        {
            ApplyBurnDamage();
        }

        if (Time.time >= burnEndTime || !health.IsAlive)
        {
            StopBurning();
            return;
        }

        if (Time.time >= nextDirectionTime)
        {
            ChooseNewDirection();
        }
    }

    private void FixedUpdate()
    {
        if (IsBurning)
        {
            if (knockbackEffect == null)
            {
                knockbackEffect = GetComponent<KnockbackEffect>();
            }

            if (knockbackEffect != null && knockbackEffect.IsKnockedBack)
            {
                return;
            }

            float speedMultiplier = slowEffect != null ? slowEffect.MovementMultiplier : 1f;
            body.MovePosition(body.position + panicDirection * panicSpeed * speedMultiplier * Time.fixedDeltaTime);
        }
    }

    public void Ignite(float duration)
    {
        if (duration <= 0f)
        {
            return;
        }

        bool wasAlreadyBurning = IsBurning;
        IsBurning = true;
        burnEndTime = Mathf.Max(burnEndTime, Time.time + duration);

        if (!wasAlreadyBurning)
        {
            nextDamageTime = Time.time + damageTickInterval;
        }

        SetFlamesVisible(true);
        ChooseNewDirection();
    }

    private void ApplyBurnDamage()
    {
        health.TakeDamage(damagePerTick);
        nextDamageTime = Time.time + damageTickInterval;
    }

    private void ChooseNewDirection()
    {
        panicDirection = Random.insideUnitCircle.normalized;
        if (panicDirection.sqrMagnitude <= 0.001f)
        {
            panicDirection = Vector2.right;
        }

        float minimum = Mathf.Min(minDirectionTime, maxDirectionTime);
        float maximum = Mathf.Max(minDirectionTime, maxDirectionTime);
        nextDirectionTime = Time.time + Random.Range(minimum, maximum);
    }

    private void StopBurning()
    {
        IsBurning = false;
        panicDirection = Vector2.zero;
        SetFlamesVisible(false);
    }

    private void SetFlamesVisible(bool visible)
    {
        if (flamesVisual != null && flamesVisual != gameObject)
        {
            flamesVisual.SetActive(visible);
        }
    }

    private void OnDisable()
    {
        IsBurning = false;
        SetFlamesVisible(false);
    }
}
