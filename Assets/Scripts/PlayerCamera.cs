using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject playerShip;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.position = playerShip.transform.position;
    }
}
