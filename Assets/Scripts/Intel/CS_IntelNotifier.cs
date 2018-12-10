using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_IntelNotifier : MonoBehaviour {

    [SerializeField]
    private bool m_bPlayerInRange = false;
    [SerializeField]

    private bool m_bIntelFound = false;
    [SerializeField]

    private bool m_bIntelCollected = false;



    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_bPlayerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_bPlayerInRange = false;

        }
    }

    public bool HasBeenCollected()
    {
        return m_bIntelCollected;
    }
    public bool HasBeenLocated()
    {
        return m_bIntelFound;
    }
    public void SetCollected(bool a_bCollected)
    {
        m_bIntelCollected = a_bCollected;
    }
}
