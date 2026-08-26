using UnityEngine;

// Base per qualunque oggetto raccoglibile: gestisce il trigger e la distruzione,
// le sottoclassi implementano solo l'effetto (es. HealthPickup cura il player).
[RequireComponent(typeof(Collider2D))]
public abstract class Pickup : MonoBehaviour
{
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
        Destroy(gameObject);
    }

    protected abstract void ApplyEffect(PlayerHealth player);
}
