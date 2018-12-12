using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PatrolPoints
{
    /// <summary>
    /// Constructor
    /// </summary>
    public PatrolPoints()
    {
    }

    public Vector3 m_v3PatrolPointPosition;//The position of the patrol point

    public int m_iIndex;//Identifier

    public int m_iNextPatrolIndex;//The next index in the patrol route

    public bool m_bHasBeenLinked;//Has already got a partner in route

    /// <summary>
    /// Finds a random point in a circle radius
    /// </summary>
    /// <param name="a_v3Pos">The center of the circle to search from</param>
    /// <param name="a_fRange">The Max range to find a point in</param>
    public void SetRandomPos(Vector3 a_v3Pos, float a_fRange)
    {
        Vector2 v2RandPosition = Random.insideUnitCircle * a_fRange;
        m_v3PatrolPointPosition = a_v3Pos += new Vector3(v2RandPosition.x, 0, v2RandPosition.y);
    }
}

public class CS_GuardPatrolManager : MonoBehaviour
{
    public List<PatrolPoints> m_lPatrolPointList;//The list of patrol points

    private List<PatrolPoints> m_lOriginalPatrolPoints;//To store the Patrol points in when the guard goes into investigation mode

    private Vector3 m_v3PatrolCenter;//The center point to localise the patrol around

    [SerializeField]
    private int m_iAmountOfPatrolPoints;//The amount of patrol points

    [SerializeField]
    private float m_fPatrolRange;//The range the guard can patrol at

    private NavMeshAgent m_aAgentRef;//Reference to the Navmesh agent

    private bool m_bInvestigationMode = false;//Whether the agent is investigating an area of interest
    private float m_fInvestigationTime = 10.0f;//The amount of time the agent should investigate for
    private float m_fInvestigationTimer = 10.0f;//Timer

    private void Awake()
    {
        m_lPatrolPointList = new List<PatrolPoints>();//Declare patrol list
        PatrolPoints ppPlaceholders = new PatrolPoints();//Declare patrol list

        //Initialise to prevent other scripts accessing this one from crashing before points have been made.
        for (int i = 0; i < m_iAmountOfPatrolPoints; i++)
        {
            m_lPatrolPointList.Add(ppPlaceholders);
        }
        m_aAgentRef = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        ReCalculatePatrol();//Calculate the first route
        m_fInvestigationTimer = m_fInvestigationTime;//Reset Investigation timer
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_bInvestigationMode)//If in Investigation mode
        {
            m_fInvestigationTimer -= Time.deltaTime;//Run down timer
            if (m_fInvestigationTimer <= 0)//Out of time?
            {
                StopInvestigating();
            }
        }
    }

    /// <summary>
    /// Function to find the shortest route to each pathpoint
    /// </summary>
    private void ChooseRoute()
    {
        FindNearestUnusedNeighbor(1);
    }

    /// <summary>
    /// Generates the random positions and checks if they are validly placed on the navmesh
    /// </summary>
    /// <param name="a_iAmountOfPoints">The amount of patrol points</param>
    /// <param name="a_fRange">The range the points can be generated away from the center point</param>
    private void GeneratePositions(int a_iAmountOfPoints, float a_fRange)
    {
        for (int i = 0; i < a_iAmountOfPoints; i++)//Loop through the points
        {
            PatrolPoints ppPatrolPlaceholder = new PatrolPoints();//Temporary placeholder to add later
            bool bCanReachTarget = false;//Can the point actually be reached
            while (bCanReachTarget == false)//Loop until point is valid
            {
                ppPatrolPlaceholder.SetRandomPos(m_v3PatrolCenter, a_fRange);//Get random position

                NavMeshPath nmpPath = new NavMeshPath();//Create new nav path
                m_aAgentRef.CalculatePath(ppPatrolPlaceholder.m_v3PatrolPointPosition, nmpPath);//

                if (nmpPath.status == NavMeshPathStatus.PathPartial || nmpPath.status == NavMeshPathStatus.PathInvalid)//If path is not valid
                {
                    Debug.Log("Bad Point");
                }
                else
                {
                    bCanReachTarget = true;
                }
            }
            ppPatrolPlaceholder.m_iIndex = i;//Set index of point
            m_lPatrolPointList.Add(ppPatrolPlaceholder);//Add it to list
        }
    }

    /// <summary>
    /// Finds the nearest neighboring node of each point and creates a "semi coherent" route between them for patrols
    /// </summary>
    /// <param name="a_iIndex">Not used</param>
    /// <returns></returns>
    private int FindNearestUnusedNeighbor(int a_iIndex)
    {
        bool bTestBool = true;
        int iNextIndex = 0;
        int iInfiniteLoopSafety = 0;
        while (bTestBool)
        {
            PatrolPoints goClosestPoint = null;//Temp storage for closest Point
            float fDistanceToPoint = 0.0f;
            foreach (PatrolPoints Point in m_lPatrolPointList)
            {
                if (Point.m_iIndex == iNextIndex || Point.m_bHasBeenLinked)
                {
                    continue;
                }
                if (goClosestPoint == null)
                {
                    //Set first Point to closest, others will compare to this
                    goClosestPoint = Point;
                    fDistanceToPoint = (Point.m_v3PatrolPointPosition - m_lPatrolPointList[iNextIndex].m_v3PatrolPointPosition).magnitude;
                }
                else
                {
                    //Checks if the current Point is closer than the last one
                    float fdist = (Point.m_v3PatrolPointPosition - m_lPatrolPointList[iNextIndex].m_v3PatrolPointPosition).magnitude;
                    if (fdist < fDistanceToPoint)
                    {
                        //Use the closer Point instead
                        goClosestPoint = Point;
                        fDistanceToPoint = fdist;
                    }
                }
            }

            if (goClosestPoint == null)
            {
                goClosestPoint = m_lPatrolPointList[0];//For the last node, which wont beable to find a partner node, so it is set to the first one to create a loop
            }

            m_lPatrolPointList[iNextIndex].m_iNextPatrolIndex = goClosestPoint.m_iIndex;//Link partner
            m_lPatrolPointList[iNextIndex].m_bHasBeenLinked = true;//Linked
            iNextIndex = goClosestPoint.m_iIndex;//For the next point in route

            if (RouteFinishCheck())//If route has finished
            {
                bTestBool = false;//Exit loop
            }

            //I put this in because I'm an idiot and kept crashing unity.
            iInfiniteLoopSafety++;
            if (iInfiniteLoopSafety > m_iAmountOfPatrolPoints + 10)
            {
                bTestBool = false;
                Debug.Log("InfiniteLoop");
            }
        }
        return 0;
    }

    /// <summary>
    /// Checks if the route between the points has finished
    /// </summary>
    /// <returns></returns>
    private bool RouteFinishCheck()
    {
        foreach (PatrolPoints ppPoint in m_lPatrolPointList)//say ppPoint out loud
        {
            if (ppPoint.m_bHasBeenLinked == false)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Sets the center for which to calculate the patrol points from
    /// </summary>
    /// <param name="a_v3Position">The position to set the center to</param>
    public void SetPatrolCenter(Vector3 a_v3Position)
    {
        m_v3PatrolCenter = a_v3Position;
    }

    /// <summary>
    /// Recalculates the patrol completely, including setting new points
    /// </summary>
    public void ReCalculatePatrol()
    {
        m_bInvestigationMode = false;
        m_v3PatrolCenter = transform.position;
        m_lPatrolPointList.Clear();
        GeneratePositions(m_iAmountOfPatrolPoints, m_fPatrolRange);
        ChooseRoute();
        GetComponent<CS_AIAgent>().m_bInterrupt = true;
    }

    /// <summary>
    /// Disables investigation mode and restores previously used patrol points
    /// </summary>
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

    /// <summary>
    /// Tells the guard to investigate an area
    /// It will save the old patrol points and generate a smaller patrol path around the investigation point
    /// Use StopInvestigating() to halt
    /// </summary>
    /// <param name="a_tPositionToInvestigate">The exact point you want the guard to investigate</param>
    /// <param name="a_iAmountOfPoints">The amount of patrol points you want the guard to look around</param>
    /// <param name="a_fRangeToInvestigate">The range at which he should investigate around the center</param>
    public void InvestigateArea(Transform a_tPositionToInvestigate, int a_iAmountOfPoints, float a_fRangeToInvestigate)
    {
        if (!m_bInvestigationMode)
        {
            m_lOriginalPatrolPoints = new List<PatrolPoints>(m_lPatrolPointList);//Save the old patrol points
        }
        m_bInvestigationMode = true;//Set to investigation mode
        m_fInvestigationTimer = m_fInvestigationTime;//set the investigation timer
        m_v3PatrolCenter = a_tPositionToInvestigate.position;//Set the center
        m_lPatrolPointList.Clear();//Clear the patrol list to start a new one
        GeneratePositions(a_iAmountOfPoints, a_fRangeToInvestigate);//Generate the positions for this new point
        ChooseRoute();//Choose a "coherent" route around the points
        GetComponent<CS_Guard>().ResetPointsForInvestigating();//Reset other variables
        CS_GuardPatrolAction cGuardPatrol = GetComponent<CS_GuardPatrolAction>();//Get the patrol action
        if (cGuardPatrol != null)
        {
            cGuardPatrol.InvestigationMode(true);//Changes the cost of patrolling whilst investigating, to give it priority over other actions
        }
    }

    /// <summary>
    /// Returns the Patrol point with the given index
    /// </summary>
    /// <param name="a_iIndex">Which patrol point to return</param>
    /// <returns>PatrolPoints</returns>
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

        //Draws a red sphere at the center of the investigation point
        if (m_bInvestigationMode)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_v3PatrolCenter, 1);
        }
    }

    /// <summary>
    /// Returns the count of the patrol list
    /// </summary>
    /// <returns>int</returns>
    public int GetAmountOfPoints()
    {
        return m_lPatrolPointList.Count;
    }
}