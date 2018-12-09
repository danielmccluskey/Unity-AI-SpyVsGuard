using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6018 - Tower Defence
//Script Purpose: This script implements an interface to force scripts to use certain functions
//This script was made using the following tutorial: https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793
//////////////////////////////////////////////////////////////////
public interface CS_IFSMState
{
    void Update(CS_IFSMState a_fsmState, GameObject a_goObject);
}