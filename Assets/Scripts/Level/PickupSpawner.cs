using UnityEngine;

// Spawner di oggetti raccoglibili (pozioni etc.): per ogni punto di spawn
// assegnato sceglie ed istanzia un pickup a caso dalla lista di prefab. Non si
// attiva da solo — va chiamato da un RoomTrigger (o da qualunque altro script)
// con SpawnPickups().
//
// Setup: come il MobSpawner. "Probabilità per punto" a 1 = spawna sempre un
// pickup in ogni punto; abbassala se vuoi che compaiano solo a volte.
public sealed class PickupSpawner : MonoBehaviour
{
    [Header("Pickup possibili")]
    [SerializeField] private GameObject[] pickupPrefabs;

    [Header("Punti di spawn")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Probabilità per punto")]
    [Range(0f, 1f)]
    [SerializeField] private float spawnChancePerPoint = 1f;

    private bool hasSpawned;

    // Prova a spawnare un pickup casuale per ogni punto di spawn assegnato.
    // Chiamate successive non fanno nulla: una stanza spawna una volta sola.
    public void SpawnPickups()
    {
        if (hasSpawned || pickupPrefabs == null || pickupPrefabs.Length == 0)
        {
            return;
        }

        hasSpawned = true;

        foreach (Transform point in spawnPoints)
        {
            if (point == null || Random.value > spawnChancePerPoint)
            {
                continue;
            }

            GameObject prefab = pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];
            if (prefab != null)
            {
                Instantiate(prefab, point.position, point.rotation);
            }
        }
    }
}
