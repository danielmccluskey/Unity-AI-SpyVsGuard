using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        m_fmSoundEventInstance = FMODUnity.RuntimeManager.CreateInstance(m_sSoundToPlay);
        m_fmSoundEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
        m_fmSoundEventInstance.start();
        m_fmSoundEventInstance.release();

        Collider[] cTargetsInViewRadius = Physics.OverlapSphere(transform.position, m_fSoundRadius, m_GuardAlertMask);

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

    public void StopSound()
    {
        m_fmSoundEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Destroy(gameObject, 10.0f);
    }
}