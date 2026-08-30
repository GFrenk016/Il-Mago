using UnityEngine;

// Pozione di cura: ripristina una quantità fissa di HP al player quando viene
// raccolta e lo annuncia con il popup dei pickup.
public sealed class HealthPickup : Pickup
{
    [Header("Cura")]
    [SerializeField, Min(0f)] private float healAmount = 1f;

    protected override string DefaultMessage => $"Pozione di cura! +{healAmount:0.#} HP";

    protected override void ApplyEffect(PlayerHealth player)
    {
        player.Heal(healAmount);
    }
}
