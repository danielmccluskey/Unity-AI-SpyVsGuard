using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Followed the Sebastian Lague tutorial on Field of View to write this script
//https://www.youtube.com/watch?v=rQG9aUWarwE
//

public class CS_GuardSight : MonoBehaviour
{
    private bool m_bShouldBeInvestigating = false;//Should I be investigating right now

    [SerializeField]
    private float m_fInvestigationRange = 5;//The range from the center that I should investigate

    [SerializeField]
    private int m_iInvestigationEffort = 5;//The amount of points I should investigate with

    private Transform m_tPlayersLastKnownPosition;//The players last known position
    public bool m_bCanSeePlayer = false;//Can I see the player
    public bool m_bCouldSeePlayer = false;//Could I see the player?

    [Range(0, 359)]
    public float m_fViewAngle;//FOV

    public float m_fViewRadius;//The range I can see
    public LayerMask m_lmObstacleMask;//What are obstacles
    public LayerMask m_lmTargetMask;//What sh
    public List<Transform> m_ltVisibleTargets = new List<Transform>();

    [SerializeField]
    private GameObject m_goAlertSoundObject;

    private bool m_bAlertPlayed = false;

    private void Start()
    {
        StartCoroutine("FindTargets", 0.2f);
    }

    /// <summary>
    /// Finds visible targets with a delay
    /// </summary>
    /// <param name="a_fDelay">The delay.</param>
    /// <returns></returns>
    private IEnumerator FindTargets(float a_fDelay)
    {
        while (true)
        {
            yield return new WaitForSeconds(a_fDelay);
            FindVisibleTargets();
        }
    }

    /// <summary>
    /// Finds visible targets with colliders.
    /// </summary>
    private void FindVisibleTargets()
    {
        m_ltVisibleTargets.Clear();//Clear the targets list
        m_bCanSeePlayer = false;//Set

        Collider[] cTargetsInViewRadius = Physics.OverlapSphere(transform.position, m_fViewRadius, m_lmTargetMask);//Get colliders in radius that we are interested in

        for (int i = 0; i < cTargetsInViewRadius.Length; i++)//Loop through this list
        {
            Transform target = cTargetsInViewRadius[i].transform;//Set the target to the current iteration
            Vector3 v3DirToTarget = (target.position - transform.position).normalized;//Get the direction from me to the target
            if (Vector3.Angle(transform.forward, v3DirToTarget) < m_fViewAngle * 0.5f)//Check if the target is in my fov
            {
                float fDistanceToTarget = Vector3.Distance(transform.position, target.position);//Calculate distance from me to target
                if (!Physics.Raycast(transform.position, v3DirToTarget, fDistanceToTarget, m_lmObstacleMask))//Check if its in my range
                {
                    m_ltVisibleTargets.Add(target);//Add that target to the list

                    if (target.CompareTag("Player"))//If it is the player
                    {
                        m_bCanSeePlayer = true;
                        m_bCouldSeePlayer = true;
                        GetComponent<CS_AIAgent>().m_bInterrupt = true;//Interrupt current action
                        m_tPlayersLastKnownPosition = target;
                        m_bShouldBeInvestigating = true;
                    }

                    if (target.CompareTag("Guard"))
                    {
                        if (target.GetComponent<CS_GuardSight>().m_bCanSeePlayer)
                        {
                            m_bCanSeePlayer = true;
                            m_bCouldSeePlayer = true;
                            GetComponent<CS_AIAgent>().m_bInterrupt = true;//Interrupt current action
                            m_tPlayersLastKnownPosition = target.GetComponent<CS_GuardSight>().m_tPlayersLastKnownPosition;
                            Debug.Log("Another guard can see player!");
                        }
                    }
                    if (target.CompareTag("HidingSpot"))
                    {
                        target.GetComponent<CS_HidingComponent>().m_bActive = false;
                    }
                }
            }
        }

        if (m_bCouldSeePlayer && m_bCanSeePlayer && !m_bAlertPlayed)
        {
            m_bAlertPlayed = true;
            GameObject goAlert = Instantiate(m_goAlertSoundObject, transform);
            goAlert.GetComponent<CS_SoundComponent>().StopSound();
        }
        if (m_bCouldSeePlayer && !m_bCanSeePlayer)//If guard has lost sight of the player
        {
            if (m_bShouldBeInvestigating)//If they should be investigating this
            {
                GetComponent<CS_AIAgent>().m_bInterrupt = true;
                GetComponent<CS_GuardPatrolManager>().InvestigateArea(m_tPlayersLastKnownPosition, m_iInvestigationEffort, m_fInvestigationRange);//Investigate the last known location of the player
                m_bShouldBeInvestigating = false;//reset
                m_bCouldSeePlayer = false;//reset
                m_bAlertPlayed = false;
            }
        }
    }

    /// <summary>
    /// Gets a direction vector from an angle in degrees;
    /// </summary>
    /// <param name="a_fAngleDegrees">The angle to turn into direction.</param>
    /// <param name="a_bGlobalAngle">if set to <c>true</c> [a b global angle].</param>
    /// <returns></returns>
    public Vector3 DirectionFromAngle(float a_fAngleDegrees, bool a_bGlobalAngle)
    {
        if (!a_bGlobalAngle)
        {
            a_fAngleDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(a_fAngleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(a_fAngleDegrees * Mathf.Deg2Rad));
    }
}