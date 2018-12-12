using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6024 - AI
//Repo: https://github.com/danielmccluskey/CT6024-AI
//Script Purpose: Controls the guard and defines its goals
//////////////////////////////////////////////////////////////////
public class CS_Guard : CS_AIAgent
{
    public GameObject m_PatrolPointsTarget;

    private PatrolPoints m_ppCurrentPatrolPoint;

    public bool m_bSeesPlayer = false;

    // Start is called before the first frame update
    private void Start()
    {
        m_PatrolPointsTarget = new GameObject("GuardTarget");
        m_PatrolPointsTarget.transform.position = transform.position;

        m_ppCurrentPatrolPoint = GetComponent<CS_GuardPatrolManager>().GetSinglePatrolPoint(0);
        GetComponent<NavMeshAgent>().SetAreaCost(14, 1);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public override HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("secureArea", true));
        goal.Add(new KeyValuePair<string, object>("damagePlayer", true));
        goal.Add(new KeyValuePair<string, object>("turnOffRadio", true));

        return goal;
    }

    public void ResetPointsForInvestigating()
    {
        m_ppCurrentPatrolPoint = GetComponent<CS_GuardPatrolManager>().GetSinglePatrolPoint(0);
        GetComponent<CS_AIAgent>().m_bInterrupt = true;
    }

    public GameObject GetCurrentPatrolPoint()
    {
        if (m_ppCurrentPatrolPoint.m_v3PatrolPointPosition == Vector3.zero)
        {
            m_ppCurrentPatrolPoint.m_v3PatrolPointPosition = transform.position;
        }
        m_PatrolPointsTarget.transform.position = m_ppCurrentPatrolPoint.m_v3PatrolPointPosition;

        return m_PatrolPointsTarget;
    }

    public void NextPatrolPoint()
    {
        m_ppCurrentPatrolPoint = GetComponent<CS_GuardPatrolManager>().GetSinglePatrolPoint(m_ppCurrentPatrolPoint.m_iNextPatrolIndex);
    }

    public void MoveTarget(Vector3 a_v3Pos)
    {
        m_PatrolPointsTarget.transform.position = a_v3Pos;
    }

    public GameObject GetSpyTarget()
    {
        return m_PatrolPointsTarget;
    }
}