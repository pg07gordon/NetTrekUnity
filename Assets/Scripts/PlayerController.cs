using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Player Mouse/Input/Commands and Player Ship Movement Physics
 * Note:    Very much a work in progress
 */

public class PlayerController : Unit
{
    #region Member Variables

    public GameObject m_MouseTarget;

    public GameObject m_PlayerShip;
    private Unit m_PlayerShipUnit;

    private float m_HorizontalInput;
    private float m_VerticalInput;

    private Vector3 m_CursorStartPos = Vector3.zero;

    private static Vector3 m_CursorWorldPosOnPlayer
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        }
    }

    private static Vector3 m_CameraToCursorDir
    {
        get
        {
            return m_CursorWorldPosOnPlayer - Camera.main.transform.position;
        }
    }

    public Vector3 m_CursorOnPos
    {
        get
        {
            Vector3 camToPlayer = m_PlayerShip.transform.position - Camera.main.transform.position;
            float camToPlayerDot = Vector3.Dot(Camera.main.transform.forward, camToPlayer);
            float camToCursorDot = Vector3.Dot(Camera.main.transform.forward, m_CameraToCursorDir);
            return Camera.main.transform.position + m_CameraToCursorDir * (camToPlayerDot / camToCursorDot);
        }
    }

    /*
    private Vector3 m_CursorToPlayerShipDir
    {
        get
        {
            return m_PlayerShip.transform.position - m_CursorOnPos;
        }
    }

    private float m_PlayerShipAngleToCursor
    {
        get
        {
            return Vector3.SignedAngle(m_CursorToPlayerShipDir, -m_PlayerShip.transform.forward, Vector3.up) * -1;
        }
    }

    private float m_CursorToPlayerDistance
    {
        get
        {
            return Vector3.Distance(m_CursorOnPos, m_PlayerShip.transform.position);
        }
    }
    */

    public EnergyWeapon TestPhaserArray; // TODO: Remove
    public GameObject MainCam; // TODO: Remove

    #endregion

    #region Body

    private void Start ()
    {
        m_PlayerShipUnit = m_PlayerShip.GetComponent<Unit>();
        m_PlayerShipUnit.m_ParentContainer = gameObject;
    }
	
	void Update ()
    {
        MouseControls();
        MovementControls();
        Commands();
    }

    #endregion

    #region Controls

    // TODO: Refactor
    private void MouseControls()
    {
        Vector3 cursorPos = new Vector3(m_CursorOnPos.x, 2, m_CursorOnPos.z); 

        if (Input.GetMouseButton(0))
        {
            m_MouseTarget.transform.position = cursorPos;

            TestPhaserArray.AttemptWeaponLock(m_MouseTarget);
        }

        if (Input.GetMouseButtonUp(0))
        {
            TestPhaserArray.ResetTargettingFlag();
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f )
        {
            Vector3 camPos = MainCam.transform.position;
            camPos.y += -(Input.GetAxis("Mouse ScrollWheel") * 4);
            MainCam.transform.position = camPos;
        }
    }

    private void MovementControls()
    {
        m_VerticalInput = Input.GetAxis("Vertical");
        m_HorizontalInput = Input.GetAxis("Horizontal");

        if (m_VerticalInput > 0)
        {
            m_PlayerShipUnit.AccelerateForward(m_VerticalInput);
        }
        else
        {
            m_PlayerShipUnit.AccelerateReverse(m_VerticalInput);
        }

        m_PlayerShipUnit.YawRotation(m_HorizontalInput);
        m_PlayerShipUnit.ZBanking(m_HorizontalInput);
    }

    private void Commands()
    {

    }

    #endregion
}