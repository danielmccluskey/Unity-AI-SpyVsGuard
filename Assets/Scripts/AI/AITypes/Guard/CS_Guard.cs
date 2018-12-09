using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_Guard : CS_AIAgent
{
    public GameObject m_PatrolPointsTarget;

    private PatrolPoints m_ppCurrentPatrolPoint;

    public bool m_bSeesPlayer = false;

    // Start is called before the first frame update
    private void Start()
    {
        m_ppCurrentPatrolPoint = GetComponent<CS_GuardPatrolManager>().GetSinglePatrolPoint(0);
        m_PatrolPointsTarget = new GameObject("GuardTarget");
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

        return goal;
    }

    public GameObject GetCurrentPatrolPoint()
    {
        m_PatrolPointsTarget.transform.position = m_ppCurrentPatrolPoint.m_v3PatrolPointPosition;

        return m_PatrolPointsTarget;
    }

    public void NextPatrolPoint()
    {
        m_ppCurrentPatrolPoint = GetComponent<CS_GuardPatrolManager>().GetSinglePatrolPoint(m_ppCurrentPatrolPoint.m_iNextPatrolIndex);
    }
}