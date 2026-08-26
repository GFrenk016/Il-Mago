using UnityEngine;

// Area segreta stile Doom: la prima volta che il player ci entra mostra il
// popup ("Hai trovato un'area segreta!") tramite SecretPopupUI, poi non fa più
// nulla. Mettila su un GameObject con un Collider2D "Is Trigger" nascosto in un
// angolo della mappa (dietro un muro, in fondo a un corridoio cieco, ecc.).
[RequireComponent(typeof(Collider2D))]
public sealed class SecretArea : MonoBehaviour
{
    [SerializeField] private string message = "Hai trovato un'area segreta!";

    private bool found;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (found)
        {
            return;
        }

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player == null || player != PlayerHealth.Instance)
        {
            return;
        }

        found = true;
        SecretPopupUI.Instance?.Show(message);
    }
}
