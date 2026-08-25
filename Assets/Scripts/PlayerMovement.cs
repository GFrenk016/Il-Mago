using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public sealed class PlayerMovement : MonoBehaviour
{
    [SerializeField, Min(0f)] private float moveSpeed = 4f;

    private Rigidbody2D body;
    private Vector2 moveInput;

    public static PlayerMovement Instance { get; private set; }

    public Vector2 MoveInput => moveInput;
    public Vector2 FacingDirection { get; private set; } = Vector2.down;

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

    private void FixedUpdate()
    {
        Vector2 nextPosition = body.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        body.MovePosition(nextPosition);
    }

    private void OnDisable()
    {
        moveInput = Vector2.zero;
    }
}
