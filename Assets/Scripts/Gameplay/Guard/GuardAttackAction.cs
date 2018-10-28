using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAttackAction : GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bAttacking = false;

    public GuardAttackAction()
    {
        addPrecondition("seePlayer", true);
        addEffect("damagePlayer", true);
        addEffect("secureArea", true);
        cost = 1.0f;
    }

    public override void reset()
    {
        m_bAttacking = false;
        target = null;
    }

    public override bool isDone()
    {
        return m_bAttacking;
    }

    public override bool requiresInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        target = GameObject.FindGameObjectWithTag("Player");
        if (target != null)
        {
            return true;
        }
        return false;
    }

    public override bool perform(GameObject agent)
    {
        m_bAttacking = true;
        return true;
    }
}