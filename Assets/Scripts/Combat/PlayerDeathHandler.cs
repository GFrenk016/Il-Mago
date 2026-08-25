using UnityEngine;

// Cosa succede quando il player muore: blocca movimento e attacco, poi avvisa
// la schermata di Game Over (se presente in scena) tramite GameOverUI.Instance.
// Anche questo senza collegamenti manuali nell'Inspector.
[RequireComponent(typeof(PlayerHealth))]
public sealed class PlayerDeathHandler : MonoBehaviour
{
    private PlayerHealth health;
    private PlayerMovement movement;
    private PlayerAttack attack;
    private bool hasHandledDeath;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if (hasHandledDeath || health.IsAlive)
        {
            return;
        }

        hasHandledDeath = true;

        if (movement != null)
        {
            movement.enabled = false;
        }

        if (attack != null)
        {
            attack.enabled = false;
        }

        if (GameOverUI.Instance != null)
        {
            GameOverUI.Instance.Show();
        }
    }

    private void OnDisable()
    {
        hasHandledDeath = false;
    }
}
