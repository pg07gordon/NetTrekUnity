﻿using UnityEngine;
using System.Collections;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Controls Attack “Beams”
 */

public class Beam : MonoBehaviour
{
    public float m_UVTime; // UV Animation speed on Line Renderer
    public float m_TrackSpeed = 5;
    public float m_NewFireDelay = 0.1f;

    private float m_BeamMaxLenght = 20;
    private float m_AnimateUVTime;
    private float m_NewFireDelayTimer = 0f;

    private LineRenderer[] m_Lines;
    private Vector3 m_Target;
    private Vector3 m_TargetDir;
    private Ray m_Ray;

    private void Start()
    {
        m_Lines = GetComponentsInChildren<LineRenderer>();

        foreach (LineRenderer line in m_Lines)
        {
            line.SetPosition(1, new Vector3(0, 0, m_BeamMaxLenght));
        }  
    }

    public void Init(GameObject emitter, float BeamMaxLenght)
    {
        transform.parent = emitter.transform;

        if (BeamMaxLenght > 1)
        {
            m_BeamMaxLenght = BeamMaxLenght;
        }

        gameObject.SetActive(false);
    }

    public void AttackNew(Vector3 target)
    {
        gameObject.SetActive(true);
        m_Target = target;
        m_NewFireDelayTimer = m_NewFireDelay;
    }

    public void AttackUpdate(Vector3 target)
    {
        gameObject.SetActive(true);
        m_Target = target;
    }

    public void TerminateBeam()
    {
       gameObject.SetActive(false);
    }

    private GameObject HitTest()
    {
        m_Ray = new Ray(transform.position, transform.forward);
        RaycastHit raycastHit;

        //Vector3 endPosition = m_Target + (m_BeamMaxLenght * m_TargetDir);

        if (Physics.Raycast(m_Ray, out raycastHit, m_BeamMaxLenght))
        {
            //endPosition = raycastHit.point;

            return raycastHit.transform.gameObject;
        }

        return null;
    }

    private void Update()
    {

        m_TargetDir = m_Target - transform.position;

        if (m_NewFireDelayTimer <= 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_TargetDir), Time.deltaTime * m_TrackSpeed);
        }
        else
        {
            m_NewFireDelayTimer = m_NewFireDelayTimer - Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(m_TargetDir);
        }

        HitTest();

        m_AnimateUVTime += Time.deltaTime;

        if (m_AnimateUVTime > 1.0f)
            m_AnimateUVTime = 0f;

        foreach (LineRenderer line in m_Lines)
        {
            line.material.SetTextureOffset("_MainTex", new Vector2(m_AnimateUVTime * m_UVTime, 0f));
        }
    }
}