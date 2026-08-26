using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Spawner di nemici "random": per ogni punto di spawn assegnato sceglie ed
// istanzia un nemico a caso dalla lista di prefab. Non si attiva da solo — va
// chiamato da un RoomTrigger (o da qualunque altro script) con SpawnWave().
//
// Setup: crea un GameObject vuoto nella stanza con questo script, trascina nei
// due array i prefab dei nemici che possono comparire e i punti (GameObject
// vuoti posizionati dove vuoi che spawnino) dove devono comparire. Un punto = un
// nemico scelto a caso tra quelli in lista.
public sealed class MobSpawner : MonoBehaviour
{
    [Header("Nemici possibili")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Punti di spawn")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Eventi")]
    [SerializeField] private UnityEvent onAllEnemiesDefeated;

    private readonly List<GameObject> spawnedEnemies = new();
    private bool hasSpawned;
    private bool clearedEventFired;

    public bool HasSpawned => hasSpawned;
    public int AliveCount => spawnedEnemies.Count;

    // True solo dopo che SpawnWave() è stata chiamata e tutti i nemici spawnati
    // sono morti (o se non c'era nessun punto di spawn da riempire).
    public bool AllDefeated => hasSpawned && spawnedEnemies.Count == 0;

    // Spawna un nemico casuale per ogni punto di spawn assegnato. Chiamate
    // successive non fanno nulla: una stanza spawna una volta sola.
    public void SpawnWave()
    {
        if (hasSpawned || enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            return;
        }

        hasSpawned = true;

        foreach (Transform point in spawnPoints)
        {
            if (point == null)
            {
                continue;
            }

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            if (prefab == null)
            {
                continue;
            }

            GameObject enemy = Instantiate(prefab, point.position, point.rotation);
            spawnedEnemies.Add(enemy);
        }
    }

    private void Update()
    {
        if (!hasSpawned || clearedEventFired)
        {
            return;
        }

        spawnedEnemies.RemoveAll(enemy => enemy == null);

        if (spawnedEnemies.Count == 0)
        {
            clearedEventFired = true;
            onAllEnemiesDefeated?.Invoke();
        }
    }
}
