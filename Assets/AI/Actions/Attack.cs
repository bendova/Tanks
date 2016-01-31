using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using System;

[RAINAction]
public class Attack : RAINAction
{
    public Expression Target;

    private TurretController m_TurretController;
    private GameObject m_Turret;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        actionName = "Attack";
        m_TurretController = ai.Body.GetComponent<TurretController>();
        m_Turret = m_TurretController.m_ShellSpawner.transform.parent.gameObject;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        ActionResult result = ActionResult.FAILURE;
        GameObject target = Target.Evaluate<GameObject>(ai.DeltaTime, ai.WorkingMemory);
        if (IsTargetInSights(ai, target) && m_TurretController.CanFire())
        {
            m_TurretController.FireProjectile(target.transform.position);
            result = ActionResult.SUCCESS;
        }

        return result;
    }

    private bool IsTargetInSights(RAIN.Core.AI ai, GameObject target)
    {
        bool isInSights = false;
        RaycastHit hit;
        Transform turret = m_Turret.transform;
        if (Physics.Raycast(ai.Body.transform.position, turret.forward, out hit))
        {
            isInSights = (hit.transform.gameObject == target);
        }

        return isInSights;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}