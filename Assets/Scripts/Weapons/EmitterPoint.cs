using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterPoint : MonoBehaviour
{
    private Light m_EmitterLight;

    private void Start()
    {
        m_EmitterLight = GetComponent<Light>();
    }

    public void TurnOffEmitterLight()
    {
        m_EmitterLight.enabled = false;
    }

    public void TurnOnEmitterLight()
    {
        m_EmitterLight.enabled = true;
    }

}