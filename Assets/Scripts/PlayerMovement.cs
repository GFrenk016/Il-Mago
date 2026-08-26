using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public sealed class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField, Min(0f)] private float moveSpeed = 4f;

    [Header("Corsa (tieni premuto Shift)")]
    [SerializeField, Min(1f)] private float sprintSpeedMultiplier = 1.6f;
    [SerializeField, Min(0.01f)] private float maxStamina = 5f;
    [SerializeField, Min(0f)] private float staminaDrainPerSecond = 1.5f;
    [SerializeField, Min(0f)] private float staminaRegenPerSecond = 1f;

    private Rigidbody2D body;
    private Vector2 moveInput;

    public static PlayerMovement Instance { get; private set; }

    public Vector2 MoveInput => moveInput;
    public Vector2 FacingDirection { get; private set; } = Vector2.down;

    public float MaxStamina => maxStamina;
    public float CurrentStamina { get; private set; }
    public bool IsSprinting { get; private set; }

    private void Reset()
    {
        Rigidbody2D attachedBody = GetComponent<Rigidbody2D>();
        attachedBody.gravityScale = 0f;
        attachedBody.freezeRotation = true;
        attachedBody.interpolation = RigidbodyInterpolation2D.Interpolate;

        CapsuleCollider2D attachedCollider = GetComponent<CapsuleCollider2D>();
        attachedCollider.direction = CapsuleDirection2D.Vertical;
        attachedCollider.size = new Vector2(0.45f, 0.35f);
        attachedCollider.offset = new Vector2(0f, -0.28f);
    }

    private void Awake()
    {
        Instance = this;
        body = GetComponent<Rigidbody2D>();
        CurrentStamina = maxStamina;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = Vector2.ClampMagnitude(value.Get<Vector2>(), 1f);

        if (moveInput.sqrMagnitude > 0.001f)
        {
            FacingDirection = moveInput.normalized;
        }
    }

    public void Face(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.001f)
        {
            FacingDirection = direction.normalized;
        }
    }

    private void Update()
    {
        bool sprintHeld = Keyboard.current != null
            && (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed);

        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        IsSprinting = sprintHeld && isMoving && CurrentStamina > 0f;

        if (IsSprinting)
        {
            CurrentStamina = Mathf.Max(0f, CurrentStamina - staminaDrainPerSecond * Time.deltaTime);
        }
        else
        {
            CurrentStamina = Mathf.Min(maxStamina, CurrentStamina + staminaRegenPerSecond * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        float speed = moveSpeed * (IsSprinting ? sprintSpeedMultiplier : 1f);
        Vector2 nextPosition = body.position + moveInput * speed * Time.fixedDeltaTime;
        body.MovePosition(nextPosition);
    }

    private void OnDisable()
    {
        moveInput = Vector2.zero;
        IsSprinting = false;
    }
}
