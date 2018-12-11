using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SpyEnemiesNearTotemAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = false;

    private bool m_bGuardDistracted = false;

    [SerializeField]
    private float m_fViewRadius;

    [SerializeField]
    private LayerMask m_lmTargetMask;

    public CS_SpyEnemiesNearTotemAction()
    {
        AddEffect("totemClearOfEnemies", true);
        // m_fCost = 1.0f;
    }

    public override void ResetGA()
    {
        //m_bGuardDistracted = false;
        m_goTarget = null;
    }

    public override bool IsActionFinished()
    {
        return m_bGuardDistracted;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject agent)
    {
        CS_TotemComponent cTotemRef = GameObject.FindObjectOfType<CS_TotemComponent>();
        if (cTotemRef == null)
        {
            return false;
        }
        Collider[] cTargetsInViewRadius = Physics.OverlapSphere(cTotemRef.transform.position, m_fViewRadius, m_lmTargetMask);//Get colliders in radius that we are interested in
        foreach (Collider cCollider in cTargetsInViewRadius)
        {
            if (cCollider.CompareTag("Guard"))
            {
                return false;
            }
        }
        return true;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bGuardDistracted = true;

        return true;
    }
}