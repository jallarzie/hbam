using UnityEngine;
using System.Collections;

public class Thymine : Nucleo {

    protected override void Init()
    {
        base.Init();
        nucleoType = NucleoType.Thymine;
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
                currentDirection = (Direction)(Random.Range(0, 4) * 2);
                walkVector = DirectionToVector(currentDirection);

                while (Physics2D.Raycast(transform.position, walkVector, 1f, LayerMask.GetMask("Boundaries")).collider != null)
                {
                    currentDirection = (Direction)(Random.Range(0, 4) * 2);
                    walkVector = DirectionToVector(currentDirection);
                }

                animator.SetBool("isWalking", true);
            }
            else
            {
                walkTime -= interval;
                if (Physics2D.Raycast(transform.position, walkVector, 1f, LayerMask.GetMask("Boundaries")).collider == null)
                {
                    transform.position = transform.position + (Vector3)(walkVector * baseWalkSpeed * interval * speedMultiplier);
                }
            }

            walking = walkTime > 0f;
            if (walkTime <= 0f)
            {
                walking = false;
                stopTime = kMaxStopTime / speedMultiplier;

                animator.SetBool("isWalking", false);
            }
        }
    }
}
