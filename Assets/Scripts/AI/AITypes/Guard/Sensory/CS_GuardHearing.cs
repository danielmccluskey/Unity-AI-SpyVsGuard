using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void AlertHearSound(Transform a_tSoundLocation)
    {
        GetComponent<CS_AIAgent>().m_bInterrupt = true;
        //GetComponent<CS_GuardPatrolManager>().InvestigateArea(a_tSoundLocation, m_iInvestigationEffort, m_fInvestigationRange);
        m_tSoundLocation = a_tSoundLocation;
        m_bCanHearRadio = true;
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
    }
}