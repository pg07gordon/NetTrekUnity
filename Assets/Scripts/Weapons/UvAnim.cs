using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UvAnim : MonoBehaviour
{
    public float m_UVTime; // UV Animation speed
    private float m_AnimateUVTime;
    private LineRenderer m_LineRenderer;

    private void Start()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    private void Update ()
    {

        m_AnimateUVTime += Time.deltaTime;

        if (m_AnimateUVTime > 1.0f)
            m_AnimateUVTime = 0f;

        m_LineRenderer.material.SetTextureOffset("_MainTex", new Vector2(m_AnimateUVTime * m_UVTime, 0f));

    }
}
