using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SpySight : MonoBehaviour
{
    public bool m_bCanSeeGuard = false;
    public bool m_bCanSeeIntel = false;
    public bool m_bCanSeeTotem = false;

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
        m_bCanSeeGuard = false;
        m_bCanSeeIntel = false;

        Collider[] cTargetsInViewRadius = Physics.OverlapSphere(transform.position, m_fViewRadius, m_lmTargetMask);
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

                    if (target.CompareTag("Guard"))
                    {
                        m_bCanSeeGuard = true;
                    }
                    if (target.CompareTag("Intel"))
                    {
                        HandleIntelSighting(target.gameObject);
                        m_bCanSeeIntel = true;
                        GetComponent<CS_AIAgent>().m_bInterrupt = true;
                    }
                    if (target.CompareTag("Totem"))
                    {
                        HandleTotemSighting(target.gameObject);
                        m_bCanSeeIntel = true;
                        m_bCanSeeTotem = true;
                        GetComponent<CS_AIAgent>().m_bInterrupt = true;
                    }
                }
            }
        }
    }

    private void HandleIntelSighting(GameObject a_goIntelInQuestion)
    {
        a_goIntelInQuestion.GetComponent<CS_KnowledgeComponent>().SetFound(true);
    }

    private void HandleTotemSighting(GameObject a_goIntelInQuestion)
    {
        CS_IntelComponent[] cIntelList = GameObject.FindObjectsOfType<CS_IntelComponent>();
        foreach (CS_IntelComponent cIntel in cIntelList)
        {
            cIntel.GetComponent<CS_KnowledgeComponent>().SetCollected(true);
        }
        a_goIntelInQuestion.GetComponent<CS_KnowledgeComponent>().SetFound(true);
    }

    public Vector3 DirectionFromAngle(float a_fAngleDegrees, bool a_bGlobalAngle)
    {
        if (!a_bGlobalAngle)
        {
            a_fAngleDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(a_fAngleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(a_fAngleDegrees * Mathf.Deg2Rad));
    }

    public GameObject GetVisibleGuard()
    {
        foreach (Transform tTarget in m_ltVisibleTargets)
        {
            if (tTarget.CompareTag("Guard"))
            {
                return tTarget.gameObject;
            }
        }
        return null;
    }
}