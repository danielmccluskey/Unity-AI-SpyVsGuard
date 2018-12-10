using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Radio : MonoBehaviour
{
    [SerializeField]
    private bool m_bPlaySound = false;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void SetRadioStatus(bool a_bOn)
    {
        if (a_bOn)
        {
            GetComponent<CS_SoundManager>().PlayTrack(0);
        }
        else
        {
            GetComponent<CS_SoundManager>().MuteAllTracks();
        }
        m_bPlaySound = a_bOn;
    }
}