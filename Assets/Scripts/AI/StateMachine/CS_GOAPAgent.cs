using System;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6018 - Tower Defence
//Script Purpose: This script controls each AI agent and defines each agents State machines and GOAP planners
//This script was made using the following tutorial: https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793
//////////////////////////////////////////////////////////////////
public class CS_GOAPAgent : MonoBehaviour
{
    //Define the Finite state machine
    private CS_FinateStateMachine FinateStateMachine;

    //Define each of the states for the FSM.
    private CS_FinateStateMachine.CS_IFSMState IdleState;

    private CS_FinateStateMachine.CS_IFSMState MovingState;
    private CS_FinateStateMachine.CS_IFSMState PerformActionState;

    private HashSet<CS_GOAPAction> AvailableActions;
    private Queue<CS_GOAPAction> CurrentlyActiveActions;
    private CS_IGOAP DataProviderInterface;
    private CS_GOAPPlanner ActionPlanner;

    private void Start()
    {
        FinateStateMachine = new CS_FinateStateMachine();
        AvailableActions = new HashSet<CS_GOAPAction>();
        CurrentlyActiveActions = new Queue<CS_GOAPAction>();
        ActionPlanner = new CS_GOAPPlanner();

        FindDataProviderInterface();
        CreateIdleState();
        CreateMovingState();
        CreatePerformActionState();

        FinateStateMachine.PushStateToStack(IdleState);
        LoadAvailableActions();
    }

    private void Update()
    {
        FinateStateMachine.Update(gameObject);
    }

    /// <summary>
    /// Adds the given action to the list of available actions.
    /// </summary>
    /// <param name="a_Action">an action.</param>
    public void AddAvailableAction(CS_GOAPAction a_Action)
    {
        AvailableActions.Add(a_Action);
    }

    /// <summary>
    /// Gets the action from the Available action list.
    /// </summary>
    /// <param name="a_Action">an action.</param>
    /// <returns></returns>
    public CS_GOAPAction GetAction(Type a_Action)
    {
        foreach (CS_GOAPAction CurrentAction in AvailableActions)
        {
            if (CurrentAction.GetType().Equals(a_Action))
            {
                return CurrentAction;
            }
        }
        return null;
    }

    /// <summary>
    /// Removes the action from the list of available actions.
    /// </summary>
    /// <param name="a_Action">an action.</param>
    public void RemoveAvailableAction(CS_GOAPAction a_Action)
    {
        AvailableActions.Remove(a_Action);
    }

    /// <summary>
    /// Determines whether the agent has an action plan].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [has action plan]; otherwise, <c>false</c>.
    /// </returns>
    private bool HasActionPlan()
    {
        if (CurrentlyActiveActions.Count > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Creates the idle state for the FSM where the agent will do nothin at all https://i.imgur.com/3IcRtUP.png.
    /// </summary>
    private void CreateIdleState()
    {
        IdleState = (a_fFinateStateMachine, a_goObject) =>
        {
            HashSet<KeyValuePair<string, object>> kvpWorldState = DataProviderInterface.GetWorldState();
            HashSet<KeyValuePair<string, object>> kvpGoal = DataProviderInterface.CreateGoalState();

            Queue<CS_GOAPAction> ActionPlan = ActionPlanner.CreateActionPlan(gameObject, AvailableActions, kvpWorldState, kvpGoal);
            if (ActionPlan != null)
            {
                CurrentlyActiveActions = ActionPlan;
                DataProviderInterface.PlanFound(kvpGoal, ActionPlan);

                FinateStateMachine.PopStateStack();
                FinateStateMachine.PushStateToStack(PerformActionState);
            }
            else
            {
                DataProviderInterface.PlanFailed(kvpGoal);
                FinateStateMachine.PopStateStack();
                FinateStateMachine.PushStateToStack(IdleState);
            }
        };
    }

    /// <summary>
    /// Creates the state to move the agent to its destination for the FSM
    /// </summary>
    private void CreateMovingState()
    {
        MovingState = (a_fFinateStateMachine, a_goObject) =>
        {
            CS_GOAPAction Action = CurrentlyActiveActions.Peek();
            if (Action.NeedsToBeInRange() && Action.m_goTarget == null)
            {
                FinateStateMachine.PopStateStack();
                FinateStateMachine.PopStateStack();
                FinateStateMachine.PushStateToStack(IdleState);
                return;
            }
            if (DataProviderInterface.MoveAgentToAction(Action))
            {
                FinateStateMachine.PopStateStack();
            }
        };
    }

    /// <summary>
    /// Creates the state to make the agent perform an action
    /// </summary>
    private void CreatePerformActionState()
    {
        PerformActionState = (a_fFinateStateMachine, a_goObject) =>
        {
            if (!HasActionPlan())
            {
                FinateStateMachine.PopStateStack();
                FinateStateMachine.PushStateToStack(IdleState);
                DataProviderInterface.AllActionsFinished();
                return;
            }
            CS_GOAPAction Action = CurrentlyActiveActions.Peek();
            if (Action.IsActionFinished())
            {
                CurrentlyActiveActions.Dequeue();
            }
            if (HasActionPlan())
            {
                Action = CurrentlyActiveActions.Peek();
                bool bIsInRange = Action.NeedsToBeInRange() ? Action.IsAgentInRange() : true;

                if (bIsInRange)
                {
                    bool bActionPerformSuccess = Action.PerformAction(a_goObject);
                    if (!bActionPerformSuccess)
                    {
                        FinateStateMachine.PopStateStack();
                        FinateStateMachine.PushStateToStack(IdleState);
                        CreateIdleState();
                        DataProviderInterface.AbortPlan(Action);
                    }
                }
                else
                {
                    FinateStateMachine.PushStateToStack(MovingState);
                }
            }
            else
            {
                FinateStateMachine.PopStateStack();
                FinateStateMachine.PushStateToStack(IdleState);
                DataProviderInterface.AllActionsFinished();
            }
        };
    }

    /// <summary>
    /// Finds the data provider interface.
    /// </summary>
    private void FindDataProviderInterface()
    {
        foreach (Component cComponent in gameObject.GetComponents(typeof(Component)))
        {
            if (typeof(CS_IGOAP).IsAssignableFrom(cComponent.GetType()))
            {
                DataProviderInterface = (CS_IGOAP)cComponent;
                return;
            }
        }
    }

    /// <summary>
    /// Loads the available actions.
    /// </summary>
    private void LoadAvailableActions()
    {
        CS_GOAPAction[] ActionArray = gameObject.GetComponents<CS_GOAPAction>();
        foreach (CS_GOAPAction Action in ActionArray)
        {
            AvailableActions.Add(Action);
        }
    }

    public CS_IGOAP GetDataProviderInterface()
    {
        return DataProviderInterface;
    }
}