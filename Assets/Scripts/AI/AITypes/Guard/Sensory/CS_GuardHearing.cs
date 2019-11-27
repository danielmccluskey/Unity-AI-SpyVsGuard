using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6024 - AI
//Repo: https://github.com/danielmccluskey/CT6024-AI
//Script Purpose: Allows the guard to hear sounds
//////////////////////////////////////////////////////////////////
public class CS_GuardHearing : MonoBehaviour
{
    private int m_iInvestigationEffort = 5;
    private float m_fInvestigationRange = 5;

    private bool m_bCanHearRadio = false;

    private Transform m_tSoundLocation;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void AlertHearRadio(Transform a_tSoundLocation)
    {
        GetComponent<CS_AIAgent>().m_bInterrupt = true;
        //GetComponent<CS_GuardPatrolManager>().InvestigateArea(a_tSoundLocation, m_iInvestigationEffort, m_fInvestigationRange);
        GetComponent<CS_Guard>().MoveTarget(a_tSoundLocation.position);

        m_tSoundLocation = a_tSoundLocation;
        m_bCanHearRadio = true;
    }

    public void AlertHearOtherSound(Transform a_tSoundLocation)
    {
        GetComponent<CS_AIAgent>().m_bInterrupt = true;
        GetComponent<CS_Guard>().MoveTarget(a_tSoundLocation.position);
        m_tSoundLocation = GetComponent<CS_Guard>().GetSpyTarget().transform;
        GetComponent<CS_GuardPatrolManager>().InvestigateArea(a_tSoundLocation, m_iInvestigationEffort, m_fInvestigationRange);
    }

    public bool GetCanHearRadio()
    {
        return m_bCanHearRadio;
    }

    public Transform GetSoundLocation()
    {
        return m_tSoundLocation;
    }

    public void TurnedRadioOff()
    {
        m_bCanHearRadio = false;
        //Destroy(m_tSoundLocation, 3.0f);
    }
}