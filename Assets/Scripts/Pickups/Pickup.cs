using UnityEngine;

// Base per qualunque oggetto raccoglibile: gestisce il trigger, il messaggio a
// schermo e la distruzione; le sottoclassi implementano solo l'effetto (es.
// HealthPickup cura il player).
//
// Quando il pickup viene raccolto mostra una scritta stile "area segreta"
// (stesso popup di SecretArea, SecretPopupUI) che dice cosa hai preso e cosa
// ha fatto. Ogni pickup ha il suo testo predefinito; se vuoi cambiarlo per un
// singolo oggetto in scena riempi il campo "Custom Message".
[RequireComponent(typeof(Collider2D))]
public abstract class Pickup : MonoBehaviour
{
    [Header("Messaggio a schermo")]
    [Tooltip("Lascia vuoto per usare il testo predefinito di questo tipo di pickup.")]
    [SerializeField] private string customMessage = "";
    [Tooltip("Toglila se vuoi che questo pickup venga raccolto in silenzio.")]
    [SerializeField] private bool showMessage = true;

    // Testo mostrato quando non c'è un messaggio personalizzato.
    protected abstract string DefaultMessage { get; }

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player == null || player != PlayerHealth.Instance)
        {
            return;
        }

        ApplyEffect(player);
        ShowPickupMessage();
        Destroy(gameObject);
    }

    private void ShowPickupMessage()
    {
        if (!showMessage)
        {
            return;
        }

        string message = string.IsNullOrWhiteSpace(customMessage) ? DefaultMessage : customMessage;
        if (!string.IsNullOrWhiteSpace(message))
        {
            SecretPopupUI.Instance?.Show(message);
        }
    }

    protected abstract void ApplyEffect(PlayerHealth player);
}
