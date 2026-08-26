using UnityEngine;

// Trigger che segna l'ingresso in una stanza: la prima volta che il player ci
// passa dentro, attiva il mob spawner e (se assegnato) il pickup spawner della
// stanza. Nessun blocco fisico: la camera continua a scorrere libera come ora.
//
// Setup: crea un GameObject vuoto sulla porta/soglia della stanza, aggiungi un
// Collider2D che copra il passaggio e spunta "Is Trigger" (lo fa già in automatico
// se lo aggiungi dopo questo script), poi trascina qui il MobSpawner (ed
// eventualmente il PickupSpawner) di quella stanza.
[RequireComponent(typeof(Collider2D))]
public sealed class RoomTrigger : MonoBehaviour
{
    [SerializeField] private MobSpawner mobSpawner;
    [SerializeField] private PickupSpawner pickupSpawner;

    private bool activated;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated)
        {
            return;
        }

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player == null || player != PlayerHealth.Instance)
        {
            return;
        }

        activated = true;

        if (mobSpawner != null)
        {
            mobSpawner.SpawnWave();
        }

        if (pickupSpawner != null)
        {
            pickupSpawner.SpawnPickups();
        }
    }
}
