using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public sealed class ArcaneTrailFade : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color startColor;
    private float lifetime;
    private float elapsed;

    public void Initialize(Color color, float duration)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = color;
        lifetime = Mathf.Max(0.01f, duration);
        spriteRenderer.color = startColor;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsed / lifetime);

        Color color = startColor;
        color.a *= 1f - progress;
        spriteRenderer.color = color;

        if (progress >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
