using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSeePlayerAction : GOAPAction
{
    private bool m_bRequiresInRange = false;

    private bool m_bSeePlayer = false;

    public GuardSeePlayerAction()
    {
        addEffect("seePlayer", true);
        cost = 1.0f;
    }

    public override void reset()
    {
        m_bSeePlayer = false;
        target = null;
    }

    public override bool isDone()
    {
        return m_bSeePlayer;
    }

    public override bool requiresInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        if (GetComponent<GuardSight>().m_bCanSeePlayer == true)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            if (target != null)
            {
                return true;
            }
        }
        return false;
    }

    public override bool perform(GameObject agent)
    {
        m_bSeePlayer = true;
        return true;
    }
}