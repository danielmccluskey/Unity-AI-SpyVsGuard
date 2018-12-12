using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_SpyHideAction : CS_GOAPAction
{
    private bool m_bRequiresInRange = true;

    private bool m_bIshidden = false;
    private bool m_bRunTimer = false;

    [SerializeField]
    private float m_fHideTime;

    private float m_fHideTimer;

    private void Start()
    {
        m_fHideTimer = m_fHideTime;
    }

    public CS_SpyHideAction()
    {
        AddEffect("avoidGuard", true);
        AddEffect("totemClearOfEnemies", false);
        AddEffect("intelClearOfEnemies", false);

        // m_fCost = 1.0f;
    }

    private void Update()
    {
        if (m_bRunTimer)

        {
            m_fHideTimer -= Time.deltaTime;
        }
    }

    public override void ResetGA()
    {
        m_goTarget = null;
        m_bIshidden = false;
    }

    public override bool IsActionFinished()
    {
        return m_bIshidden;
    }

    public override bool NeedsToBeInRange()
    {
        return m_bRequiresInRange;
    }

    public override bool CheckPreCondition(GameObject agent)
    {
        CS_Spy cSpyRef = GetComponent<CS_Spy>();
        if (cSpyRef == null || !cSpyRef.GetNeedsToHide())
        {
            return false;
        }

        CS_HidingComponent[] cHidingLocations = GameObject.FindObjectsOfType<CS_HidingComponent>();
        CS_HidingComponent cClosestHidingSpot = null;

        NavMeshAgent nmaAgentRef = GetComponent<NavMeshAgent>();
        float fDistanceToPoint = 0;

        foreach (CS_HidingComponent cHide in cHidingLocations)
        {
            if (!cHide.GetActive())
            {
                continue;
            }
            NavMeshPath nmpPath = new NavMeshPath();//Create new nav path
            nmaAgentRef.CalculatePath(cHide.transform.position, nmpPath);
            //Path length function from here https://forum.unity.com/threads/getting-the-distance-in-nav-mesh.315846/

            if (cClosestHidingSpot == null)
            {
                //Set first Point to closest, others will compare to this
                cClosestHidingSpot = cHide;
                fDistanceToPoint = GetPathLength(nmpPath);
            }
            else
            {
                //Checks if the current Point is closer than the last one
                float fdist = GetPathLength(nmpPath);
                if (fdist < fDistanceToPoint)
                {
                    //Use the closer Point instead
                    cClosestHidingSpot = cHide;
                    fDistanceToPoint = fdist;
                }
            }
        }

        m_goTarget = cClosestHidingSpot.gameObject;

        if (m_goTarget != null)
        {
            return true;
        }

        return false;
    }

    public override bool PerformAction(GameObject agent)
    {
        m_bIshidden = true;
        m_bRunTimer = true;
        if (m_fHideTimer <= 0)
        {
            GetComponent<CS_Spy>().SetHide(false);
            m_fHideTimer = m_fHideTime;
            GetComponent<CS_AIAgent>().m_bInterrupt = true;//Interrupt current action
            m_bRunTimer = false;
            return true;
        }
        return false;
    }

    private float GetPathLength(NavMeshPath a_nmpPath)
    {
        float fLength = 0.0f;

        if ((a_nmpPath.status != NavMeshPathStatus.PathInvalid) && (a_nmpPath.corners.Length >= 1))
        {
            for (int i = 1; i < a_nmpPath.corners.Length; ++i)
            {
                fLength += Vector3.Distance(a_nmpPath.corners[i - 1], a_nmpPath.corners[i]);
            }
        }

        return fLength;
    }
}