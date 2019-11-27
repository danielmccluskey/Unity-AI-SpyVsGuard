using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CS_GuardSight))]
public class CS_GuardSightEditor : Editor
{
    private void OnSceneGUI()
    {
        CS_GuardSight GuardRef = (CS_GuardSight)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(GuardRef.transform.position, Vector3.up, Vector3.forward, 360, GuardRef.m_fViewRadius);

        Vector3 v3FOVAngleA = GuardRef.DirectionFromAngle(-GuardRef.m_fViewAngle / 2, false);
        Vector3 v3FOVAngleB = GuardRef.DirectionFromAngle(GuardRef.m_fViewAngle / 2, false);

        Handles.DrawLine(GuardRef.transform.position, GuardRef.transform.position + v3FOVAngleA * GuardRef.m_fViewRadius);
        Handles.DrawLine(GuardRef.transform.position, GuardRef.transform.position + v3FOVAngleB * GuardRef.m_fViewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in GuardRef.m_ltVisibleTargets)
        {
            Handles.DrawLine(GuardRef.transform.position, visibleTarget.position);
        }
    }
}