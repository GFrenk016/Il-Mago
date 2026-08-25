using UnityEngine;

// Stesso identico principio di PlayerDirectionalSprite, ma pilotato da EnemyMovement.
// Generico: ogni archetipo assegna i propri 8 sprite Idle nell'Inspector del suo Prefab.
[RequireComponent(typeof(SpriteRenderer), typeof(EnemyMovement))]
public sealed class EnemyDirectionalSprite : MonoBehaviour
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

    private EnemyMovement movement;
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

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
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
