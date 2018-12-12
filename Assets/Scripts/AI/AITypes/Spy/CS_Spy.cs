using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_Spy : CS_AIAgent
{
    public GameObject m_PatrolPointsTarget;

    private PatrolPoints m_ppCurrentPatrolPoint;
    private int m_iPatrolIndex;
    public bool m_bSeesGuard = false;
    private bool m_bNeedsToHide = false;

    // Start is called before the first frame update
    private void Start()
    {
        m_PatrolPointsTarget = new GameObject("SpyTarget");
        m_ppCurrentPatrolPoint = GetComponent<CS_GuardPatrolManager>().GetSinglePatrolPoint(0);
    }

    public override HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("escapeArea", true));
        goal.Add(new KeyValuePair<string, object>("avoidGuard", true));
        goal.Add(new KeyValuePair<string, object>("searchArea", true));
        goal.Add(new KeyValuePair<string, object>("getIntel", true));

        return goal;
    }

    public GameObject GetCurrentSearchPoint()
    {
        m_PatrolPointsTarget.transform.position = m_ppCurrentPatrolPoint.m_v3PatrolPointPosition;

        return m_PatrolPointsTarget;
    }

    public void NextSearchPoint()
    {
        m_iPatrolIndex++;
        if (m_iPatrolIndex >= GetComponent<CS_GuardPatrolManager>().GetAmountOfPoints())
        {
            GetComponent<CS_GuardPatrolManager>().ReCalculatePatrol();
            m_iPatrolIndex = 0;
        }
        m_ppCurrentPatrolPoint = GetComponent<CS_GuardPatrolManager>().GetSinglePatrolPoint(m_ppCurrentPatrolPoint.m_iNextPatrolIndex);
    }

    public bool GetNeedsToHide()
    {
        return m_bNeedsToHide;
    }

    public GameObject GetSpyTarget()
    {
        return m_PatrolPointsTarget;
    }

    public void MoveTarget(Vector3 a_v3Pos)
    {
        m_PatrolPointsTarget.transform.position = a_v3Pos;
    }

    public void SetHide(bool a_bOn)
    {
        m_bNeedsToHide = a_bOn;
        GetComponent<CS_AIAgent>().m_bInterrupt = true;
    }
}