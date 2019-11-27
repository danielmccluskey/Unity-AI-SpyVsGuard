using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6024 - AI
//Repo: https://github.com/danielmccluskey/CT6024-AI
//Script Purpose: Hiding spot identifier
//////////////////////////////////////////////////////////////////
public class CS_HidingComponent : MonoBehaviour
{
    public bool m_bActive = true;

    private float m_fDeactiveTime = 45.0f;
    private float m_fDeactiveTimer;

    // Use this for initialization
    private void Start()
    {
        m_fDeactiveTimer = m_fDeactiveTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!m_bActive)
        {
            m_fDeactiveTimer -= Time.deltaTime;
            if (m_fDeactiveTimer <= 0.0f)
            {
                m_fDeactiveTimer = m_fDeactiveTime;
                m_bActive = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Guard"))
        {
            m_bActive = false;
        }
    }

    public bool GetActive()
    {
        return m_bActive;
    }

    private void OnDrawGizmos()
    {
    }
}