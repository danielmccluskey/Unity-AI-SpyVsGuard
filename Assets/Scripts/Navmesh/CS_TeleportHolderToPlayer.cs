using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TeleportHolderToPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject m_goGameObjectToSpawn;

    private Transform m_tObjectToTeleportToMe;

    [SerializeField]
    private Vector3 m_v3Offset;

    [SerializeField]
    private bool m_bRotate = false;

    // Use this for initialization
    private void Start()
    {
        m_tObjectToTeleportToMe = Instantiate(m_goGameObjectToSpawn).transform;
    }

    // Update is called once per frame
    private void Update()
    {
        m_tObjectToTeleportToMe.position = transform.position;
        m_tObjectToTeleportToMe.Translate(m_v3Offset);

        m_tObjectToTeleportToMe.rotation = transform.rotation;
        m_tObjectToTeleportToMe.Rotate(Vector3.up, -90.0f);
    }
}