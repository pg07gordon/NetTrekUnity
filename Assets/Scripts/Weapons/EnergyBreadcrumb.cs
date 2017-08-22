using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBreadcrumb : MonoBehaviour
{
    public uint m_NumbOfCrumbs = 5;
    public Vector3 m_Size;

    internal uint m_CurrentCrumb = 0;
    internal uint m_NextCrumb;

    internal GameObject[] m_Crumbs;
    
    internal Transform m_InactiveParent;

    private float m_AlphaTimer;
    private float m_AlphaDuration = 0.75f;
    private float m_NextSpawnDuration = 0.03f;
    private float m_NextSpawnTimer;

    private float m_Alpha;
    //private Material m_Material;
    //private Color m_Color;
    
	void Start ()
    {
        //m_Material = GetComponent<Renderer>().material;
        //m_Color = m_Material.GetColor("_TintColor");
        //m_Material.SetColor("_TintColor", m_Color);

        Init();

        if (m_CurrentCrumb == 0)
        {
            m_Crumbs = new GameObject[m_NumbOfCrumbs];
            m_Crumbs[0] = gameObject;

            for (uint i = 1; i < m_NumbOfCrumbs; i++)
            {
                m_Crumbs[i] = Instantiate(gameObject, transform.position, transform.rotation);
                

                var eb = m_Crumbs[i].GetComponent<EnergyBreadcrumb>();
                eb.m_Crumbs = m_Crumbs;
                eb.m_InactiveParent = transform.parent;
                eb.m_CurrentCrumb = i;
                eb.m_NextCrumb = i + 1;
            }
        }
    }

    void Init()
    {
        m_AlphaTimer = m_AlphaDuration;
        

        if (m_CurrentCrumb == 0)
        {
            m_NextCrumb = 1;
        }
        else
        {
            transform.parent = m_InactiveParent;
            transform.position = m_Crumbs[0].transform.position;
            gameObject.SetActive(false);
        }

        transform.localScale = m_Size;
    }
	
	void Update ()
    {
        m_Alpha = Mathf.Lerp(1f, 0f, m_AlphaTimer * m_AlphaDuration);
        m_AlphaTimer += Time.deltaTime;

        //Color color = new Color(m_Color.r, m_Color.g, m_Color.b, m_Alpha);
        //m_Material.SetColor("_TintColor", color);

        m_NextSpawnTimer = m_NextSpawnTimer - Time.deltaTime;

        if (m_CurrentCrumb == 0 && m_NextSpawnTimer < 0 && m_NumbOfCrumbs > m_NextCrumb)
        {
            m_NextSpawnTimer = m_NextSpawnDuration;

            m_Crumbs[m_NextCrumb].transform.parent = transform.parent.transform.parent;
            m_Crumbs[m_NextCrumb].SetActive(true);
            m_NextCrumb++;
        }
        else if (m_CurrentCrumb == 0 && m_NextSpawnTimer < 0 && m_NumbOfCrumbs == m_NextCrumb)
        {
            Init();
        }
        else if (m_Alpha <= 0 && m_CurrentCrumb != 0)
        {
            Init();
        }
    }
}
