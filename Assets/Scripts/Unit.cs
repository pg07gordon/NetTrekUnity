using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Range(1, 100)]
    private int hullHealth = 100;

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
