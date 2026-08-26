using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Punto di fine livello: resta disattivato finché il MobSpawner della stanza
// finale non segnala che tutti i nemici sono morti (o se non gliene assegni
// uno, è attivo da subito). Quando è attivo e il player ci passa sopra,
// completa il livello.
//
// Setup: mettilo su un GameObject con un Collider2D "Is Trigger" nel punto di
// uscita della stanza finale, trascina qui il MobSpawner di quella stanza e,
// se vuoi un effetto visivo (es. un portale che appare), assegna "Visual" —
// resterà nascosto finché la stanza non è ripulita.
[RequireComponent(typeof(Collider2D))]
public sealed class LevelEndPoint : MonoBehaviour
{
    [SerializeField] private MobSpawner finalRoomSpawner;
    [SerializeField] private GameObject visual;

    [Header("Cosa succede al completamento")]
    [SerializeField] private bool loadMainMenuOnComplete = true;
    [SerializeField] private UnityEvent onLevelComplete;

    private Collider2D endCollider;
    private bool unlocked;
    private bool completed;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Awake()
    {
        endCollider = GetComponent<Collider2D>();
        SetUnlocked(finalRoomSpawner == null || finalRoomSpawner.AllDefeated);
    }

    private void Update()
    {
        if (!unlocked && finalRoomSpawner != null && finalRoomSpawner.AllDefeated)
        {
            SetUnlocked(true);
        }
    }

    private void SetUnlocked(bool value)
    {
        unlocked = value;
        endCollider.enabled = value;

        if (visual != null)
        {
            visual.SetActive(value);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (completed || !unlocked)
        {
            return;
        }

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player == null || player != PlayerHealth.Instance)
        {
            return;
        }

        completed = true;
        onLevelComplete?.Invoke();

        // Placeholder finché non esiste la schermata risultati della Milestone 9:
        // per ora torna semplicemente al Main Menu. Disattiva la checkbox se vuoi
        // gestire il completamento solo con l'evento onLevelComplete qui sopra.
        if (loadMainMenuOnComplete)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
