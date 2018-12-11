using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GuardSeePlayerAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = false;

    private bool m_bSeePlayer = false;

    public CS_GuardSeePlayerAction()
    {
        AddEffect("seePlayer", true);
        m_fCost = 1.0f;
    }

    public override void ResetGA()
    {
        m_bSeePlayer = false;
        m_goTarget = null;
    }

    public override bool IsActionFinished()
    {
        return m_bSeePlayer;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject a_goAIAgent)
    {
        if (GetComponent<CS_GuardSight>().m_bCanSeePlayer == true)
        {
            m_goTarget = GameObject.FindGameObjectWithTag("Player");
            if (m_goTarget != null)
            {
                return true;
            }
        }
        return false;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bSeePlayer = true;
        return true;
    }
}