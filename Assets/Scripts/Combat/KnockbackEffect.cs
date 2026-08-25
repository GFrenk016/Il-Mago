using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class KnockbackEffect : MonoBehaviour
{
    private Rigidbody2D body;
    private Vector2 direction;
    private float initialSpeed;
    private float startTime;
    private float endTime;

    public bool IsKnockedBack { get; private set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Vector2 knockbackDirection, float speed, float duration)
    {
        if (speed <= 0f || duration <= 0f)
        {
            return;
        }

        direction = knockbackDirection.sqrMagnitude > 0.001f
            ? knockbackDirection.normalized
            : Vector2.right;

        initialSpeed = speed;
        startTime = Time.time;
        endTime = Time.time + duration;
        IsKnockedBack = true;
    }

    private void FixedUpdate()
    {
        if (!IsKnockedBack)
        {
            return;
        }

        if (Time.time >= endTime)
        {
            IsKnockedBack = false;
            return;
        }

        float remaining = Mathf.InverseLerp(endTime, startTime, Time.time);
        float currentSpeed = initialSpeed * Mathf.Lerp(0.25f, 1f, remaining);
        body.MovePosition(body.position + direction * currentSpeed * Time.fixedDeltaTime);
    }

    private void OnDisable()
    {
        IsKnockedBack = false;
    }
}
