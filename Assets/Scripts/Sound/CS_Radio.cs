using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6024 - AI
//Repo: https://github.com/danielmccluskey/CT6024-AI
//Script Purpose: Simple script to start and stop radio
//////////////////////////////////////////////////////////////////
public class CS_Radio : MonoBehaviour
{
    [SerializeField]
    private bool m_bPlaySound = false;

    [SerializeField]
    private GameObject m_goSoundPrefab;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_bPlaySound)
        {
            m_bPlaySound = false;
            Instantiate(m_goSoundPrefab, transform);
        }
    }

    public void SetRadioStatus(bool a_bOn)
    {
        m_bPlaySound = a_bOn;
    }
}