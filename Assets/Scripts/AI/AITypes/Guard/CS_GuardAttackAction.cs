using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GuardAttackAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bAttacking = false;

    public CS_GuardAttackAction()
    {
        AddPreCondition("seePlayer", true);
        AddEffect("damagePlayer", true);
        AddEffect("secureArea", true);
        m_fCost = 1.0f;
    }

    public override void ResetGA()
    {
        m_bAttacking = false;
        m_goTarget = null;
    }

    public override bool IsActionFinished()
    {
        return m_bAttacking;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject a_goAIAgent)
    {
        m_goTarget = GameObject.FindGameObjectWithTag("Player");
        if (m_goTarget != null)
        {
            return true;
        }
        return false;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bAttacking = true;
        return true;
    }
}