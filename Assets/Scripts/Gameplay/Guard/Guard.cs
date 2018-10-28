using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : AIAgent
{
    public Transform m_PatrolPointsHolder;

    private List<GameObject> m_lPatrolPoints;
    private int m_iCurrentPatrolPoint = 0;

    public bool m_bSeesPlayer = false;

    // Start is called before the first frame update
    private void Start()
    {
        health = 100;
        m_lPatrolPoints = new List<GameObject>();
        for (int i = 0; i < m_PatrolPointsHolder.childCount; i++)
        {
            m_lPatrolPoints.Add(m_PatrolPointsHolder.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("secureArea", true));
        goal.Add(new KeyValuePair<string, object>("damagePlayer", true));

        return goal;
    }

    public GameObject GetCurrentPatrolPoint()
    {
        if (m_iCurrentPatrolPoint >= m_lPatrolPoints.Count)
        {
            m_iCurrentPatrolPoint = 0;
        }
        return m_lPatrolPoints[m_iCurrentPatrolPoint];
    }

    public void NextPatrolPoint()
    {
        if (m_iCurrentPatrolPoint >= m_lPatrolPoints.Count)
        {
            m_iCurrentPatrolPoint = 0;
            return;
        }
        m_iCurrentPatrolPoint++;
    }
}