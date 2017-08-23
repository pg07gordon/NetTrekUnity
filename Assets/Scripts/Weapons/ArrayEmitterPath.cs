using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Reports when an Array Weapons Emitter Collides on a path
 * Notes:   Should only be one per Array Weapon
 * TODO:    Setup call back
 */

public class ArrayEmitterPath : MonoBehaviour
{
    private BeamArrayWeapon m_BeamArray;
    internal bool m_PrimaryArray = false;

    void Start()
    {
        m_BeamArray = GetComponentInParent<BeamArrayWeapon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_PrimaryArray)
        {
            m_BeamArray.SetEmitterLocation(other.transform.position);
            m_BeamArray.m_EmitterCollision = true;
        }
    }
}