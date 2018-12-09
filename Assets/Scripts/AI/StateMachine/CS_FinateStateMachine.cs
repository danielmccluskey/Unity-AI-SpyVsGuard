using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6018 - Tower Defence
//Script Purpose: This script defines how the Finite state machine works
//This script was made using the following tutorial: https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793
// I also misspelled it :/
//////////////////////////////////////////////////////////////////
public class CS_FinateStateMachine
{
    private Stack<CS_IFSMState> m_sStatesStack = new Stack<CS_IFSMState>();//Stack to hold the states in.

    public delegate void CS_IFSMState(CS_FinateStateMachine a_fFinateStateMachine, GameObject a_goObject);

    public void Update(GameObject a_goObject)
    {
        if (m_sStatesStack.Peek() != null)
        {
            m_sStatesStack.Peek().Invoke(this, a_goObject);
        }
    }

    /// <summary>
    /// Pushes the state to stack.
    /// </summary>
    /// <param name="a_fsmState">State of the FSM.</param>
    public void PushStateToStack(CS_IFSMState a_fsmState)
    {
        m_sStatesStack.Push(a_fsmState);
    }

    /// <summary>
    /// Pops the state stack.
    /// </summary>
    public void PopStateStack()
    {
        m_sStatesStack.Pop();
    }
}