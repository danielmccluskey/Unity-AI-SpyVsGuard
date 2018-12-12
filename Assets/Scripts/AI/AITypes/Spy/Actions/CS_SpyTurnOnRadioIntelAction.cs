using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SpyTurnOnRadioIntelAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bGuardDistracted = false;

    public CS_SpyTurnOnRadioIntelAction()
    {
        AddEffect("intelClearOfEnemies", true);
        // m_fCost = 1.0f;
    }

    public override void ResetGA()
    {
        //m_bGuardDistracted = false;
        m_goTarget = null;
    }

    public override bool IsActionFinished()
    {
        return m_bGuardDistracted;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject agent)
    {
        CS_RadioComponent[] goDistractingObjects = (CS_RadioComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(CS_RadioComponent));
        CS_RadioComponent goClosestDistraction = null;
        CS_IntelComponent cIntel = GameObject.FindObjectOfType<CS_IntelComponent>();
        if (cIntel == null)
        {
            return false;
        }
        float fDistanceToDistraction = 0;
        foreach (CS_RadioComponent distraction in goDistractingObjects)
        {
            if (goClosestDistraction == null)
            {
                // first one, so choose it for now
                goClosestDistraction = distraction;
                fDistanceToDistraction = (distraction.gameObject.transform.position - cIntel.transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                float dist = (distraction.gameObject.transform.position - cIntel.transform.position).magnitude;
                if (dist < fDistanceToDistraction)
                {
                    // we found a closer one, use it
                    goClosestDistraction = distraction;
                    fDistanceToDistraction = dist;
                }
            }
        }
        if (goClosestDistraction == null)
        {
            return false;
        }
        m_goTarget = goClosestDistraction.gameObject;

        if (m_goTarget != null)
        {
            return true;
        }

        return false;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bGuardDistracted = true;
        m_goTarget.GetComponent<CS_Radio>().SetRadioStatus(true);
        GameObject.FindObjectOfType<CS_Spy>().SetHide(true);

        return false;
    }
}