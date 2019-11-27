using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6018 - Tower Defence
//Script Purpose: This script chooses which actions to use, when to use them and chooses a path of actions to reach an ultimate goal
//This script was made using the following tutorial: https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793
//////////////////////////////////////////////////////////////////
public class CS_GOAPPlanner
{
    /// <summary>
    /// Creates the action plan.
    /// </summary>
    /// <param name="a_goAIAgent">The agent (Self ref)</param>
    /// <param name="a_hsAvailableActions">The actions the agent can perform</param>
    /// <param name="a_kvpWorldState">The world state (Current goals in world)</param>
    /// <param name="a_kvpGoal">The goal of this particular agent</param>
    /// <returns></returns>
    public Queue<CS_GOAPAction> CreateActionPlan(GameObject a_goAIAgent, HashSet<CS_GOAPAction> a_hsAvailableActions, HashSet<KeyValuePair<string, object>> a_kvpWorldState, HashSet<KeyValuePair<string, object>> a_kvpGoal)
    {
        //Reset all of the actions this agent can perform
        foreach (CS_GOAPAction Action in a_hsAvailableActions)
        {
            Action.ResetAction();
        }

        //Loops through the available actions and adds the ones that the agent is able to perform to a list.
        HashSet<CS_GOAPAction> hsUsableActions = new HashSet<CS_GOAPAction>();
        foreach (CS_GOAPAction Action in a_hsAvailableActions)
        {
            if (Action.CheckPreCondition(a_goAIAgent))//If the agent can perform this action
            {
                hsUsableActions.Add(Action);
            }
        }

        //Create a "tree" of actions that will lead to the wanted goal.
        List<Node> nLeaves = new List<Node>();
        Node nStartNode = new Node(null, 0, a_kvpWorldState, null);//Beginning node
        bool bSuccessfulTree = BuildGOAPPlan(nStartNode, nLeaves, hsUsableActions, a_kvpGoal);//Build a tree of actions/plan of actions to the goal
        if (!bSuccessfulTree)//null check
        {
            return null;
        }

        //Find the cheapest plan of action out of each generated plan
        Node nCheapestPlan = null;
        foreach (Node nNode in nLeaves)
        {
            if (nCheapestPlan == null)//First node?
            {
                nCheapestPlan = nNode;//This is cheapest for now
            }
            else
            {
                if (nNode.m_fCost < nCheapestPlan.m_fCost)//Is this plan cheaper than the last?
                {
                    nCheapestPlan = nNode;//Set this one instead
                }
            }
        }

        //Create a final plan to actually follow
        List<CS_GOAPAction> FinishedPlan = new List<CS_GOAPAction>();
        Node nCheapestNodeClone = nCheapestPlan;
        while (nCheapestNodeClone != null)
        {
            if (nCheapestNodeClone.m_Action != null)
            {
                FinishedPlan.Insert(0, nCheapestNodeClone.m_Action);
            }
            nCheapestNodeClone = nCheapestNodeClone.m_nParentNode;
        }

        //Create a queue of actions that the agent can run through
        Queue<CS_GOAPAction> ActionQueue = new Queue<CS_GOAPAction>();
        foreach (CS_GOAPAction Action in FinishedPlan)
        {
            ActionQueue.Enqueue(Action);
        }
        return ActionQueue;
    }

    /// <summary>
    /// Builds the Action plan.
    /// </summary>
    /// <param name="a_nParent">The parent node</param>
    /// <param name="a_nNodeList">The list of nodes</param>
    /// <param name="a_UsableActions">The actions available to the agent</param>
    /// <param name="a_kvpGoal">The goal of this agent.</param>
    /// <returns></returns>
    protected bool BuildGOAPPlan(Node a_nParent, List<Node> a_nNodeList, HashSet<CS_GOAPAction> a_UsableActions, HashSet<KeyValuePair<string, object>> a_kvpGoal)
    {
        bool bFoundSuccessfulPath = false;
        foreach (CS_GOAPAction Action in a_UsableActions)
        {
            if (IsInState(Action.GetPreConditions(), a_nParent.m_kvpState))
            {
                HashSet<KeyValuePair<string, object>> kvpCurrentState = PopulateState(a_nParent.m_kvpState, Action.GetEffects());
                Node nNewNode = new Node(a_nParent, a_nParent.m_fCost + Action.m_fCost, kvpCurrentState, Action);

                if (GoalIsInState(a_kvpGoal, kvpCurrentState))
                {
                    a_nNodeList.Add(nNewNode);
                    bFoundSuccessfulPath = true;
                }
                else
                {
                    HashSet<CS_GOAPAction> ChildBranches = ActionBranches(a_UsableActions, Action);
                    bool bFoundSecondPath = BuildGOAPPlan(nNewNode, a_nNodeList, ChildBranches, a_kvpGoal);
                    if (bFoundSecondPath)
                    {
                        bFoundSuccessfulPath = true;
                    }
                }
            }
        }
        return bFoundSuccessfulPath;
    }

    /// <summary>
    /// Builds the branches from each action
    /// </summary>
    /// <param name="a_Actions">Usuable actions</param>
    /// <param name="a_ExcludedAction">The action to exclude from this branch</param>
    /// <returns></returns>
    protected HashSet<CS_GOAPAction> ActionBranches(HashSet<CS_GOAPAction> a_Actions, CS_GOAPAction a_ExcludedAction)
    {
        HashSet<CS_GOAPAction> Branches = new HashSet<CS_GOAPAction>();
        foreach (CS_GOAPAction Action in a_Actions)
        {
            if (!Action.Equals(a_ExcludedAction))
            {
                Branches.Add(Action);
            }
        }
        return Branches;
    }

    /// <summary>
    /// Figures out if the current benefit of the action is the goal of the agent
    /// </summary>
    /// <param name="a_kvpGoal">The goal of the agent.</param>
    /// <param name="a_kvpState">The benefit of the current action.</param>
    /// <returns></returns>
    protected bool GoalIsInState(HashSet<KeyValuePair<string, object>> a_kvpGoal, HashSet<KeyValuePair<string, object>> a_kvpState)
    {
        bool bIsInState = false;
        foreach (KeyValuePair<string, object> kvpGoal in a_kvpGoal)
        {
            foreach (KeyValuePair<string, object> kvpState in a_kvpState)
            {
                if (kvpState.Equals(kvpGoal))
                {
                    bIsInState = true;
                    break;
                }
            }
        }
        return bIsInState;
    }

    /// <summary>
    /// Determines whether the precondition to this action has been met by another action in the tree.
    /// </summary>
    /// <param name="a_kvpPreConditions">the pre conditions.</param>
    /// <param name="a_kvpState">The benefits of the acitons</param>
    /// <returns></returns>
    protected bool IsInState(HashSet<KeyValuePair<string, object>> a_kvpPreConditions, HashSet<KeyValuePair<string, object>> a_kvpState)
    {
        bool bAllPreConditionsMet = true;
        foreach (KeyValuePair<string, object> kvpPreCons in a_kvpPreConditions)
        {
            bool bPreConMet = false;
            foreach (KeyValuePair<string, object> kvpState in a_kvpState)
            {
                if (kvpState.Equals(kvpPreCons))
                {
                    bPreConMet = true;
                    break;
                }
            }
            if (!bPreConMet)
            {
                bAllPreConditionsMet = false;
            }
        }
        return bAllPreConditionsMet;
    }

    /// <summary>
    /// .
    /// </summary>
    /// <param name="a_kvpCurrentState">The current state</param>
    /// <param name="a_kvpStateChange">State change</param>
    /// <returns></returns>
    protected HashSet<KeyValuePair<string, object>> PopulateState(HashSet<KeyValuePair<string, object>> a_kvpCurrentState, HashSet<KeyValuePair<string, object>> a_kvpStateChange)
    {
        HashSet<KeyValuePair<string, object>> kvpNewState = new HashSet<KeyValuePair<string, object>>();

        foreach (KeyValuePair<string, object> kvpState in a_kvpCurrentState)
        {
            kvpNewState.Add(new KeyValuePair<string, object>(kvpState.Key, kvpState.Value));
        }

        foreach (KeyValuePair<string, object> kvpChange in a_kvpStateChange)
        {
            bool bKeyExists = false;

            foreach (KeyValuePair<string, object> kvpState in kvpNewState)
            {
                if (kvpState.Equals(kvpChange.Key))
                {
                    bKeyExists = true;
                    break;
                }
            }

            if (bKeyExists)
            {
                kvpNewState.RemoveWhere((KeyValuePair<string, object> kvp) =>
                {
                    return kvp.Key.Equals(kvpChange.Key);
                }
                );
                KeyValuePair<string, object> kvpUpdatedPair = new KeyValuePair<string, object>(kvpChange.Key, kvpChange.Value);
                kvpNewState.Add(kvpUpdatedPair);
            }
            else
            {
                kvpNewState.Add(new KeyValuePair<string, object>(kvpChange.Key, kvpChange.Value));
            }
        }
        return kvpNewState;
    }

    //Basic node class used for the action trees
    protected class Node
    {
        public Node m_nParentNode;//This nodes parent
        public float m_fCost;//The cost of this action
        public HashSet<KeyValuePair<string, object>> m_kvpState;//Benefits of this action
        public CS_GOAPAction m_Action;//The actual action

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="a_nParentNode">The parent node.</param>
        /// <param name="a_fCost">The cost.</param>
        /// <param name="a_kvpState">State of a KVP.</param>
        /// <param name="a_Action">The action.</param>
        public Node(Node a_nParentNode, float a_fCost, HashSet<KeyValuePair<string, object>> a_kvpState, CS_GOAPAction a_Action)
        {
            m_nParentNode = a_nParentNode;
            m_fCost = a_fCost;
            m_kvpState = a_kvpState;
            m_Action = a_Action;
        }
    }
}