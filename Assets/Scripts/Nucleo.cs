﻿using UnityEngine;
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
    Adenine,
    Thymine,
    Cytosine,
    Guanine
}

public abstract class Nucleo : MonoBehaviour {

    [SerializeField]
    protected float kMaxWalkTime = 2f;

    [SerializeField]
    protected float kMaxStopTime = 2f;

    [SerializeField]
    protected float baseWalkSpeed;

    protected Animator animator;

    protected float stopTime;
    protected float walkTime;
    protected bool walking;
    protected float cooldown;

    protected Direction currentDirection;
    protected Vector2 walkVector;

    public NucleoType nucleoType { get; protected set; }

    public bool followed { get; set; }

    protected abstract void ProcessMovement(float interval);

    public float speedMultiplier { get; set; }

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        animator = GetComponent<Animator>();
        followed = false;
    }

    private void Update()
    {
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime; 
        }
        else
        {
            ProcessMovement(Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        followed = false;
    }

    public void Match()
    {
        animator.SetTrigger("reactTrigger");
        cooldown = 1.7f;
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
