using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayEmitterPath : MonoBehaviour
{
    private BeamArrayWeapon m_BeamArray;
    internal bool primaryArray = false;

    void Start()
    {
        m_BeamArray = GetComponentInParent<BeamArrayWeapon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (primaryArray)
        {
            m_BeamArray.SetEmitterLocation(other.transform.position);
            m_BeamArray.m_EmitterCollision = true;
        }
    }
}