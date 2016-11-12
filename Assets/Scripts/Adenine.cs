using UnityEngine;
using System.Collections;

public class Adenine : Nucleo {

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
                walkTime = 2f;
                currentDirection = possibleDirections[Random.Range(0, possibleDirections.Length)];
                walkVector = DirectionToVector(currentDirection);
            }
            else
            {
                walkTime -= interval;
                transform.position = transform.position + (Vector3)(walkVector * baseWalkSpeed * interval);
            }

            walking = walkTime > 0f;
            if (walkTime <= 0f)
            {
                walking = false;
                stopTime = 2f;
            }
        }
    }
}
