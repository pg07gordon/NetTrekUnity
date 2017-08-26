using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Builds a Grid that moves/rebuilds with the players movement
 */

public class MapGrid : MonoBehaviour
{
    public GameObject m_GridUnit;
    public GameObject m_PlayerShip;
    public int m_GridSqrdSize = 5;
    public float m_GridSpacing = 2;

    public float m_YAxis = -1;

    private float m_GridSize;
    private GameObject[,] m_GridUnits;

    void Start ()
    {
        float unitSize = m_GridUnit.transform.localScale.x;

        m_GridSize = (unitSize + m_GridSpacing) * m_GridSqrdSize;

        float gridCenter = (-m_GridSize - unitSize) * 0.5f;
        float posX = 0;
        float posY = 0;

        m_GridUnits = new GameObject[m_GridSqrdSize, m_GridSqrdSize];
        
        for (uint x = 0; x < m_GridSqrdSize; x++)
        {
            for (uint y = 0; y < m_GridSqrdSize; y++)
            {
                m_GridUnits[x,y] = Instantiate(m_GridUnit);

                posX = (x + 1) * (unitSize + m_GridSpacing) + gridCenter + m_PlayerShip.transform.position.x;
                posY = (y + 1) * (unitSize + m_GridSpacing) + gridCenter + m_PlayerShip.transform.position.y;

                m_GridUnits[x,y].transform.position = new Vector3(posX, m_YAxis, posY);

                m_GridUnits[x, y].transform.parent = transform;
            }
        }
    }

    void Update ()
    {
        float currentPosX = 0;
        float currentPosY = 0;
        float gridSizeHalf = m_GridSize * 0.5f;

        for (uint x = 0; x < m_GridSqrdSize; x++)
        {
            for (uint y = 0; y < m_GridSqrdSize; y++)
            {
                currentPosX = m_GridUnits[x, y].transform.position.x;
                currentPosY = m_GridUnits[x, y].transform.position.z;

                if (m_PlayerShip.transform.position.z > currentPosY + gridSizeHalf)
                {
                    m_GridUnits[x, y].transform.position = new Vector3(currentPosX, m_YAxis, currentPosY + m_GridSize);
                }
                else if (m_PlayerShip.transform.position.z < currentPosY - gridSizeHalf)
                {
                    m_GridUnits[x, y].transform.position = new Vector3(currentPosX, m_YAxis, currentPosY - m_GridSize);
                }
                else if (m_PlayerShip.transform.position.x > currentPosX + gridSizeHalf)
                {
                    m_GridUnits[x, y].transform.position = new Vector3(currentPosX + m_GridSize, m_YAxis, currentPosY);
                }
                else if (m_PlayerShip.transform.position.x < currentPosX - gridSizeHalf)
                {
                    m_GridUnits[x, y].transform.position = new Vector3(currentPosX - m_GridSize, m_YAxis, currentPosY);
                }
            }
        }
    }
}