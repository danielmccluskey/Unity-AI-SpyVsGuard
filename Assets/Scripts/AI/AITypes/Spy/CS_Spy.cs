using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_Spy : CS_AIAgent
{
    public GameObject m_PatrolPointsTarget;

    private PatrolPoints m_ppCurrentPatrolPoint;
    public bool m_bSeesGuard = false;

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

        return goal;
    }

    public GameObject GetCurrentSearchPoint()
    {
        m_PatrolPointsTarget.transform.position = m_ppCurrentPatrolPoint.m_v3PatrolPointPosition;

        return m_PatrolPointsTarget;
    }

    public void NextSearchPoint()
    {
        m_ppCurrentPatrolPoint = GetComponent<CS_GuardPatrolManager>().GetSinglePatrolPoint(m_ppCurrentPatrolPoint.m_iNextPatrolIndex);
    }
}