using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_Spy : CS_AIAgent
{
    public bool m_bSeesGuard = false;

    // Start is called before the first frame update
    private void Start()
    {
    }

    public override HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("escapeArea", true));
        goal.Add(new KeyValuePair<string, object>("avoidGuard", true));

        return goal;
    }
}