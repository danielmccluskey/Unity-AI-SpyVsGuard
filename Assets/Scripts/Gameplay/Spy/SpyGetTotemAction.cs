using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyGetTotemAction : GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bHasTotem = false;

    public SpyGetTotemAction()
    {
        addPrecondition("distractGuard", true);
        addEffect("getTotem", true);
        cost = 1.0f;
    }

    public override void reset()
    {
        m_bHasTotem = false;
        target = null;
    }

    public override bool isDone()
    {
        return m_bHasTotem;
    }

    public override bool requiresInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        TotemComponent goTotem = (TotemComponent)UnityEngine.GameObject.FindObjectOfType(typeof(TotemComponent));
        target = goTotem.gameObject;

        if (target != null)
        {
            return true;
        }

        return false;
    }

    public override bool perform(GameObject agent)
    {
        m_bHasTotem = true;
        return true;
    }
}