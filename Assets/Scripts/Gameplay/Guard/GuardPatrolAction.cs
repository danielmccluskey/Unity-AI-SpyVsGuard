using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPatrolAction : GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bisPatrolling = false;

    public GuardPatrolAction()
    {
        addPrecondition("seePlayer", false);
        addEffect("secureArea", true);
        cost = 1.0f;
    }

    public override void reset()
    {
        m_bisPatrolling = false;
        target = null;
    }

    public override bool isDone()
    {
        return m_bisPatrolling;
    }

    public override bool requiresInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        target = GetComponent<Guard>().GetCurrentPatrolPoint();

        if (target != null)
        {
            return true;
        }

        return false;
    }

    public override bool perform(GameObject agent)
    {
        m_bisPatrolling = true;
        GetComponent<Guard>().NextPatrolPoint();
        return true;
    }
}