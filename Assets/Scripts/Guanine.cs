using UnityEngine;
using System.Collections;

public class Guanine : Nucleo {

    protected override void Init()
    {
        base.Init();
        nucleoType = NucleoType.Guanine;
    }

    [SerializeField]
    private float followDistance;
    [SerializeField]
    private float stopDistance;

    protected override void ProcessMovement(float interval)
    {
        int nucleoCount = BoardController.instance.nucleoCount;

        Nucleo closestNucleo = null;
        float closestNucleoDistance = 0f;

        for (int i = 0; i < nucleoCount; i++)
        {
            Nucleo nucleo = BoardController.instance.GetNucleo(i);

            if (nucleo.nucleoType != nucleoType && !nucleo.followed)
            {
                float nucleoDistance = Vector3.Distance(nucleo.transform.position, transform.position);

                if (nucleoDistance < followDistance && (closestNucleo == null || nucleoDistance < closestNucleoDistance))
                {
                    closestNucleo = nucleo;
                    closestNucleoDistance = nucleoDistance;
                }
             }
        }

        if (closestNucleo != null && closestNucleoDistance > stopDistance)
        {
            closestNucleo.followed = true;
            if (!walking)
            {
                walking = true;
                animator.SetBool("isWalking", true);
            }
            walkVector = (closestNucleo.transform.position - transform.position).normalized;
            if (Physics2D.Raycast(transform.position, walkVector, 1f, LayerMask.GetMask("Boundaries")).collider == null)
            {
                transform.position = transform.position + (Vector3)(walkVector * baseWalkSpeed * interval);
            }
        }
        else if (walking)
        {
            walking = false;
            animator.SetBool("isWalking", false);
        }
    }
}
