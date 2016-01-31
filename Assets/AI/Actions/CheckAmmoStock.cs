using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;

[RAINAction]
public class CheckAmmoStock : RAINAction
{
    public Expression NeedAmmoVar;
    public Expression MinStock;

    private TurretController m_TurretController;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);

        m_TurretController = ai.Body.GetComponent<TurretController>();
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (NeedAmmoVar != null && MinStock != null)
        {
            string ammoVar = NeedAmmoVar.Evaluate<string>(ai.DeltaTime, ai.WorkingMemory);
            int minStock = MinStock.Evaluate<int>(ai.DeltaTime, ai.WorkingMemory);
            bool needsAmmo = (m_TurretController.m_ShellsCount <= minStock);

            ai.WorkingMemory.SetItem(ammoVar, needsAmmo);
        }

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}