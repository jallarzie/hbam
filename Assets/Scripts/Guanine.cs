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

            if (nucleo != this)
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
            if (!walking)
            {
                walking = true;
                animator.SetBool("isWalking", true);
            }
            transform.position = transform.position + (Vector3)((closestNucleo.transform.position - transform.position).normalized * baseWalkSpeed * interval);
        }
        else if (walking)
        {
            walking = false;
            animator.SetBool("isWalking", false);
        }
    }
}
