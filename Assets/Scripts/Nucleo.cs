using UnityEngine;
using System.Collections;

public enum Direction
{
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft
}

public enum NucleoType
{
    Ademine,
    Cytosine,
    Thymine
}

public abstract class Nucleo : MonoBehaviour {

    protected const float kMaxWalkTime = 2f;
    protected const float kMaxStopTime = 2f;

    [SerializeField]
    public NucleoType _nucleoType;

    [SerializeField]
    protected float baseWalkSpeed;

    protected Animator animator;

    protected float stopTime;
    protected float walkTime;
    protected bool walking;

    protected Direction currentDirection;
    protected Vector2 walkVector;

    public NucleoType nucleoType { get { return nucleoType; } }

    protected abstract void ProcessMovement(float interval);

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        ProcessMovement(Time.deltaTime);
    }

    public static Vector2 DirectionToVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector2.up;
            case Direction.UpRight:
                return new Vector2(0.5f, 0.5f);
            case Direction.Right:
                return Vector2.right;
            case Direction.DownRight:
                return new Vector2(0.5f, -0.5f);
            case Direction.Down:
                return Vector2.down;
            case Direction.DownLeft:
                return new Vector2(-0.5f, -0.5f);
            case Direction.Left:
                return Vector2.left;
            case Direction.UpLeft:
                return new Vector2(-0.5f, 0.5f);
            default:
                return Vector2.zero;
        }
    }
}
