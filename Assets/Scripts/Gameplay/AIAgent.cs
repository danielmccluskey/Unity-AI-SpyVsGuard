using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public abstract class AIAgent : MonoBehaviour, IGOAP
{
    public int health;
    public float m_fAgrroDistance = 100.0f;
    public float m_fArrivalDistance = 1.5f;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    public HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("damagePlayer", false)); //to-do: change player's state for world data here
        worldData.Add(new KeyValuePair<string, object>("secureArea", false)); //to-do: change player's state for world data here
        worldData.Add(new KeyValuePair<string, object>("seePlayer", false)); //to-do: change player's state for world data here
        worldData.Add(new KeyValuePair<string, object>("getTotem", false)); //to-do: change player's state for world data here
        worldData.Add(new KeyValuePair<string, object>("avoidGuard", false)); //to-do: change player's state for world data here
        worldData.Add(new KeyValuePair<string, object>("distractGuard", false)); //to-do: change player's state for world data hereescapeArea
        worldData.Add(new KeyValuePair<string, object>("escapeArea", false)); //to-do: change player's state for world data here
        return worldData;
    }

    public abstract HashSet<KeyValuePair<string, object>> createGoalState();

    public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
    }

    public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> action)
    {
    }

    public void actionsFinished()
    {
    }

    public void planAborted(GOAPAction aborter)
    {
    }

    public void setSpeed(float val)
    {
        return;
    }

    public virtual bool moveAgent(GOAPAction nextAction)
    {
        float dist = Vector3.Distance(transform.position, nextAction.target.transform.position);
        if (dist < m_fAgrroDistance)
        {
            GetComponent<NavMeshAgent>().isStopped = false;
            GetComponent<NavMeshAgent>().SetDestination(nextAction.target.transform.position);
            Vector3 v3LookDirection = nextAction.target.transform.position - transform.position;
            v3LookDirection.y = 0;
            Quaternion qRotation = Quaternion.LookRotation(v3LookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, qRotation, 0.005f);
        }

        if (GetComponent<GuardSight>() != null)
        {
            if (GetComponent<GuardSight>().m_bCanSeePlayer == true)//Quick interrupt for if the guard sees an enemy whilst moving.
            {
                nextAction.setInRange(true);

                return true;
            }
        }

        if (dist <= m_fArrivalDistance)
        {
            nextAction.setInRange(true);
            return true;
        }
        else
        {
            return false;
        }
    }
}