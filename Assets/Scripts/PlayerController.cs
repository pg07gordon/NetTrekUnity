using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Player Mouse/Input/Commands and Player Ship Movement Physics
 * TODO:    Move Movement Physics to Unit
 */

public class PlayerController : Unit
{
    private float m_HorizontalInput;
    private float m_VerticalInput;

    public GameObject m_PlayerShip;
    public GameObject m_MouseTarget;
    private Rigidbody m_PlayerShipRb;

    //TODO: Remove
    private int maxGameSubLightSpeed = 20;
    private int maxForwardAcceration = 10;
    private int maxReverseAcceration = 5;
    private int maxForwardSubLightSpeed = 10;
    private int maxReverseSubLightSpeed = 10;
    private int maxRollAngle = 30;
    private int maxRotSpeed = 1;
    private float subLightEnginePower = 1;
    //END

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

    public EnergyWeapon blah; // TODO: Remove

    void Start ()
    {
        m_PlayerShipRb = m_PlayerShip.GetComponent<Rigidbody>();
	}
	
	void Update ()
    {
        MouseControls();
        MovementControls();
        Commands();

        ZBanking();
    }

    // TODO: Refactor
    private void ZBanking()
    {
        float zRotCalc = 0;
        float zRot = m_PlayerShip.transform.eulerAngles.z;
        float xRot = 0;
        float yRot = m_PlayerShip.transform.eulerAngles.y;

        Vector3 rollRot = Vector3.zero;

        float angleZ = m_PlayerShip.transform.localEulerAngles.z;
        angleZ = (angleZ > 180) ? angleZ - 360 : angleZ;

        if (m_HorizontalInput != 0)
        {
            zRotCalc = m_HorizontalInput * m_PlayerShipRb.velocity.magnitude * Time.deltaTime * subLightEnginePower * 2;
            rollRot = new Vector3(xRot, yRot, zRot + zRotCalc);

            if (angleZ > maxRollAngle)
            {
                rollRot = new Vector3(xRot, yRot, maxRollAngle);
            }
            else if (angleZ < -maxRollAngle)
            {
                rollRot = new Vector3(xRot, yRot, 360 - maxRollAngle);
            }
        }
        else
        {
            if (angleZ > 0)
            {
                zRotCalc = Mathf.Lerp(zRot, 0, Time.deltaTime * subLightEnginePower);
            }
            else
            {
                zRotCalc = Mathf.Lerp(zRot, 360, Time.deltaTime * subLightEnginePower);
            }

            rollRot = new Vector3(xRot, yRot, zRotCalc);
        }

        m_PlayerShip.transform.rotation = Quaternion.Euler(rollRot);
    }

    private void MouseControls()
    {
        Vector3 test = new Vector3(m_CursorOnPos.x, 2, m_CursorOnPos.z); 


        if (Input.GetMouseButtonDown(0))
        {
            //m_MouseTarget.transform.position = test;
            //m_CursorStartPos = test;
        }

        if (Input.GetMouseButton(0))
        {
            m_MouseTarget.transform.position = test;

            blah.AttemptWeaponLock(m_MouseTarget); // TODO: Remove

            /*
            if ((test - m_CursorStartPos).sqrMagnitude > 50f) // TODO: Remove Magic numb
            {
                m_CursorStartPos = test;
            }
            else
            {
                
            }
            */
        }

        if (Input.GetMouseButtonUp(0))
        {
            blah.ResetTargettingFlag();
        }
    }

    private void MovementControls()
    {
        m_HorizontalInput = Input.GetAxis("Horizontal");
        m_VerticalInput = Input.GetAxis("Vertical");

        m_PlayerShipRb.drag = subLightEnginePower;
        m_PlayerShipRb.angularDrag = subLightEnginePower;

        if (m_VerticalInput > 0)
        {
            m_PlayerShipRb.AddRelativeForce(maxForwardAcceration * transform.forward * m_VerticalInput);

            if (m_PlayerShipRb.velocity.magnitude > maxForwardAcceration)
            {
                m_PlayerShipRb.velocity = m_PlayerShipRb.velocity.normalized * maxForwardAcceration;
            }
        }
        else
        {
            m_PlayerShipRb.AddRelativeForce(maxReverseAcceration * transform.forward * m_VerticalInput);

            if (m_PlayerShipRb.velocity.magnitude > maxReverseSubLightSpeed)
            {
                m_PlayerShipRb.velocity = m_PlayerShipRb.velocity.normalized * maxReverseSubLightSpeed;
            }
        }

        m_PlayerShipRb.AddRelativeTorque(transform.up * m_HorizontalInput);

        if (m_PlayerShipRb.velocity.magnitude > maxGameSubLightSpeed)
        {
            m_PlayerShipRb.velocity = m_PlayerShipRb.velocity.normalized * maxGameSubLightSpeed;
        }

        if (m_PlayerShipRb.angularVelocity.magnitude > maxRotSpeed)
        {
            m_PlayerShipRb.angularVelocity = m_PlayerShipRb.angularVelocity.normalized * maxRotSpeed;
        }
    }

    private void Commands()
    {

    }
}