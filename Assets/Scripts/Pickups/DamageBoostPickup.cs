using UnityEngine;

// Pergamena del potere: raddoppia il danno di tutte le magie per un tempo
// limitato (di default x2 per 5 minuti). Il timer compare nell'HUD grazie a
// DamageBuffTimerUI e il pickup annuncia l'effetto con il popup stile area
// segreta.
//
// Setup: stesso identico del HealthPickup — un GameObject con SpriteRenderer,
// un Collider2D "Is Trigger" e questo script; salvalo come prefab e mettilo
// nella lista del PickupSpawner (o piazzalo a mano nella mappa).
public sealed class DamageBoostPickup : Pickup
{
    [Header("Potenziamento danno")]
    [SerializeField, Min(1f)] private float damageMultiplier = 2f;
    [Tooltip("Durata del buff in secondi (300 = 5 minuti).")]
    [SerializeField, Min(1f)] private float durationSeconds = 300f;

    protected override string DefaultMessage =>
        $"Potere arcano! Danno x{damageMultiplier:0.#} per {DescribeDuration(durationSeconds)}";

    protected override void ApplyEffect(PlayerHealth player)
    {
        PlayerDamageBuff.Apply(damageMultiplier, durationSeconds);
    }

    private static string DescribeDuration(float seconds)
    {
        int totalSeconds = Mathf.RoundToInt(seconds);
        int minutes = totalSeconds / 60;
        int remainder = totalSeconds % 60;

        if (minutes > 0 && remainder == 0)
        {
            return minutes == 1 ? "1 minuto" : $"{minutes} minuti";
        }

        if (minutes > 0)
        {
            return $"{minutes}m {remainder}s";
        }

        return $"{remainder} secondi";
    }
}
