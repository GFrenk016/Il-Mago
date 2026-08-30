using TMPro;
using UnityEngine;

// Timer del potenziamento danno nell'HUD: mostra "DANNO x2  04:59" mentre il
// buff è attivo e sparisce da solo quando finisce. Nessun collegamento manuale
// richiesto (legge PlayerDamageBuff, che è statico), stesso pattern delle barre
// di vita e stamina.
//
// Setup: nel Canvas dell'HUD crea un testo TMP (es. in alto a destra, sotto le
// barre), mettici sopra questo script e lascia "Label" vuoto — si prende da solo
// il TMP_Text del suo GameObject.
public sealed class DamageBuffTimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [Tooltip("{0} = moltiplicatore, {1} = tempo rimanente mm:ss")]
    [SerializeField] private string format = "DANNO x{0}  {1}";

    [Header("Colori")]
    [SerializeField] private Color normalColor = new(1f, 0.85f, 0.25f);
    [SerializeField] private Color endingSoonColor = new(1f, 0.35f, 0.25f);
    [Tooltip("Sotto questi secondi il testo diventa del colore 'Ending Soon' e lampeggia.")]
    [SerializeField, Min(0f)] private float endingSoonThreshold = 15f;
    [SerializeField, Min(0f)] private float blinksPerSecond = 2f;

    private void Awake()
    {
        if (label == null)
        {
            label = GetComponent<TMP_Text>();
        }

        SetVisible(false);
    }

    private void Update()
    {
        if (label == null)
        {
            return;
        }

        if (!PlayerDamageBuff.IsActive)
        {
            SetVisible(false);
            return;
        }

        SetVisible(true);

        float remaining = PlayerDamageBuff.RemainingTime;
        label.text = string.Format(
            format,
            PlayerDamageBuff.Multiplier.ToString("0.#"),
            FormatTime(remaining));

        bool endingSoon = remaining <= endingSoonThreshold;
        Color color = endingSoon ? endingSoonColor : normalColor;

        if (endingSoon && blinksPerSecond > 0f)
        {
            float blink = Mathf.PingPong(Time.unscaledTime * blinksPerSecond, 1f);
            color.a *= Mathf.Lerp(0.35f, 1f, blink);
        }

        label.color = color;
    }

    private void SetVisible(bool visible)
    {
        if (label != null && label.enabled != visible)
        {
            label.enabled = visible;
        }
    }

    private static string FormatTime(float seconds)
    {
        int totalSeconds = Mathf.CeilToInt(Mathf.Max(0f, seconds));
        return $"{totalSeconds / 60:00}:{totalSeconds % 60:00}";
    }
}
