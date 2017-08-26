using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Controls Glows
 */

public class ExhaustGlow : MonoBehaviour
{
    public Color m_PrimaryColor;
    public float m_ColorChangeSpeed = 1f;

    private Material m_MyMaterial;
    private Color m_LerpColor;

    /*
    private float _TargetGlowIntensity = 0;
    internal float m_TargetGlowIntensity
    {
        get
        {
            return _TargetGlowIntensity;
        }

        set
        {
            if (_TargetGlowIntensity > 1)
                _TargetGlowIntensity = 1;
            else if (_TargetGlowIntensity < 0)
                _TargetGlowIntensity = 0;
            else
                _TargetGlowIntensity = value;
        }
    }

        private void Update()
        {
                m_TargetGlowIntensity = 2;
                Debug.Log(m_TargetGlowIntensity);
        }

    */

    private float m_TargetGlowIntensity = 0;
    private float m_ActualGlowIntensity = 0;
    private float m_Step;

    internal void SetTargetGlowIntensity(float value)
    {
        if (value > 1)
            m_TargetGlowIntensity = 1;
        else if (value < 0)
            m_TargetGlowIntensity = 0;
        else
            m_TargetGlowIntensity = value;
    }

    private void Start()
    {
        m_MyMaterial = GetComponent<Renderer>().material;
        m_MyMaterial.SetColor("_Color", m_PrimaryColor);
        m_MyMaterial.SetColor("_EmissionColor", m_PrimaryColor * m_ActualGlowIntensity);
    }

    private void Update()
    {
        m_Step = Time.deltaTime * m_ColorChangeSpeed;
        m_LerpColor = Color.Lerp(m_MyMaterial.GetColor("_EmissionColor"), m_PrimaryColor * m_TargetGlowIntensity, m_Step);
        m_MyMaterial.SetColor("_EmissionColor", m_LerpColor);
    }
}
