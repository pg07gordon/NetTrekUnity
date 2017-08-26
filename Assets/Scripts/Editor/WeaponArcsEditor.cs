using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Shows Weapon Angle Arcs in the Editor
 */

[CustomEditor(typeof(Weapon), true)]
public class WeaponArcsEditor : Editor
{
    private Color m_ArcColor = Color.cyan;
    private float m_ArcInsideAlpha = 0.1f;
    private float m_ArcEdgeAlpha = 1f;
    private float m_ArcSize = 6;

    private float m_ArcAngles;
    private Weapon m_Weapon;
    private Transform m_Trans;
    private Vector3 m_Rot;

    void OnEnable()
    {
        if (target != null)
        {
            m_Weapon = target as Weapon;
            m_Trans = m_Weapon.transform;
        }
    }

    void OnSceneGUI()
    {
        if (m_Weapon != null && !Application.isPlaying)
        {
            m_Rot = Quaternion.AngleAxis(m_Weapon.GetAngleOfAttack().x, m_Trans.up) * m_Trans.forward;
            m_ArcAngles = m_Weapon.GetAngleOfAttack().y - m_Weapon.GetAngleOfAttack().x;

            //Arc Inside
            m_ArcColor.a = m_ArcInsideAlpha;
            Handles.color = m_ArcColor;
            Handles.DrawSolidArc(m_Trans.position, m_Trans.up, m_Rot, m_ArcAngles, m_ArcSize);

            //Arc Edge
            m_ArcColor.a = m_ArcEdgeAlpha;
            Handles.color = m_ArcColor;
            Handles.DrawWireArc(m_Trans.position, m_Trans.up, m_Rot, m_ArcAngles, m_ArcSize);
        }
    }
}
