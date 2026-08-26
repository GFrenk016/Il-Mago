using System.Collections;
using TMPro;
using UnityEngine;

// Popup stile "A secret is revealed!" di Doom II: appare con una dissolvenza,
// resta visibile qualche secondo e sparisce da sola. Usa un Instance statico
// (come GameOverUI) così SecretArea lo trova da sola senza collegamenti manuali.
//
// Setup: nel Canvas dell'HUD (lo stesso di GameOverUI/PlayerHealthBarUI) crea un
// pannello con un CanvasGroup e un testo TMP dentro, mettici sopra questo
// script e trascina il testo nel campo "Label". Lascia alpha del CanvasGroup a
// 0 in partenza: ci pensa lo script ad alzarlo quando serve.
[RequireComponent(typeof(CanvasGroup))]
public sealed class SecretPopupUI : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField, Min(0f)] private float visibleDuration = 2.5f;
    [SerializeField, Min(0.01f)] private float fadeDuration = 0.4f;

    private CanvasGroup canvasGroup;
    private Coroutine activeRoutine;

    public static SecretPopupUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void Show(string message)
    {
        if (label != null)
        {
            label.text = message;
        }

        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
        }

        activeRoutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        yield return Fade(canvasGroup.alpha, 1f);
        yield return new WaitForSeconds(visibleDuration);
        yield return Fade(1f, 0f);
        activeRoutine = null;
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}
