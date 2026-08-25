using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(PlayerMovement))]
[MovedFrom(true, sourceNamespace: null, sourceAssembly: null, sourceClassName: "PlayerFireballAttack")]
public sealed class PlayerAttack : MonoBehaviour
{
    private enum SelectedSpell
    {
        Fireball,
        IceBolt,
        ArcaneBlast
    }

    [Header("Spell selection")]
    [SerializeField] private SelectedSpell selectedSpell = SelectedSpell.Fireball;

    [Header("Fireball")]
    [SerializeField] private FireballProjectile fireballPrefab;
    [FormerlySerializedAs("cooldown")]
    [SerializeField, Min(0f)] private float fireballCooldown = 0.4f;

    [Header("Ice Bolt")]
    [SerializeField] private IceBoltProjectile iceBoltPrefab;
    [SerializeField, Min(0f)] private float iceBoltCooldown = 0.8f;

    [Header("Arcane Blast")]
    [SerializeField] private ArcaneBlastProjectile arcaneBlastPrefab;
    [SerializeField, Min(0f)] private float arcaneBlastCooldown = 6f;

    [Header("Cast origin")]
    [SerializeField] private Transform firePoint;
    [SerializeField, Min(0f)] private float spawnDistance = 0.55f;

    [Header("Mouse aim")]
    [SerializeField] private Camera aimCamera;

    private PlayerMovement movement;
    private float nextFireballTime;
    private float nextIceBoltTime;
    private float nextArcaneBlastTime;

    public string SelectedSpellName => selectedSpell.ToString();

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (fireballPrefab == null)
        {
            GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Prefabs/Fireball.prefab");
            fireballPrefab = prefab != null ? prefab.GetComponent<FireballProjectile>() : null;
        }

        if (iceBoltPrefab == null)
        {
            GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Prefabs/IceBolt.prefab");
            iceBoltPrefab = prefab != null ? prefab.GetComponent<IceBoltProjectile>() : null;
        }

        if (arcaneBlastPrefab == null)
        {
            GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/Prefabs/ArcaneBlast.prefab");
            arcaneBlastPrefab = prefab != null ? prefab.GetComponent<ArcaneBlastProjectile>() : null;
        }

        if (aimCamera == null)
        {
            aimCamera = Camera.main;
        }
    }
#endif

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();

        if (aimCamera == null)
        {
            aimCamera = Camera.main;
        }

    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            selectedSpell = SelectedSpell.Fireball;
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame || Keyboard.current.numpad2Key.wasPressedThisFrame)
        {
            selectedSpell = SelectedSpell.IceBolt;
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame || Keyboard.current.numpad3Key.wasPressedThisFrame)
        {
            selectedSpell = SelectedSpell.ArcaneBlast;
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            TryCastSelectedSpell();
        }
    }

    private void TryCastSelectedSpell()
    {
        if (aimCamera == null || Mouse.current == null)
        {
            return;
        }

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = aimCamera.ScreenToWorldPoint(
            new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, -aimCamera.transform.position.z));

        Vector2 origin = firePoint != null ? firePoint.position : transform.position;
        Vector2 aimDirection = (Vector2)mouseWorldPosition - origin;
        if (aimDirection.sqrMagnitude <= 0.001f)
        {
            return;
        }

        aimDirection.Normalize();
        Vector2 spawnPosition = firePoint != null
            ? (Vector2)firePoint.position
            : (Vector2)transform.position + aimDirection * spawnDistance;

        bool spellWasCast = selectedSpell switch
        {
            SelectedSpell.Fireball => TryCastFireball(spawnPosition, aimDirection),
            SelectedSpell.IceBolt => TryCastIceBolt(spawnPosition, aimDirection),
            SelectedSpell.ArcaneBlast => TryCastArcaneBlast(spawnPosition, aimDirection),
            _ => false
        };

        if (spellWasCast)
        {
            movement.Face(aimDirection);
        }
    }

    private bool TryCastFireball(Vector2 spawnPosition, Vector2 aimDirection)
    {
        if (fireballPrefab == null || Time.time < nextFireballTime)
        {
            return false;
        }

        FireballProjectile projectile = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
        projectile.Initialize(aimDirection, transform);
        nextFireballTime = Time.time + fireballCooldown;
        return true;
    }

    private bool TryCastIceBolt(Vector2 spawnPosition, Vector2 aimDirection)
    {
        if (iceBoltPrefab == null || Time.time < nextIceBoltTime)
        {
            return false;
        }

        IceBoltProjectile projectile = Instantiate(iceBoltPrefab, spawnPosition, Quaternion.identity);
        projectile.Initialize(aimDirection, transform);
        nextIceBoltTime = Time.time + iceBoltCooldown;
        return true;
    }

    private bool TryCastArcaneBlast(Vector2 spawnPosition, Vector2 aimDirection)
    {
        if (arcaneBlastPrefab == null || Time.time < nextArcaneBlastTime)
        {
            return false;
        }

        ArcaneBlastProjectile projectile = Instantiate(arcaneBlastPrefab, spawnPosition, Quaternion.identity);
        projectile.Initialize(aimDirection, transform);
        nextArcaneBlastTime = Time.time + arcaneBlastCooldown;
        return true;
    }
}
