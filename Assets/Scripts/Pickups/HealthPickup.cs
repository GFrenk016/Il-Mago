using UnityEngine;

// Pozione di cura: ripristina una quantità fissa di HP al player quando viene
// raccolta.
public sealed class HealthPickup : Pickup
{
    [SerializeField, Min(0f)] private float healAmount = 1f;

    protected override void ApplyEffect(PlayerHealth player)
    {
        player.Heal(healAmount);
    }
}
