using UnityEngine;
using UnityEngine.UI;

// Barra della vita generica: basta metterlo sullo stesso GameObject di uno Slider UI,
// nessun collegamento manuale al player richiesto (usa PlayerHealth.Instance).
[RequireComponent(typeof(Slider))]
public sealed class PlayerHealthBarUI : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0f;
    }

    private void Update()
    {
        PlayerHealth health = PlayerHealth.Instance;
        if (health == null)
        {
            return;
        }

        slider.maxValue = health.MaxHealth;
        slider.value = health.CurrentHealth;
    }
}
