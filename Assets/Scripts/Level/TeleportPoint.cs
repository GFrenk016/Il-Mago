using UnityEngine;
using UnityEngine.Events;

// Teletrasporto che collega due stanze della mappa. Stessa meccanica del
// LevelEndPoint: resta chiuso finché il MobSpawner della stanza non segnala che
// tutti i nemici sono morti (se non gliene assegni uno è aperto da subito).
// Quando è aperto e il player ci sale sopra, lo sposta di colpo sul teleport
// gemello (o sul Transform di destinazione) e riallinea la camera.
//
// Setup tipico (stanza A -> stanza B):
//  1. Crea un GameObject "Teleport_A" nella stanza A con un Collider2D "Is
//     Trigger" e questo script; trascina in "Room Spawner" il MobSpawner della
//     stanza A e in "Visual" lo sprite/effetto del portale (resta nascosto
//     finché la stanza non è ripulita).
//  2. Crea allo stesso modo "Teleport_B" nella stanza B, lasciando "Room
//     Spawner" vuoto se vuoi che il ritorno sia sempre aperto.
//  3. Trascina Teleport_B nel campo "Linked Teleport" di Teleport_A e viceversa.
//     I due si sbloccano a vicenda quando serve e non si rimbalzano il player.
[RequireComponent(typeof(Collider2D))]
public sealed class TeleportPoint : MonoBehaviour
{
    [Header("Destinazione")]
    [SerializeField] private TeleportPoint linkedTeleport;
    [Tooltip("Usato solo se non hai assegnato un Linked Teleport.")]
    [SerializeField] private Transform destinationOverride;
    [Tooltip("Punto esatto in cui compare il player quando arriva qui (di solito un figlio del portale). Se vuoto usa il portale stesso.")]
    [SerializeField] private Transform arrivalPoint;

    [Header("Sblocco")]
    [Tooltip("Se assegnato, il teleport si apre solo quando tutti i nemici di questo spawner sono morti.")]
    [SerializeField] private MobSpawner roomSpawner;
    [Tooltip("Sprite/effetto del portale: nascosto finché il teleport è chiuso.")]
    [SerializeField] private GameObject visual;

    [Header("Messaggi a schermo (vuoto = nessun messaggio)")]
    [SerializeField] private string unlockMessage = "Il teletrasporto si e' aperto!";
    [SerializeField] private string teleportMessage = "";

    [Header("Varie")]
    [Tooltip("Distanza oltre la quale il portale di arrivo torna utilizzabile, se il player non esce mai dal suo trigger.")]
    [SerializeField, Min(0.1f)] private float reactivateDistance = 1.5f;
    [SerializeField] private UnityEvent onTeleport;

    private Collider2D teleportCollider;
    private bool unlocked;
    private bool ignoreUntilExit;

    public bool IsUnlocked => unlocked;

    // Dove finisce il player quando arriva su questo teleport.
    public Vector3 ArrivalPosition => arrivalPoint != null ? arrivalPoint.position : transform.position;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Awake()
    {
        teleportCollider = GetComponent<Collider2D>();
        SetUnlocked(roomSpawner == null || roomSpawner.AllDefeated, announce: false);
    }

    private void Update()
    {
        if (!unlocked && roomSpawner != null && roomSpawner.AllDefeated)
        {
            SetUnlocked(true, announce: true);
        }

        // Sicurezza: se il player si è allontanato senza far scattare
        // OnTriggerExit2D, riabilita comunque il portale.
        if (ignoreUntilExit)
        {
            Transform player = PlayerHealth.Instance != null ? PlayerHealth.Instance.transform : null;
            if (player == null ||
                Vector2.Distance(player.position, transform.position) > reactivateDistance)
            {
                ignoreUntilExit = false;
            }
        }
    }

    private void SetUnlocked(bool value, bool announce)
    {
        unlocked = value;

        if (teleportCollider != null)
        {
            teleportCollider.enabled = value;
        }

        if (visual != null)
        {
            visual.SetActive(value);
        }

        if (value && announce && !string.IsNullOrWhiteSpace(unlockMessage))
        {
            SecretPopupUI.Instance?.Show(unlockMessage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!unlocked || ignoreUntilExit)
        {
            return;
        }

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player == null || player != PlayerHealth.Instance)
        {
            return;
        }

        Teleport(player.transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null && player == PlayerHealth.Instance)
        {
            ignoreUntilExit = false;
        }
    }

    private void Teleport(Transform player)
    {
        Vector3 destination;

        if (linkedTeleport != null)
        {
            destination = linkedTeleport.ArrivalPosition;
        }
        else if (destinationOverride != null)
        {
            destination = destinationOverride.position;
        }
        else
        {
            Debug.LogWarning($"{name}: nessuna destinazione assegnata al teleport.", this);
            return;
        }

        destination.z = player.position.z;
        MovePlayer(player, destination);

        // Il portale di arrivo non deve rispedire subito indietro il player.
        if (linkedTeleport != null)
        {
            linkedTeleport.BlockUntilPlayerLeaves();
        }

        SnapCamera();

        if (!string.IsNullOrWhiteSpace(teleportMessage))
        {
            SecretPopupUI.Instance?.Show(teleportMessage);
        }

        onTeleport?.Invoke();
    }

    private static void MovePlayer(Transform player, Vector3 destination)
    {
        if (player.TryGetComponent(out Rigidbody2D body))
        {
            // Con l'interpolazione attiva lo spostamento istantaneo lascerebbe
            // una "scia": la spengo per questo frame e la rimetto subito dopo.
            RigidbodyInterpolation2D previousInterpolation = body.interpolation;
            body.interpolation = RigidbodyInterpolation2D.None;
            body.linearVelocity = Vector2.zero;
            body.position = destination;
            player.position = destination;
            body.interpolation = previousInterpolation;
        }
        else
        {
            player.position = destination;
        }
    }

    private static void SnapCamera()
    {
        CameraFollow2D follow = Camera.main != null ? Camera.main.GetComponent<CameraFollow2D>() : null;
        if (follow == null)
        {
            follow = FindFirstObjectByType<CameraFollow2D>();
        }

        if (follow != null)
        {
            follow.SnapToTarget();
        }
    }

    // Chiamata dal teleport gemello: evita il rimbalzo avanti/indietro appena
    // il player atterra qui sopra.
    public void BlockUntilPlayerLeaves()
    {
        ignoreUntilExit = true;
    }
}
