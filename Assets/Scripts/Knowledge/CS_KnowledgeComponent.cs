using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6024 - AI
//Repo: https://github.com/danielmccluskey/CT6024-AI
//Script Purpose: Knowledge identifier
//////////////////////////////////////////////////////////////////
public class CS_KnowledgeComponent : MonoBehaviour
{
    [SerializeField]
    private bool m_bPlayerInRange = false;

    [SerializeField]
    private bool m_bFound = false;

    [SerializeField]
    private bool m_bCollected = false;

    private void FixedUpdate()
    {
    }

    public bool HasBeenCollected()
    {
        return m_bCollected;
    }

    public bool HasBeenLocated()
    {
        return m_bFound;
    }

    public void SetFound(bool a_bFound)
    {
        m_bFound = a_bFound;
    }

    public void SetCollected(bool a_bCollected)
    {
        m_bCollected = a_bCollected;
        m_bFound = a_bCollected;
    }
}