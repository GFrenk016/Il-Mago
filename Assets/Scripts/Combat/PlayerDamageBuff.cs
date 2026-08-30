using UnityEngine;

// Potenziamento temporaneo del danno del player (es. il pickup "danno x2").
// È un singleton che si crea da solo la prima volta che serve: non devi
// aggiungerlo a nessun GameObject in scena, ci pensa il pickup. Muore insieme
// alla scena, quindi il buff non si trascina nel livello successivo.
//
// I proiettili chiedono il moltiplicatore con PlayerDamageBuff.Scale(danno),
// l'HUD legge IsActive / Multiplier / RemainingTime per mostrare il timer.
public sealed class PlayerDamageBuff : MonoBehaviour
{
    private static PlayerDamageBuff instance;

    private float multiplier = 1f;
    private float remainingTime;
    private float totalDuration;

    public static bool IsActive => instance != null && instance.remainingTime > 0f;

    // 1 = nessun buff attivo.
    public static float Multiplier => IsActive ? instance.multiplier : 1f;

    // Secondi rimanenti del buff (0 se non attivo).
    public static float RemainingTime => IsActive ? instance.remainingTime : 0f;

    // Durata totale con cui è stato attivato l'ultimo buff (utile per barre di progresso).
    public static float TotalDuration => IsActive ? instance.totalDuration : 0f;

    // Applica il moltiplicatore corrente a un valore di danno.
    public static float Scale(float damage)
    {
        return damage * Multiplier;
    }

    // Attiva (o rinnova) il buff. Se ne arriva uno più forte mentre un altro è
    // attivo vince il più forte; a parità di moltiplicatore il tempo si somma.
    public static void Apply(float damageMultiplier, float duration)
    {
        if (damageMultiplier <= 0f || duration <= 0f)
        {
            return;
        }

        PlayerDamageBuff buff = GetOrCreate();

        if (!IsActive || damageMultiplier > buff.multiplier)
        {
            buff.multiplier = damageMultiplier;
            buff.remainingTime = duration;
        }
        else if (Mathf.Approximately(damageMultiplier, buff.multiplier))
        {
            buff.remainingTime += duration;
        }
        else
        {
            // Buff più debole di quello attivo: non lo indebolisco.
            return;
        }

        buff.totalDuration = Mathf.Max(buff.totalDuration, buff.remainingTime);
    }

    public static void Clear()
    {
        if (instance != null)
        {
            instance.remainingTime = 0f;
            instance.multiplier = 1f;
            instance.totalDuration = 0f;
        }
    }

    private static PlayerDamageBuff GetOrCreate()
    {
        if (instance == null)
        {
            instance = FindFirstObjectByType<PlayerDamageBuff>();
        }

        if (instance == null)
        {
            GameObject holder = new("PlayerDamageBuff");
            instance = holder.AddComponent<PlayerDamageBuff>();
        }

        return instance;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void Update()
    {
        if (remainingTime <= 0f)
        {
            return;
        }

        remainingTime = Mathf.Max(0f, remainingTime - Time.deltaTime);

        if (remainingTime <= 0f)
        {
            multiplier = 1f;
            totalDuration = 0f;
        }
    }
}
