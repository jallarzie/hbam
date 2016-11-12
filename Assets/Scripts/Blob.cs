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

public abstract class Blob : MonoBehaviour {

    [SerializeField]
    protected float baseWalkSpeed;

    protected abstract void ProcessMovement(float interval);

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
