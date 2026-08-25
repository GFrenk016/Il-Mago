using UnityEngine;

// Feedback visivo quando il player viene colpito: lo sprite lampeggia per tutta
// la durata dell'invulnerabilità, così si capisce sia che si è preso danno sia
// per quanto tempo si è temporaneamente immuni. Nessun collegamento manuale:
// legge direttamente lo stato del PlayerHealth sullo stesso GameObject.
[RequireComponent(typeof(SpriteRenderer), typeof(PlayerHealth))]
public sealed class PlayerDamageFlash : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float blinkInterval = 0.08f;
    [SerializeField, Range(0f, 1f)] private float dimmedAlpha = 0.25f;

    private SpriteRenderer spriteRenderer;
    private PlayerHealth health;
    private float nextBlinkTime;
    private bool isDimmed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (!health.IsInvulnerable)
        {
            if (isDimmed)
            {
                SetAlpha(1f);
                isDimmed = false;
            }
            return;
        }

        if (Time.time < nextBlinkTime)
        {
            return;
        }

        isDimmed = !isDimmed;
        SetAlpha(isDimmed ? dimmedAlpha : 1f);
        nextBlinkTime = Time.time + blinkInterval;
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private void OnDisable()
    {
        SetAlpha(1f);
        isDimmed = false;
    }
}
