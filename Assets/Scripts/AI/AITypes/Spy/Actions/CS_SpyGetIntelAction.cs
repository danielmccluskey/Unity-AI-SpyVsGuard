using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6024 - AI
//Repo: https://github.com/danielmccluskey/CT6024-AI
//Script Purpose: Makes the spy collect intel
//////////////////////////////////////////////////////////////////
public class CS_SpyGetIntelAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bHasIntel = false;

    public CS_SpyGetIntelAction()
    {
        AddEffect("knowsTotemLocation", true);
        AddEffect("getIntel", true);
        AddPreCondition("intelClearOfEnemies", true);
        // m_fCost = 1.0f;
    }

    public override void ResetGA()
    {
        m_bHasIntel = false;
        m_goTarget = null;
    }

    public override bool IsActionFinished()
    {
        return m_bHasIntel;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject agent)
    {
        CS_IntelComponent goIntel = (CS_IntelComponent)UnityEngine.GameObject.FindObjectOfType(typeof(CS_IntelComponent));

        m_goTarget = goIntel.gameObject;

        if (m_goTarget == null)
        {
            return false;
        }
        if (m_goTarget.GetComponent<CS_KnowledgeComponent>().HasBeenCollected())
        {
            return false;
        }
        if (m_goTarget.GetComponent<CS_KnowledgeComponent>().HasBeenLocated())
        {
            return true;
        }

        return false;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bHasIntel = true;

        m_goTarget.GetComponent<CS_KnowledgeComponent>().SetCollected(true);

        return true;
    }
}