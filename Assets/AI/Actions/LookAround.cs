using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class LookAround : RAINAction
{
    private TurretController m_TurretController;
    private GameObject m_Turret;

    private float m_Angle;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        m_Angle = 0.0f;
        m_TurretController = ai.Body.GetComponent<TurretController>();
        m_Turret = m_TurretController.m_ShellSpawner.transform.parent.gameObject;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        ActionResult isDone = ActionResult.SUCCESS;
        if (m_Angle < 360f)
        {
            isDone = ActionResult.RUNNING;
            float angleInc = ai.DeltaTime * 40.0f;
            m_TurretController.TurnTurret(angleInc);
            m_Angle += angleInc;
        }
        return isDone;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}