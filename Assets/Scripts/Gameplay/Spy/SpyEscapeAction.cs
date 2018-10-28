using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyEscapeAction : GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bHasEscaped = false;

    public SpyEscapeAction()
    {
        addPrecondition("getTotem", true);
        addEffect("escapeArea", true);
        cost = 1.0f;
    }

    public override void reset()
    {
        m_bHasEscaped = false;
        target = null;
    }

    public override bool isDone()
    {
        return m_bHasEscaped;
    }

    public override bool requiresInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        EscapePointComponent[] goEscapePoints = (EscapePointComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(EscapePointComponent));
        EscapePointComponent goClosestPoint = null;
        float fDistanceToPoint = 0;
        foreach (EscapePointComponent distraction in goEscapePoints)
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
        target = goClosestPoint.gameObject;

        if (target != null)
        {
            return true;
        }

        return false;
    }

    public override bool perform(GameObject agent)
    {
        m_bHasEscaped = true;
        return true;
    }
}