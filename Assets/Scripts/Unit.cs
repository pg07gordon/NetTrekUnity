using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Base Class for Ships and Stations
 */

public class Unit : MonoBehaviour
{
    [Range(1, 100)]
    public int m_HullStrength = 100;

    void Awake()
    {

    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
        //transform.position = new Vector3(transform.position.x, 0, transform.position.y);
	}
}
