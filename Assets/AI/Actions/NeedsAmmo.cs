using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;

[RAINDecision]
public class NeedsAmmo : RAINDecision
{
    public Expression MinStock;

    private int _lastRunning = 0;
    private TurretController m_TurretController;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);

        _lastRunning = 0;
        m_TurretController = ai.Body.GetComponent<TurretController>();
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        ActionResult tResult = ActionResult.FAILURE;

        if (MinStock != null)
        {
            int minStock = MinStock.Evaluate<int>(ai.DeltaTime, ai.WorkingMemory);
            bool needsAmmo = (m_TurretController.m_ShellsCount <= minStock);
            if (needsAmmo)
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