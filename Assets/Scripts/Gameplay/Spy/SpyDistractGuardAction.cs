using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyDistractGuardAction : GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bGuardDistracted = false;

    public SpyDistractGuardAction()
    {
        addEffect("distractGuard", true);
        cost = 1.0f;
    }

    public override void reset()
    {
        m_bGuardDistracted = false;
        target = null;
    }

    public override bool isDone()
    {
        return m_bGuardDistracted;
    }

    public override bool requiresInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        DistractionComponent[] goDistractingObjects = (DistractionComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(DistractionComponent));
        DistractionComponent goClosestDistraction = null;
        float fDistanceToDistraction = 0;
        foreach (DistractionComponent distraction in goDistractingObjects)
        {
            if (goClosestDistraction == null)
            {
                // first one, so choose it for now
                goClosestDistraction = distraction;
                fDistanceToDistraction = (distraction.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                float dist = (distraction.gameObject.transform.position - agent.transform.position).magnitude;
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
        target = goClosestDistraction.gameObject;

        if (target != null)
        {
            return true;
        }

        return false;
    }

    public override bool perform(GameObject agent)
    {
        m_bGuardDistracted = true;
        return true;
    }
}