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

public class GuardPatrolManager : MonoBehaviour
{
    public List<PatrolPoints> m_lPatrolPointList;

    private Vector3 m_v3PatrolCenter;

    [SerializeField]
    private int m_iAmountOfPatrolPoints;

    [SerializeField]
    private float m_fPatrolRange;

    private NavMeshAgent m_aAgentRef;

    // Start is called before the first frame update
    private void Start()
    {
        m_lPatrolPointList = new List<PatrolPoints>();
        m_aAgentRef = GetComponent<NavMeshAgent>();
        ReCalculatePatrol();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void ChooseRoute()
    {
        //Find Closest Patrol Point
        //Set Index and Linked

        FindNearestUnusedNeighbor(1);
        //for (int i = 0; i < m_iAmountOfPatrolPoints; i++)
        //{
        //    m_lPatrolPointList[i].m_iNextPatrolIndex = FindNearestUnusedNeighbor(i);
        //    m_lPatrolPointList[i].m_bHasBeenLinked = true;
        //}
    }

    private void GeneratePositions()
    {
        for (int i = 0; i < m_iAmountOfPatrolPoints; i++)
        {
            PatrolPoints ppPatrolPlaceholder = new PatrolPoints();
            bool bCanReachTarget = false;
            while (bCanReachTarget == false)
            {
                ppPatrolPlaceholder.SetRandomPos(m_v3PatrolCenter, m_fPatrolRange);

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
        m_v3PatrolCenter = transform.position;
        GeneratePositions();
        ChooseRoute();
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
    }
}