using UnityEngine;
using UnityEngine.UI;

// Barra della stamina: stesso pattern di PlayerHealthBarUI, nessun collegamento
// manuale richiesto (usa PlayerMovement.Instance). Mettila sotto la barra
// della vita nello stesso Canvas HUD.
[RequireComponent(typeof(Slider))]
public sealed class PlayerStaminaBarUI : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0f;
    }

    private void Update()
    {
        PlayerMovement movement = PlayerMovement.Instance;
        if (movement == null)
        {
            return;
        }

        slider.maxValue = movement.MaxStamina;
        slider.value = movement.CurrentStamina;
    }
}
