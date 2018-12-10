using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6018 - Tower Defence
//Script Purpose: Global AI agent class, tells each agent what goals they *could* have and how to do them
//This script was made using the following tutorial: https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793
//////////////////////////////////////////////////////////////////
public abstract class CS_AIAgent : MonoBehaviour, CS_IGOAP
{
    [HideInInspector]
    public int m_iHealth;

    public bool m_bInterrupt = false;
    public bool m_bKnowsTotemLocation = false;

    public float m_fAggroDistance = 1000.0f;//Distance it can "see"
    public float m_fArrivalDistance = 1.5f;//How close the agent has to be to arrive at the target

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        //All of the possible goals in the game
        HashSet<KeyValuePair<string, object>> WorldData = new HashSet<KeyValuePair<string, object>>();
        WorldData.Add(new KeyValuePair<string, object>("damagePlayer", false)); //to-do: change player's state for world data here
        WorldData.Add(new KeyValuePair<string, object>("secureArea", false)); //to-do: change player's state for world data here
        WorldData.Add(new KeyValuePair<string, object>("seePlayer", false)); //to-do: change player's state for world data here
        WorldData.Add(new KeyValuePair<string, object>("getTotem", false)); //to-do: change player's state for world data here
        WorldData.Add(new KeyValuePair<string, object>("avoidGuard", false)); //to-do: change player's state for world data here
        WorldData.Add(new KeyValuePair<string, object>("distractGuard", false)); //to-do: change player's state for world data hereescapeArea
        WorldData.Add(new KeyValuePair<string, object>("escapeArea", false)); //to-do: change player's state for world data here
        WorldData.Add(new KeyValuePair<string, object>("knowsTotemLocation", false)); //to-do: change player's state for world data here
        WorldData.Add(new KeyValuePair<string, object>("foundIntel", false)); //to-do: change player's state for world data here
        WorldData.Add(new KeyValuePair<string, object>("searchArea", false)); //to-do: change player's state for world data here

        return WorldData;
    }

    public abstract HashSet<KeyValuePair<string, object>> CreateGoalState();

    /// <summary>
    /// Handles what happens when the plan fails (Unfinished)
    /// </summary>
    /// <param name="a_kvpFailedGoal"></param>
    public void PlanFailed(HashSet<KeyValuePair<string, object>> a_kvpFailedGoal)
    {
    }

    /// <summary>
    /// Handles what happens when the agent finds a play (Unfinished)
    /// </summary>
    /// <param name="a_kvpGoal"></param>
    /// <param name="a_qActionQueue"></param>
    public void PlanFound(HashSet<KeyValuePair<string, object>> a_kvpGoal, Queue<CS_GOAPAction> a_qActionQueue)
    {
    }

    public void AllActionsFinished()
    {
    }

    public void AbortPlan(CS_GOAPAction a_FailedAction)
    {
        GetComponent<CS_GOAPAgent>().GetDataProviderInterface().AllActionsFinished();
        a_FailedAction.ResetGA();
        a_FailedAction.ResetAction();
    }

    /// <summary>
    /// Function for the move state of the FSM, tells the agent how to move to its target
    /// </summary>
    /// <param name="a_NextAction">Action to do after the agent arrives at the target</param>
    /// <returns></returns>
    public bool MoveAgentToAction(CS_GOAPAction a_NextAction)
    {
        GetComponent<CS_DebugText>().ChangeCurrentActionText(a_NextAction.m_sActionName);

        float fDistance = Vector3.Distance(transform.position, a_NextAction.m_goTarget.transform.position);//Get distance to target
        if (fDistance < m_fAggroDistance)//If it is in aggro range
        {
            GetComponent<NavMeshAgent>().isStopped = false;
            GetComponent<NavMeshAgent>().SetDestination(a_NextAction.m_goTarget.transform.position);//Let the nav mesh do the work
            Vector3 v3LookDirection = a_NextAction.m_goTarget.transform.position - transform.position;//Look at the target
            v3LookDirection.y = 0;
            Quaternion qRotation = Quaternion.LookRotation(v3LookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, qRotation, 0.005f);
        }

        if (m_bInterrupt)
        {
            GetComponent<CS_GOAPAgent>().GetDataProviderInterface().AbortPlan(a_NextAction);

            AbortPlan(a_NextAction);
            m_bInterrupt = false;

            return true;
        }

        if (fDistance <= m_fArrivalDistance)//If I have arrived
        {
            a_NextAction.SetInRange(true);
            return true;
        }
        else
        {
            return false;
        }
    }
}