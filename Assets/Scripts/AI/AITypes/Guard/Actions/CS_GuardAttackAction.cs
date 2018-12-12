using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6024 - AI
//Repo: https://github.com/danielmccluskey/CT6024-AI
//Script Purpose: Makes the guard attack the player
//////////////////////////////////////////////////////////////////
public class CS_GuardAttackAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bAttacking = false;

    public CS_GuardAttackAction()
    {
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

        if (GetComponent<CS_GuardSight>().m_bCanSeePlayer)
        {
            if (m_goTarget != null)
            {
                return true;
            }
        }

        return false;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bAttacking = true;
        m_goTarget.GetComponent<CS_Spy>().SetHide(true);
        return true;
    }
}