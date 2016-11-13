using UnityEngine;
using System.Collections;

public class Cytosine : Nucleo {

    protected override void Init()
    {
        base.Init();
        nucleoType = NucleoType.Cytosine;
    }

    private Vector3 position;

    [SerializeField]
    private float sineMagnitude = 5f;
    [SerializeField]
    private float sineFrequency = 5f;

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

                position = transform.position;

                animator.SetBool("isWalking", true);
            }
            else
            {
                walkTime -= interval;
                position += (Vector3)walkVector * interval * baseWalkSpeed;
                transform.position = position + new Vector3(walkVector.y, -walkVector.x, 0f) * Mathf.Sin((kMaxWalkTime - walkTime) * sineFrequency) * sineMagnitude;
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
