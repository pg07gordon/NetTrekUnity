using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour
{
    public float m_UVTime; // UV Animation speed
    public float m_BeamMaxLenght = 100;
    public float m_TrackSpeed = 5;
    public float m_NewFireDelay = 0.1f;

    private float m_AnimateUVTime;
    private float m_NewFireDelayTimer = 0f;

    private LineRenderer[] m_Lines;
    private Vector3 m_Target;

    private void Start()
    {
        m_Lines = GetComponentsInChildren<LineRenderer>();

        foreach (LineRenderer line in m_Lines)
        {
            line.SetPosition(1, new Vector3(0, 0, m_BeamMaxLenght));
        }  
    }

    public void Init(GameObject emitter)
    {
        transform.parent = emitter.transform;
        gameObject.SetActive(false);
    }

    public void AttackUpdate(Vector3 target)
    {
        gameObject.SetActive(true);
        m_Target = target;
    }

    public void AttackNew(Vector3 target)
    {
        gameObject.SetActive(true);
        m_Target = target;
        m_NewFireDelayTimer = m_NewFireDelay;
    }

    public void TerminateBeam()
    {
       gameObject.SetActive(false);
    }

    private void Update()
    {
        if (m_NewFireDelayTimer <= 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_Target - transform.position), Time.deltaTime * m_TrackSpeed);
        }
        else
        {
            m_NewFireDelayTimer = m_NewFireDelayTimer - Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(m_Target - transform.position);
        }

        m_AnimateUVTime += Time.deltaTime;

        if (m_AnimateUVTime > 1.0f)
            m_AnimateUVTime = 0f;

        foreach (LineRenderer line in m_Lines)
        {
            line.material.SetTextureOffset("_MainTex", new Vector2(m_AnimateUVTime * m_UVTime, 0f));
        }
    }
}