using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using System;
using RAIN.Perception.Sensors;

[RAINAction]
public class KeepTargetInRange : RAINAction
{
    public Expression TargetPos;
    public Expression MoveSpeed;
    public Expression Range;

    private TurretController m_TurretController;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        actionName = "KeepTargetInRange";
        m_TurretController = ai.Body.GetComponent<TurretController>();
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        RAINSensor sensor = ai.Senses.GetSensor("AttackRangeSensor");

        Vector3 targetPos = TargetPos.Evaluate<Vector3>(ai.DeltaTime, ai.WorkingMemory);
        float speed = MoveSpeed.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);
        float range = Range.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);

        float distanceToTarget = Vector3.Distance(ai.Body.transform.position, targetPos);
        if (distanceToTarget > range)
        {
            Vector3 dirToTarget = targetPos - ai.Body.transform.position;
            dirToTarget *= range/dirToTarget.magnitude;
            ai.Motor.Speed = speed;
            ai.Motor.MoveTo(ai.Body.transform.position + dirToTarget);
            m_TurretController.TurnTurretForward();
        }
        else
        {
            targetPos.y = m_TurretController.Turret.transform.position.y;
            m_TurretController.TurretLookAt(targetPos);
        }

        return ActionResult.SUCCESS;
    }

    private bool IsInRange(RAIN.Core.AI ai, Vector3 targetPos, float range)
    {
        return (Vector3.Distance(ai.Body.transform.position, targetPos) < range);
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}