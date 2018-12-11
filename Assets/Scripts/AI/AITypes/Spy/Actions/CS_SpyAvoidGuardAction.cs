using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SpyAvoidGuardAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bAvoidedGuard = false;

    public CS_SpyAvoidGuardAction()
    {
        AddEffect("avoidGuard", true);
        //m_fCost = 1.0f;
    }

    public override void ResetGA()
    {
        m_bAvoidedGuard = false;
        m_goTarget = null;
    }

    public override bool IsActionFinished()
    {
        return m_bAvoidedGuard;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject agent)
    {
        CS_SpySight cSpySightref = GetComponent<CS_SpySight>();
        GameObject goVisibleGuard = cSpySightref.GetVisibleGuard();
        if (goVisibleGuard == null)
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
        m_bAvoidedGuard = true;

        return true;
    }
}