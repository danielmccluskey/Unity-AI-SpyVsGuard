using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6024 - AI
//Repo: https://github.com/danielmccluskey/CT6024-AI
//Script Purpose: Sound Identifier
//////////////////////////////////////////////////////////////////
public class CS_SoundComponent : MonoBehaviour
{
    private bool m_bPlayOnStart = false;

    [SerializeField]
    private float m_fSoundRadius = 0;

    [SerializeField]
    private bool a_bRadio = false;

    [SerializeField]
    private LayerMask m_GuardAlertMask;

    [SerializeField]
    [FMODUnity.EventRef]
    private string m_sSoundToPlay;

    [FMODUnity.EventRef]
    public FMOD.Studio.EventInstance m_fmSoundEventInstance;

    // Use this for initialization
    private void Start()
    {
        m_fSoundRadius = GetComponent<FMODUnity.StudioEventEmitter>().OverrideMaxDistance;//Use the FMOD events distance

        m_fmSoundEventInstance = FMODUnity.RuntimeManager.CreateInstance(m_sSoundToPlay);//Create a sound instance that we can control
        m_fmSoundEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));//Make the sound here
        m_fmSoundEventInstance.start();//Start the sound
        m_fmSoundEventInstance.release();

        Collider[] cTargetsInViewRadius = Physics.OverlapSphere(transform.position, m_fSoundRadius, m_GuardAlertMask);//Get all guards in radius

        foreach (Collider cCurrentTarget in cTargetsInViewRadius)
        {
            CS_GuardHearing cGuardRef = cCurrentTarget.GetComponent<CS_GuardHearing>();
            if (cGuardRef != null)
            {
                if (a_bRadio)
                {
                    cGuardRef.AlertHearRadio(transform);
                }
                else
                {
                    cGuardRef.AlertHearOtherSound(transform);
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// Stops the sound.
    /// </summary>
    public void StopSound()
    {
        m_fmSoundEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Destroy(gameObject, 3.0f);
    }
}