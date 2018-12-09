using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GuardPatrolAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bisPatrolling = false;

    public CS_GuardPatrolAction()
    {
        AddPreCondition("seePlayer", false);
        AddEffect("secureArea", true);
        m_fCost = 1.0f;
    }

    public override void ResetGA()
    {
        m_bisPatrolling = false;
        m_goTarget = null;
    }

    public override bool IsActionFinished()
    {
        return m_bisPatrolling;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject agent)
    {
        m_goTarget = GetComponent<CS_Guard>().GetCurrentPatrolPoint();

        if (m_goTarget != null)
        {
            return true;
        }

        return false;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bisPatrolling = true;
        GetComponent<CS_Guard>().NextPatrolPoint();
        return true;
    }
}