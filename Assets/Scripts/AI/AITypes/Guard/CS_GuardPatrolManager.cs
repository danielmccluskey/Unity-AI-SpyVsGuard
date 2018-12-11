using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PatrolPoints
{
    public PatrolPoints()
    {
    }

    public Vector3 m_v3PatrolPointPosition;

    public int m_iIndex;

    public int m_iNextPatrolIndex;

    public bool m_bHasBeenLinked;

    public void SetRandomPos(Vector3 a_v3Pos, float a_fRange)
    {
        Vector2 v2RandPosition = Random.insideUnitCircle * a_fRange;
        m_v3PatrolPointPosition = a_v3Pos += new Vector3(v2RandPosition.x, 0, v2RandPosition.y);
    }
}

public class CS_GuardPatrolManager : MonoBehaviour
{
    public List<PatrolPoints> m_lPatrolPointList;

    private List<PatrolPoints> m_lOriginalPatrolPoints;

    private Vector3 m_v3PatrolCenter;

    [SerializeField]
    private int m_iAmountOfPatrolPoints;

    [SerializeField]
    private float m_fPatrolRange;

    private NavMeshAgent m_aAgentRef;

    private bool m_bInvestigationMode = false;
    private float m_fInvestigationTime = 10.0f;
    private float m_fInvestigationTimer = 10.0f;

    private void Awake()
    {
        m_lPatrolPointList = new List<PatrolPoints>();
        PatrolPoints ppPlaceholders = new PatrolPoints();
        for (int i = 0; i < m_iAmountOfPatrolPoints; i++)
        {
            m_lPatrolPointList.Add(ppPlaceholders);
        }
        m_aAgentRef = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        ReCalculatePatrol();
        m_fInvestigationTimer = m_fInvestigationTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_bInvestigationMode)
        {
            m_fInvestigationTimer -= Time.deltaTime;
            if (m_fInvestigationTimer <= 0)
            {
                StopInvestigating();
            }
        }
    }

    private void ChooseRoute()
    {
        //Find Closest Patrol Point
        //Set Index and Linked

        FindNearestUnusedNeighbor(1);
    }

    private void GeneratePositions(int a_iAmountOfPoints, float a_fRange)
    {
        for (int i = 0; i < a_iAmountOfPoints; i++)
        {
            PatrolPoints ppPatrolPlaceholder = new PatrolPoints();
            bool bCanReachTarget = false;
            while (bCanReachTarget == false)
            {
                ppPatrolPlaceholder.SetRandomPos(m_v3PatrolCenter, a_fRange);

                NavMeshPath nmpPath = new NavMeshPath();
                m_aAgentRef.CalculatePath(ppPatrolPlaceholder.m_v3PatrolPointPosition, nmpPath);

                if (nmpPath.status == NavMeshPathStatus.PathPartial || nmpPath.status == NavMeshPathStatus.PathInvalid)
                {
                    Debug.Log("Bad Point");
                }
                else
                {
                    bCanReachTarget = true;
                }
            }
            ppPatrolPlaceholder.m_iIndex = i;
            m_lPatrolPointList.Add(ppPatrolPlaceholder);
        }
    }

    private int FindNearestUnusedNeighbor(int a_iIndex)
    {
        bool bTestBool = true;
        int iNextIndex = 0;
        int infiniteloopsafety = 0;
        while (bTestBool)
        {
            //Get NextIndex
            //Find Closest unused point
            //Set that as next index
            PatrolPoints goClosestTurret = null;//Temp storage for closest turret
            float fDistanceToTurret = 0.0f;
            foreach (PatrolPoints Turret in m_lPatrolPointList)
            {
                if (Turret.m_iIndex == iNextIndex || Turret.m_bHasBeenLinked)
                {
                    continue;
                }
                if (goClosestTurret == null)
                {
                    //Set first turret to closest, others will compare to this
                    goClosestTurret = Turret;
                    fDistanceToTurret = (Turret.m_v3PatrolPointPosition - m_lPatrolPointList[a_iIndex].m_v3PatrolPointPosition).magnitude;
                }
                else
                {
                    //Checks if the current turret is closer than the last one
                    float fdist = (Turret.m_v3PatrolPointPosition - m_lPatrolPointList[a_iIndex].m_v3PatrolPointPosition).magnitude;
                    if (fdist < fDistanceToTurret)
                    {
                        //Use the closer turret instead
                        goClosestTurret = Turret;
                        fDistanceToTurret = fdist;
                    }
                }
            }

            if (goClosestTurret == null)
            {
                goClosestTurret = m_lPatrolPointList[0];
            }

            m_lPatrolPointList[iNextIndex].m_iNextPatrolIndex = goClosestTurret.m_iIndex;
            m_lPatrolPointList[iNextIndex].m_bHasBeenLinked = true;
            iNextIndex = goClosestTurret.m_iIndex;

            if (RouteFinishCheck())
            {
                bTestBool = false;
            }

            infiniteloopsafety++;
            if (infiniteloopsafety > m_iAmountOfPatrolPoints + 10)
            {
                bTestBool = false;
                Debug.Log("InfiniteLoop");
            }
        }
        return 0;
    }

    private bool RouteFinishCheck()
    {
        foreach (PatrolPoints Turret in m_lPatrolPointList)
        {
            if (Turret.m_bHasBeenLinked == false)
            {
                return false;
            }
        }
        return true;
    }

    public void SetPatrolCenter(Vector3 a_v3Position)
    {
        m_v3PatrolCenter = a_v3Position;
    }

    public void ReCalculatePatrol()
    {
        m_bInvestigationMode = false;
        m_v3PatrolCenter = transform.position;
        m_lPatrolPointList.Clear();
        GeneratePositions(m_iAmountOfPatrolPoints, m_fPatrolRange);
        ChooseRoute();
    }

    public void StopInvestigating()
    {
        m_lPatrolPointList.Clear();
        m_lPatrolPointList = new List<PatrolPoints>(m_lOriginalPatrolPoints);
        m_bInvestigationMode = false;
        m_fInvestigationTimer = m_fInvestigationTime;
        CS_GuardPatrolAction cGuardPatrol = GetComponent<CS_GuardPatrolAction>();
        if (cGuardPatrol != null)
        {
            cGuardPatrol.InvestigationMode(false);
        }
    }

    public void InvestigateArea(Transform a_tPositionToInvestigate, int a_iAmountOfPoints, float a_fRangeToInvestigate)
    {
        if (!m_bInvestigationMode)
        {
            m_lOriginalPatrolPoints = new List<PatrolPoints>(m_lPatrolPointList);
        }
        m_bInvestigationMode = true;
        m_fInvestigationTimer = m_fInvestigationTime;
        m_v3PatrolCenter = a_tPositionToInvestigate.position;
        m_lPatrolPointList.Clear();
        GeneratePositions(a_iAmountOfPoints, a_fRangeToInvestigate);
        ChooseRoute();
        GetComponent<CS_Guard>().ResetPointsForInvestigating();
        CS_GuardPatrolAction cGuardPatrol = GetComponent<CS_GuardPatrolAction>();
        if (cGuardPatrol != null)
        {
            cGuardPatrol.InvestigationMode(true);
        }
    }

    public PatrolPoints GetSinglePatrolPoint(int a_iIndex)
    {
        return m_lPatrolPointList[a_iIndex];
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position

        foreach (PatrolPoints ppPIQ in m_lPatrolPointList)
        {
            if (ppPIQ == null)//Null Check
            {
                return;
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(ppPIQ.m_v3PatrolPointPosition, 1);
        }

        if (m_bInvestigationMode)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_v3PatrolCenter, 1);
        }
    }

    public int GetAmountOfPoints()
    {
        return m_lPatrolPointList.Count;
    }
}