using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GuardTurnOffRadioAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bHasTurnedOffRadio = false;

    public CS_GuardTurnOffRadioAction()
    {
        AddEffect("turnOffRadio", true);
        m_fCost = 1.0f;
    }

    public override void ResetGA()
    {
        m_bHasTurnedOffRadio = false;
        m_goTarget = null;
    }

    public override bool IsActionFinished()
    {
        return m_bHasTurnedOffRadio;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject agent)
    {
        CS_GuardHearing cHearingRef = GetComponent<CS_GuardHearing>();

        if (cHearingRef == null || !cHearingRef.GetCanHearRadio())
        {
            return false;
        }

        m_goTarget = cHearingRef.GetSoundLocation().gameObject;

        if (m_goTarget != null)
        {
            return true;
        }

        return false;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bHasTurnedOffRadio = true;
        CS_Guard[] cGuardList = FindObjectsOfType<CS_Guard>();
        foreach (CS_Guard cGuard in cGuardList)
        {
            cGuard.GetComponent<CS_AIAgent>().m_bInterrupt = true;
            cGuard.GetComponent<CS_GuardHearing>().TurnedRadioOff();
        }
        GetComponent<CS_GuardPatrolManager>().InvestigateArea(m_goTarget.transform, 5, 5);//Investigate the last known location of the player

        return true;
    }
}