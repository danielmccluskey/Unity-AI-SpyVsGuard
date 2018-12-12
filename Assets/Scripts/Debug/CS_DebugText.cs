using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6024 - AI
//Repo: https://github.com/danielmccluskey/CT6024-AI
//Script Purpose: Script to change the debug text
//////////////////////////////////////////////////////////////////
public class CS_DebugText : MonoBehaviour
{
    [SerializeField]
    private bool m_bSpy = false;

    private string m_sCurrentAction = "test";
    private Text m_DebugText;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnGUI()
    {
        if (m_bSpy)
        {
            GUILayout.BeginArea(new Rect(0, 0, 100, 100));

            GUILayout.BeginVertical();
            GUILayout.Label("Spy Debug");
            GUILayout.Label("Current Action: " + m_sCurrentAction);
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        else
        {
            GUILayout.BeginArea(new Rect(110, 0, 100, 100));

            GUILayout.BeginVertical();
            GUILayout.Label("Guard Debug");
            GUILayout.Label("Current Action: " + m_sCurrentAction);
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }

    public void ChangeCurrentActionText(string a_sText)
    {
        m_sCurrentAction = a_sText;
    }
}