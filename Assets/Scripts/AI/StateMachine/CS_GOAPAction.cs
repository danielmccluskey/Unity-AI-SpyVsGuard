using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6018 - Tower Defence
//Script Purpose: This script defines the variables that a GOAP action should inherit
//This script was made using the following tutorial: https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793
//////////////////////////////////////////////////////////////////
public abstract class CS_GOAPAction : MonoBehaviour
{
    private HashSet<KeyValuePair<string, object>> m_kvpPreconditions;//The list of preconditions that this action requires to function
    private HashSet<KeyValuePair<string, object>> m_kvpEffects;//The effects this action has when successfuly performed

    [SerializeField]
    private bool m_bInRange = false;//Is the agent in range to perform this action?

    [SerializeField]
    public float m_fCost = 1.0f;//The m_fCost of using this action

    [SerializeField]
    public GameObject m_goTarget;//The target to perform this action on

    public string m_sActionName;

    /// <summary>
    /// Initializes a new instance of the <see cref="CS_GOAPAction"/> class.
    /// </summary>
    public CS_GOAPAction()
    {
        m_kvpPreconditions = new HashSet<KeyValuePair<string, object>>();
        m_kvpEffects = new HashSet<KeyValuePair<string, object>>();
    }

    /// <summary>
    /// Resets the action.
    /// </summary>
    public void ResetAction()
    {
        m_bInRange = false;
        m_goTarget = null;
        ResetGA();
    }

    public abstract void ResetGA();

    public abstract bool IsActionFinished();

    public abstract bool CheckPreCondition(GameObject a_goAIAgent);

    public abstract bool PerformAction(GameObject a_goAIAgent);

    public abstract bool NeedsToBeInRange();

    /// <summary>
    /// Determines whether [is agent in range].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is agent in range]; otherwise, <c>false</c>.
    /// </returns>
    public bool IsAgentInRange()
    {
        return m_bInRange;
    }

    /// <summary>
    /// Sets the in range variable.
    /// </summary>
    /// <param name="a_bTrue">if set to <c>true</c> [a b true].</param>
    public void SetInRange(bool a_bTrue)
    {
        m_bInRange = true;
    }

    /// <summary>
    /// Adds the given pre condition.
    /// </summary>
    /// <param name="a_sPreCondition">the pre condition.</param>
    /// <param name="a_bValue">True or false.</param>
    public void AddPreCondition(string a_sPreCondition, object a_bValue)
    {
        m_kvpPreconditions.Add(new KeyValuePair<string, object>(a_sPreCondition, a_bValue));
    }

    /// <summary>
    /// Removes the given pre condition.
    /// </summary>
    /// <param name="a_sPreCondition">The pre condition.</param>
    public void RemovePreCondition(string a_sPreCondition)
    {
        KeyValuePair<string, object> kvpPreConditionToRemove = new KeyValuePair<string, object>();
        foreach (KeyValuePair<string, object> kvpPreCondition in m_kvpPreconditions)
        {
            if (kvpPreCondition.Key.Equals(a_sPreCondition))
            {
                kvpPreConditionToRemove = kvpPreCondition;
            }
            if (!default(KeyValuePair<string, object>).Equals(kvpPreConditionToRemove))
            {
                m_kvpPreconditions.Remove(kvpPreConditionToRemove);
            }
        }
    }

    /// <summary>
    /// Adds the given effect.
    /// </summary>
    /// <param name="a_sEffect">The effect.</param>
    /// <param name="a_bValue">True or false</param>
    public void AddEffect(string a_sEffect, object a_bValue)
    {
        m_kvpEffects.Add(new KeyValuePair<string, object>(a_sEffect, a_bValue));
    }

    /// <summary>
    /// Removes the given effect.
    /// </summary>
    /// <param name="a_sEffect">The effect.</param>
    public void RemoveEffect(string a_sEffect)
    {
        KeyValuePair<string, object> kvpEffectToRemove = new KeyValuePair<string, object>();
        foreach (KeyValuePair<string, object> kvpEffect in m_kvpEffects)
        {
            if (kvpEffect.Key.Equals(a_sEffect))
            {
                kvpEffectToRemove = kvpEffect;
            }
            if (!default(KeyValuePair<string, object>).Equals(kvpEffectToRemove))
            {
                m_kvpEffects.Remove(kvpEffectToRemove);
            }
        }
    }

    /// <summary>
    /// Gets the pre conditions.
    /// </summary>
    /// <returns></returns>
    public HashSet<KeyValuePair<string, object>> GetPreConditions()
    {
        return m_kvpPreconditions;
    }

    /// <summary>
    /// Gets the effects.
    /// </summary>
    /// <returns></returns>
    public HashSet<KeyValuePair<string, object>> GetEffects()
    {
        return m_kvpEffects;
    }
}