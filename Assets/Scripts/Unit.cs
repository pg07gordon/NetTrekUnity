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

    internal GameObject m_ParentContainer;

    [SerializeField]
    private float m_MaxRollAngle = 30;

    private Rigidbody m_UnitRB;

    private float m_SubLightEnginePower;
    //private float m_SubLightEngineReverseRatio;

    private float m_MaxForwardAcceration;
    private float m_MaxReverseAcceration;
    //private float m_MaxForwardSubLightSpeed;
    //private float m_MaxReverseSubLightSpeed;

    private float m_MaxRotSpeed;

    private void Start()
    {
        m_UnitRB = GetComponent<Rigidbody>();

        // TODO: Needs to be all Updated
        m_SubLightEnginePower = 1;
        //m_SubLightEngineReverseRatio = 0.5f;
        m_MaxForwardAcceration = 10f;
        m_MaxReverseAcceration = 5f;
        m_MaxRotSpeed = 10f;

        //m_MaxForwardSubLightSpeed = 10f;
        //m_MaxReverseSubLightSpeed = m_MaxForwardSubLightSpeed * m_SubLightEngineReverseRatio;

        m_UnitRB.drag = m_SubLightEnginePower;
        m_UnitRB.angularDrag = m_SubLightEnginePower;
    }

    private void Update()
    {
        if (m_UnitRB.velocity.magnitude > GameManager.instance.m_maxGameSubLightSpeed)
        {
            m_UnitRB.velocity = m_UnitRB.velocity.normalized * GameManager.instance.m_maxGameSubLightSpeed;
        }

        if (m_UnitRB.angularVelocity.magnitude > m_MaxRotSpeed)
        {
            m_UnitRB.angularVelocity = m_UnitRB.angularVelocity.normalized * m_MaxRotSpeed;
        }
    }


    #region Movement

    public void AccelerateForward(float power)
    {
        Accelerate(power, m_MaxForwardAcceration);
    }

    public void AccelerateReverse(float power)
    {
        Accelerate(power, m_MaxReverseAcceration);
    }

    private void Accelerate(float power, float maxAcceration)
    {
        m_UnitRB.AddRelativeForce(maxAcceration * m_ParentContainer.transform.forward * power);

        if (m_UnitRB.velocity.magnitude > maxAcceration)
        {
            m_UnitRB.velocity = m_UnitRB.velocity.normalized * maxAcceration;
        }
    }

    public void YawRotation(float power)
    {
        m_UnitRB.AddRelativeTorque(m_ParentContainer.transform.up * power);
    }

    // TODO: Refactor
    public void ZBanking(float power)
    {
        float zRotCalc = 0;
        float zRot = transform.eulerAngles.z;
        float xRot = 0;
        float yRot = transform.eulerAngles.y;

        Vector3 rollRot = Vector3.zero;

        float angleZ = transform.localEulerAngles.z;
        angleZ = (angleZ > 180) ? angleZ - 360 : angleZ;

        if (power != 0)
        {
            zRotCalc = power * m_UnitRB.velocity.magnitude * Time.deltaTime * m_SubLightEnginePower * 2;
            rollRot = new Vector3(xRot, yRot, zRot + zRotCalc);

            if (angleZ > m_MaxRollAngle)
            {
                rollRot = new Vector3(xRot, yRot, m_MaxRollAngle);
            }
            else if (angleZ < -m_MaxRollAngle)
            {
                rollRot = new Vector3(xRot, yRot, 360 - m_MaxRollAngle);
            }
        }
        else
        {
            if (angleZ > 0)
            {
                zRotCalc = Mathf.Lerp(zRot, 0, Time.deltaTime * m_SubLightEnginePower);
            }
            else
            {
                zRotCalc = Mathf.Lerp(zRot, 360, Time.deltaTime * m_SubLightEnginePower);
            }

            rollRot = new Vector3(xRot, yRot, zRotCalc);
        }

        transform.rotation = Quaternion.Euler(rollRot);
    }

    #endregion

}
