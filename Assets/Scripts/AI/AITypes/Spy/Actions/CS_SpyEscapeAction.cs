using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6024 - AI
//Repo: https://github.com/danielmccluskey/CT6024-AI
//Script Purpose: Makes the spy escape the level
//////////////////////////////////////////////////////////////////
public class CS_SpyEscapeAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bHasEscaped = false;

    public CS_SpyEscapeAction()
    {
        AddPreCondition("getTotem", true);
        AddEffect("escapeArea", true);
        //m_fCost = 1.0f;
    }

    public override void ResetGA()
    {
        m_bHasEscaped = false;
        m_goTarget = null;
    }

    public override bool IsActionFinished()
    {
        return m_bHasEscaped;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject agent)
    {
        CS_EscapePointComponent[] goEscapePoints = (CS_EscapePointComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(CS_EscapePointComponent));
        CS_EscapePointComponent goClosestPoint = null;
        float fDistanceToPoint = 0;
        foreach (CS_EscapePointComponent distraction in goEscapePoints)
        {
            if (goClosestPoint == null)
            {
                // first one, so choose it for now
                goClosestPoint = distraction;
                fDistanceToPoint = (distraction.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                float dist = (distraction.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < fDistanceToPoint)
                {
                    // we found a closer one, use it
                    goClosestPoint = distraction;
                    fDistanceToPoint = dist;
                }
            }
        }
        if (goClosestPoint == null)
        {
            return false;
        }
        m_goTarget = goClosestPoint.gameObject;

        if (m_goTarget != null)
        {
            return true;
        }

        return false;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bHasEscaped = true;
        return true;
    }
}