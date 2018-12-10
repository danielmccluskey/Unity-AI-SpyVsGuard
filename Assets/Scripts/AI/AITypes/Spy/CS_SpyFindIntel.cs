using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SpyFindIntel : CS_GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bHasTotem = false;

    public CS_SpyFindIntel()
    {
        AddEffect("foundIntel", true);
        m_fCost = 1.0f;
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
        CS_IntelComponent goTotem = (CS_IntelComponent)UnityEngine.GameObject.FindObjectOfType(typeof(CS_IntelComponent));
        m_goTarget = goTotem.gameObject;

        if (!goTotem.GetComponent<CS_KnowledgeComponent>().HasBeenLocated())
        {
            return false;
        }

        if (m_goTarget != null)
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