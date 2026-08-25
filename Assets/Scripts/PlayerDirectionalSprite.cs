using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(PlayerMovement))]
public sealed class PlayerDirectionalSprite : MonoBehaviour
{
    [Header("Idle sprites")]
    [SerializeField] private Sprite south;
    [SerializeField] private Sprite southWest;
    [SerializeField] private Sprite west;
    [SerializeField] private Sprite northWest;
    [SerializeField] private Sprite north;
    [SerializeField] private Sprite northEast;
    [SerializeField] private Sprite east;
    [SerializeField] private Sprite southEast;

    private PlayerMovement movement;
    private SpriteRenderer spriteRenderer;
    private Direction currentDirection = Direction.South;

    private enum Direction
    {
        East,
        NorthEast,
        North,
        NorthWest,
        West,
        SouthWest,
        South,
        SouthEast
    }

#if UNITY_EDITOR
    private void Reset()
    {
        south = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(
            "Assets/Sprites/Player/Idle/south.png");
        southWest = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(
            "Assets/Sprites/Player/Idle/south-west.png");
        west = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(
            "Assets/Sprites/Player/Idle/west.png");
        northWest = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(
            "Assets/Sprites/Player/Idle/north-west.png");
        north = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(
            "Assets/Sprites/Player/Idle/north.png");
        northEast = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(
            "Assets/Sprites/Player/Idle/north-east.png");
        east = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(
            "Assets/Sprites/Player/Idle/east.png");
        southEast = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(
            "Assets/Sprites/Player/Idle/south-east.png");
    }
#endif

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplySprite(currentDirection);
    }

    private void Update()
    {
        Direction nextDirection = GetDirection(movement.FacingDirection);
        if (nextDirection == currentDirection)
        {
            return;
        }

        currentDirection = nextDirection;
        ApplySprite(currentDirection);
    }

    private static Direction GetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0f)
        {
            angle += 360f;
        }

        int sector = Mathf.RoundToInt(angle / 45f) % 8;
        return (Direction)sector;
    }

    private void ApplySprite(Direction direction)
    {
        Sprite nextSprite = direction switch
        {
            Direction.East => east,
            Direction.NorthEast => northEast,
            Direction.North => north,
            Direction.NorthWest => northWest,
            Direction.West => west,
            Direction.SouthWest => southWest,
            Direction.South => south,
            Direction.SouthEast => southEast,
            _ => south
        };

        if (nextSprite != null)
        {
            spriteRenderer.sprite = nextSprite;
        }
    }
}
