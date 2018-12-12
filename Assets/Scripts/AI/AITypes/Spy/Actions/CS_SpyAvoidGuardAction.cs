using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_SpyAvoidGuardAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bAvoidedGuard = false;

    public CS_SpyAvoidGuardAction()
    {
        AddEffect("avoidGuard", true);
        //m_fCost = 1.0f;
    }

    public override void ResetGA()
    {
        m_bAvoidedGuard = false;
        m_goTarget = null;
    }

    public override bool IsActionFinished()
    {
        return m_bAvoidedGuard;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject agent)
    {
        CS_SpySight cSpySightref = GetComponent<CS_SpySight>();
        GameObject goVisibleGuard = cSpySightref.GetVisibleGuard();
        if (goVisibleGuard == null)
        {
            return false;
        }
        float fDotProduct = Vector3.Dot(goVisibleGuard.transform.forward, (transform.position - goVisibleGuard.transform.position).normalized);
        if (fDotProduct < 0.5f)
        {
            return false;
        }

        //Idea for raycast from Callum Pertoldi
        RaycastHit rcHit;
        Transform tTarget = GetComponent<CS_Spy>().GetSpyTarget().transform;
        float fLeftDist, fRightDist, fBackDist;
        float fFurthest;
        Ray rLeftRay = new Ray(transform.position, Vector3.left);
        Ray rBackRay = new Ray(transform.position, Vector3.back);
        Ray rRightRay = new Ray(transform.position, Vector3.right);

        Physics.Raycast(rLeftRay, out rcHit);
        tTarget.position = rcHit.point;
        fLeftDist = rcHit.distance;
        fFurthest = fLeftDist;

        Physics.Raycast(rRightRay, out rcHit);
        fRightDist = rcHit.distance;
        if (fRightDist > fFurthest)
        {
            fFurthest = fRightDist;
            tTarget.position = rcHit.point;
        }

        Physics.Raycast(rBackRay, out rcHit);
        fBackDist = rcHit.distance;
        if (fBackDist > fFurthest)
        {
            fFurthest = fBackDist;
            tTarget.position = rcHit.point;
        }
        NavMeshHit nmHit;
        tTarget.position = tTarget.position + ((transform.position - tTarget.position) / 2);

        if (NavMesh.SamplePosition(tTarget.position, out nmHit, Mathf.Infinity, NavMesh.AllAreas))
        {
        }
        tTarget.transform.position = nmHit.position;

        GetComponent<CS_Spy>().MoveTarget(nmHit.position);

        m_goTarget = tTarget.gameObject;
        Debug.Log("Avoiding! ");

        if (m_goTarget != null)
        {
            return true;
        }

        return false;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bAvoidedGuard = true;

        return true;
    }
}