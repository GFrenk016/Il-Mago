using UnityEngine;

public sealed class SlowEffect : MonoBehaviour
{
    [Header("Visual selected in the Inspector")]
    [SerializeField] private GameObject slowVisual;
    [SerializeField] private SpriteRenderer tintTarget;
    [SerializeField] private Color slowedColor = new(0.45f, 0.85f, 1f, 1f);

    private Color originalColor = Color.white;
    private float slowEndTime;

    public bool IsSlowed { get; private set; }
    public float MovementMultiplier { get; private set; } = 1f;

    private void Awake()
    {
        if (tintTarget == null)
        {
            tintTarget = GetComponentInChildren<SpriteRenderer>(true);
        }

        if (tintTarget != null)
        {
            originalColor = tintTarget.color;
        }

        SetSlowVisual(false);
    }

    private void Update()
    {
        if (IsSlowed && Time.time >= slowEndTime)
        {
            RemoveSlow();
        }
    }

    public void ApplySlow(float duration, float speedMultiplier)
    {
        if (duration <= 0f)
        {
            return;
        }

        float clampedMultiplier = Mathf.Clamp(speedMultiplier, 0.05f, 1f);
        MovementMultiplier = IsSlowed
            ? Mathf.Min(MovementMultiplier, clampedMultiplier)
            : clampedMultiplier;

        IsSlowed = true;
        slowEndTime = Mathf.Max(slowEndTime, Time.time + duration);
        SetSlowVisual(true);

        if (tintTarget != null)
        {
            tintTarget.color = slowedColor;
        }
    }

    private void RemoveSlow()
    {
        IsSlowed = false;
        MovementMultiplier = 1f;
        SetSlowVisual(false);

        if (tintTarget != null)
        {
            tintTarget.color = originalColor;
        }
    }

    private void SetSlowVisual(bool visible)
    {
        if (slowVisual != null && slowVisual != gameObject)
        {
            slowVisual.SetActive(visible);
        }
    }

    private void OnDisable()
    {
        IsSlowed = false;
        MovementMultiplier = 1f;
        SetSlowVisual(false);

        if (tintTarget != null)
        {
            tintTarget.color = originalColor;
        }
    }
}
