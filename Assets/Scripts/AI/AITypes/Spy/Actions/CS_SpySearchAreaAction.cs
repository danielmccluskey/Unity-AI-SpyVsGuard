using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SpySearchAreaAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bisSearching = false;

    public CS_SpySearchAreaAction()
    {
        AddEffect("searchArea", true);
        m_fCost = 1.0f;
    }

    public override void ResetGA()
    {
        m_bisSearching = false;
        m_goTarget = null;
    }

    public override bool IsActionFinished()
    {
        return m_bisSearching;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject agent)
    {
        m_goTarget = GetComponent<CS_Spy>().GetCurrentSearchPoint();

        CS_SpySight cSpySight = GetComponent<CS_SpySight>();

        if (cSpySight.m_bCanSeeGuard || cSpySight.m_bCanSeeIntel)
        {
            return false;
        }
        if (m_goTarget != null)
        {
            return true;
        }

        return false;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bisSearching = true;
        GetComponent<CS_Spy>().NextSearchPoint();

        return true;
    }

    public bool GetActive()
    {
        return m_bisSearching;
    }
}