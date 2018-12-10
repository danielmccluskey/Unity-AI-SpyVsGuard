using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            GUILayout.Box("Spy Debug");
            GUILayout.Box("Current Action: " + m_sCurrentAction);
        }
        else
        {
            GUILayout.Box("");
            GUILayout.Box("");
            GUILayout.Box("Guard Debug");
            GUILayout.Box("Current Action: " + m_sCurrentAction);
        }
    }

    public void ChangeCurrentActionText(string a_sText)
    {
        m_sCurrentAction = a_sText;
    }
}