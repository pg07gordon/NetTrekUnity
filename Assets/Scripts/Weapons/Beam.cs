using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour
{
    public float m_UVTime; // UV Animation speed
    public float m_BeamMaxLenght = 100;

    private float m_AnimateUVTime;
    private LineRenderer[] m_Lines;

    private GameObject m_Target;
    private Vector3 m_TargetPath;

    public GameObject ship;


    private Quaternion forwardRotation;

    private void Start()
    {
        m_Lines = GetComponentsInChildren<LineRenderer>();
        m_Target = gameObject;

        LastTarget = gameObject.transform.position;
        NextTarget = gameObject.transform.position;
    }

    public void Init(GameObject emitter)
    {
        transform.parent = emitter.transform;
        //gameObject.SetActive(false);
    }

    public void Shoot(GameObject target)
    {
        //gameObject.SetActive(true);
        m_Target = target;

        step = 0;

        if (Switch)
        {
            Switch = !Switch;
            LastTarget = target.transform.position;
        }
        else
        {
            Switch = !Switch;
            NextTarget = target.transform.position;
        }

    }

    public void TerminateBeam()
    {
       //gameObject.SetActive(false);
    }

    private Vector3 LastTarget;
    private Vector3 NextTarget;

    private float step = 0f;
    bool Switch = true;


    private void Update()
    {
        m_AnimateUVTime += Time.deltaTime;

        if (m_AnimateUVTime > 1.0f)
            m_AnimateUVTime = 0f;

        step += Time.deltaTime;

        Vector3 smooth = Vector3.Lerp(LastTarget, NextTarget, step);

        //m_TargetPath = (transform.InverseTransformPoint(m_Target.transform.position) - transform.localPosition).normalized * m_BeamMaxLenght;
        m_TargetPath = (transform.InverseTransformPoint(smooth) - transform.localPosition).normalized * m_BeamMaxLenght;

        foreach (LineRenderer line in m_Lines)
        {
            line.SetPosition(0, transform.localPosition);

            line.SetPosition(1, m_TargetPath);

            line.material.SetTextureOffset("_MainTex", new Vector2(m_AnimateUVTime * m_UVTime, 0f));
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(LastTarget, new Vector3(1, 1, 1));

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(NextTarget, new Vector3(1, 1, 1));

        Vector3 x = Vector3.Lerp(LastTarget, NextTarget, step);

        Gizmos.color = Color.green;
        Gizmos.DrawCube(x, new Vector3(1, 1, 1));
    }
}