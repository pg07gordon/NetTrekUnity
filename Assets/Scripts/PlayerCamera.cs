using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    
 */

public class PlayerCamera : MonoBehaviour
{
    private GameObject m_PlayerShip;

	private void Start ()
    {
        m_PlayerShip = transform.parent.transform.GetComponentInChildren<Unit>().gameObject;
	}

    public void UpdatePlayerShip(GameObject newShip)
    {
        if (newShip != null)
        {
            m_PlayerShip = newShip;
        }
    }

    private void Update ()
    {
        transform.position = m_PlayerShip.transform.position;
    }
}