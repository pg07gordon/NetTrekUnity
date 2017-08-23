using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Game Manager Singleton
 */

public class GameManager : MonoBehaviour
{

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    private static GameManager _instance;

    void Awake()
    {

    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
