using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Followed the Sebastian Lague tutorial on Field of View to write this script
//https://www.youtube.com/watch?v=rQG9aUWarwE
//

public class CS_GuardSight : MonoBehaviour
{
    public bool m_bCanSeePlayer = false;
    private Transform m_tPlayersLastKnownPosition;
    private bool m_bShouldBeInvestigating = false;

    [SerializeField]
    private int m_iInvestigationEffort = 5;

    [SerializeField]
    private float m_fInvestigationRange = 5;

    public float m_fViewRadius;

    [Range(0, 359)]
    public float m_fViewAngle;

    public LayerMask m_lmTargetMask;
    public LayerMask m_lmObstacleMask;

    public List<Transform> m_ltVisibleTargets = new List<Transform>();

    private void Start()
    {
        StartCoroutine("FindTargets", 0.2f);
    }

    private IEnumerator FindTargets(float a_fDelay)
    {
        while (true)
        {
            yield return new WaitForSeconds(a_fDelay);
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        m_ltVisibleTargets.Clear();
        m_bCanSeePlayer = false;

        Collider[] cTargetsInViewRadius = Physics.OverlapSphere(transform.position, m_fViewRadius, m_lmTargetMask);

        if (cTargetsInViewRadius.Length <= 0)
        {
            if (m_bShouldBeInvestigating)
            {
                GetComponent<CS_AIAgent>().m_bInterrupt = true;
                GetComponent<CS_GuardPatrolManager>().InvestigateArea(m_tPlayersLastKnownPosition, m_iInvestigationEffort, m_fInvestigationRange);
                m_bShouldBeInvestigating = false;
            }
        }

        for (int i = 0; i < cTargetsInViewRadius.Length; i++)
        {
            Transform target = cTargetsInViewRadius[i].transform;
            Vector3 v3DirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, v3DirToTarget) < m_fViewAngle * 0.5f)
            {
                float fDistanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, v3DirToTarget, fDistanceToTarget, m_lmObstacleMask))
                {
                    m_ltVisibleTargets.Add(target);

                    if (target.CompareTag("Player"))
                    {
                        m_bCanSeePlayer = true;
                        GetComponent<CS_AIAgent>().m_bInterrupt = true;
                        m_tPlayersLastKnownPosition = target;
                        m_bShouldBeInvestigating = true;
                    }
                }
            }
        }
    }

    public Vector3 DirectionFromAngle(float a_fAngleDegrees, bool a_bGlobalAngle)
    {
        if (!a_bGlobalAngle)
        {
            a_fAngleDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(a_fAngleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(a_fAngleDegrees * Mathf.Deg2Rad));
    }
}