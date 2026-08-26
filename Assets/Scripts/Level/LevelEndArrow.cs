using UnityEngine;

// Freccia che fluttua sopra la testa del player e punta verso il punto di
// fine livello, ma solo dopo che tutti i nemici della stanza finale sono
// morti (o sempre, se non assegni nessun Final Room Spawner). Ruota ogni
// frame per indicare la direzione verso Target.
//
// Setup: crea un GameObject in scena (non serve metterlo dentro il Player,
// segue da solo la sua posizione) con uno SpriteRenderer (una freccia),
// aggiungi questo script, trascina in Target il Transform del LevelEndPoint
// e, se vuoi che appaia solo a stanza ripulita, anche il MobSpawner della
// stanza finale in Final Room Spawner.
[RequireComponent(typeof(SpriteRenderer))]
public sealed class LevelEndArrow : MonoBehaviour
{
    [Header("Riferimenti")]
    [SerializeField] private Transform target;
    [SerializeField] private MobSpawner finalRoomSpawner;

    [Header("Aspetto")]
    [SerializeField] private float heightAbovePlayer = 1.2f;
    [SerializeField] private float rotationOffsetDegrees = -90f; // -90 se lo sprite di base punta verso l'alto

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetVisible(false);
    }

    private void Update()
    {
        Transform player = PlayerHealth.Instance != null ? PlayerHealth.Instance.transform : null;

        if (player == null || target == null)
        {
            SetVisible(false);
            return;
        }

        bool shouldShow = finalRoomSpawner == null || finalRoomSpawner.AllDefeated;
        SetVisible(shouldShow);

        if (!shouldShow)
        {
            return;
        }

        transform.position = player.position + Vector3.up * heightAbovePlayer;

        Vector2 direction = target.position - transform.position;
        if (direction.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffsetDegrees);
        }
    }

    private void SetVisible(bool visible)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = visible;
        }
    }
}
