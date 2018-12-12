using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6024 - AI
//Repo: https://github.com/danielmccluskey/CT6024-AI
//Script Purpose: Controls the spy and defines its goals
//////////////////////////////////////////////////////////////////
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

    /// <summary>
    /// Gets the current search point.
    /// </summary>
    /// <returns></returns>
    public GameObject GetCurrentSearchPoint()
    {
        m_PatrolPointsTarget.transform.position = m_ppCurrentPatrolPoint.m_v3PatrolPointPosition;

        return m_PatrolPointsTarget;
    }

    /// <summary>
    /// Gets the next search point.
    /// </summary>
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

    /// <summary>
    /// Gets the needs to hide.
    /// </summary>
    /// <returns></returns>
    public bool GetNeedsToHide()
    {
        return m_bNeedsToHide;
    }

    /// <summary>
    /// Gets the spy target.
    /// </summary>
    /// <returns></returns>
    public GameObject GetSpyTarget()
    {
        return m_PatrolPointsTarget;
    }

    /// <summary>
    /// Moves the target.
    /// </summary>
    /// <param name="a_v3Pos">Position to move it to</param>
    public void MoveTarget(Vector3 a_v3Pos)
    {
        m_PatrolPointsTarget.transform.position = a_v3Pos;
    }

    /// <summary>
    /// Sets the hide.
    /// </summary>
    /// <param name="a_bOn">if set to <c>true</c> [a b on].</param>
    public void SetHide(bool a_bOn)
    {
        m_bNeedsToHide = a_bOn;
        GetComponent<CS_AIAgent>().m_bInterrupt = true;
    }
}