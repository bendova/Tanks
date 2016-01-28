using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;

[RAINAction]
public class Attack : RAINAction
{
    public Expression Target;

    private TurretController m_TurretController;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        actionName = "Attack";
        m_TurretController = ai.Body.GetComponent<TurretController>();
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        GameObject target = Target.Evaluate<GameObject>(ai.DeltaTime, ai.WorkingMemory);
        ai.Motor.FaceAt(target.transform.position);
        if (m_TurretController.CanFire())
        {
            m_TurretController.FireProjectile(target.transform.position);
        }

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}