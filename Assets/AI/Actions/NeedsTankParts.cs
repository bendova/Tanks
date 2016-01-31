using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using System;

[RAINDecision]
public class NeedsTankParts : RAINDecision
{
    public Expression MinHitPoints;

    private int _lastRunning = 0;
    private HitPoints m_HitPoints;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);

        _lastRunning = 0;
        m_HitPoints = ai.Body.GetComponent<HitPoints>();
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        ActionResult tResult = ActionResult.FAILURE;
        if (MinHitPoints != null)
        {
            int minHp = MinHitPoints.Evaluate<int>(ai.DeltaTime, ai.WorkingMemory);
            bool needsTankParts = (m_HitPoints.m_HitPoints <= minHp);
            if (needsTankParts)
            {
                tResult = ProcessChildNodes(ai);
            }
        }

        return tResult;
    }

    private ActionResult ProcessChildNodes(RAIN.Core.AI ai)
    {
        ActionResult tResult = ActionResult.SUCCESS;
        for (; _lastRunning < _children.Count; _lastRunning++)
        {
            tResult = _children[_lastRunning].Run(ai);
            if (tResult != ActionResult.SUCCESS)
                break;
        }
        return tResult;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}