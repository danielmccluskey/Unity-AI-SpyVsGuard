using System.Collections.Generic;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6018 - Tower Defence
//Script Purpose: This script is an interface to force the use of these variables
//This script was made using the following tutorial: https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793
//////////////////////////////////////////////////////////////////
public interface CS_IGOAP
{
    HashSet<KeyValuePair<string, object>> GetWorldState();

    HashSet<KeyValuePair<string, object>> CreateGoalState();

    void PlanFailed(HashSet<KeyValuePair<string, object>> a_kvpFailedGoal);

    void PlanFound(HashSet<KeyValuePair<string, object>> a_kvpGoal, Queue<CS_GOAPAction> a_qActionQueue);

    void AllActionsFinished();

    void AbortPlan(CS_GOAPAction a_FailedAction);

    bool MoveAgentToAction(CS_GOAPAction a_NextAction);
}