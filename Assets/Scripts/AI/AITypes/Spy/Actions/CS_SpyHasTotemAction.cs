using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6024 - AI
//Repo: https://github.com/danielmccluskey/CT6024-AI
//Script Purpose: Checks if the spy has the totem
//////////////////////////////////////////////////////////////////
public class CS_SpyHasTotemAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = false;

    private bool m_bHasTotem = false;

    public CS_SpyHasTotemAction()
    {
        AddEffect("getTotem", true);
        //m_fCost = 1.0f;
    }

    public override void ResetGA()
    {
        m_bHasTotem = false;
        m_goTarget = null;
    }

    public override bool IsActionFinished()
    {
        return m_bHasTotem;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject agent)
    {
        CS_TotemComponent goTotem = (CS_TotemComponent)UnityEngine.GameObject.FindObjectOfType(typeof(CS_TotemComponent));
        m_goTarget = goTotem.gameObject;

        if (m_goTarget == null)
        {
            return false;
        }
        if (m_goTarget.GetComponent<CS_KnowledgeComponent>().HasBeenCollected())
        {
            return true;
        }

        return false;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bHasTotem = true;
        return true;
    }
}