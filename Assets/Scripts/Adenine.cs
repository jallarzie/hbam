using UnityEngine;
using System.Collections;

public class Adenine : Nucleo {

    protected override void Init()
    {
        base.Init();
        nucleoType = NucleoType.Adenine;
    }

    protected override void ProcessMovement(float interval)
    {
        if (stopTime > 0f)
        {
            stopTime -= interval;
        }
        else
        {
            if (!walking)
            {
                walkTime = kMaxWalkTime;
                currentDirection = (Direction)Random.Range(0, 8);
                walkVector = DirectionToVector(currentDirection);
                walking = true;
                animator.SetBool("isWalking", true);
            }
            else
            {
                walkTime -= interval;
                if (Physics2D.Raycast(transform.position, walkVector, 1f, LayerMask.GetMask("Boundaries")).collider == null)
                {
                    transform.position = transform.position + (Vector3)(walkVector * baseWalkSpeed * interval);
                }
            }

            walking = walkTime > 0f;
            if (walkTime <= 0f)
            {
                walking = false;
                stopTime = kMaxStopTime;
                animator.SetBool("isWalking", false);
            }
        }
    }
}
