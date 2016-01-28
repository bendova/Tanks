using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;

[RAINAction]
public class LookAtTarget : RAINAction
{
    public Expression Target;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        actionName = "LookAtTarget";
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        GameObject target = Target.Evaluate<GameObject>(ai.DeltaTime, ai.WorkingMemory);

        ai.Motor.FaceAt(target.transform.position);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}