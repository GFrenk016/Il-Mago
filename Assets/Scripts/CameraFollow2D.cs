using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Collider2D levelBounds;
    [SerializeField, Min(0f)] private float smoothTime = 0.12f;

    private Camera attachedCamera;
    private Vector3 velocity;

    private void Awake()
    {
        attachedCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        MoveToTarget(immediate: true);
    }

    // Riallinea subito la camera sul target, senza lo smorzamento: serve
    // quando il player viene spostato di colpo (es. TeleportPoint).
    public void SnapToTarget()
    {
        velocity = Vector3.zero;
        MoveToTarget(immediate: true);
    }

    private void LateUpdate()
    {
        MoveToTarget(immediate: false);
    }

    private void MoveToTarget(bool immediate)
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition = new(target.position.x, target.position.y, transform.position.z);
        desiredPosition = ClampInsideBounds(desiredPosition);

        transform.position = immediate || smoothTime <= 0f
            ? desiredPosition
            : Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }

    private Vector3 ClampInsideBounds(Vector3 position)
    {
        if (levelBounds == null || !attachedCamera.orthographic)
        {
            return position;
        }

        Bounds bounds = levelBounds.bounds;
        float verticalExtent = attachedCamera.orthographicSize;
        float horizontalExtent = verticalExtent * attachedCamera.aspect;

        position.x = ClampAxis(position.x, bounds.min.x + horizontalExtent, bounds.max.x - horizontalExtent,
            bounds.center.x);
        position.y = ClampAxis(position.y, bounds.min.y + verticalExtent, bounds.max.y - verticalExtent,
            bounds.center.y);

        return position;
    }

    private static float ClampAxis(float value, float minimum, float maximum, float fallback)
    {
        return minimum <= maximum ? Mathf.Clamp(value, minimum, maximum) : fallback;
    }
}
