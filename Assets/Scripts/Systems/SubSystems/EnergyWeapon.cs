using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Base class for Energy Weapons
 */

public class EnergyWeapon : SubSystems
{
    #region Member Variables

    public float m_MaxAngleOfAttackFrom = 0;
    public float m_MaxAngleOfAttackTo = 90;

    public float m_OverAngleReFire = 20; 

    public float m_MaxAttackDistance = 50;
    public float m_MinAttackDistance = 5;

    public float m_EnergyConsumptionPerSec = 20;
    public float m_MinEnergyLevelToFire = 40;

    public float m_MinBeamBurstTime = 0.25f;
    protected float m_MinBeamBurstTimer = 0;

    public float m_BeamDelayAfterStopTargeting = 0.1f;

    protected bool m_WeaponDischarging = false;
    protected bool m_WeaponCharging = false;

    protected bool m_UserActivelyTargettingFlag = false;

    protected GameObject m_Target;

    protected float m_EnergyRechargePerSec
    {
        get
        {
            return (m_EnergyConsumptionPerSec * 0.5f) / m_CurrentAvailableEnergy * 100;
        }
    }

    protected Vector3 m_TargetDir
    {
        get
        {
            return m_Ship.transform.position - m_Target.transform.position;
        }
    }

    protected float m_AngleToTarget
    {
        get
        {
            return Vector3.SignedAngle(m_TargetDir, -m_Ship.transform.forward, Vector3.up) * -1;
        }
    }

    protected float m_DistanceToTarget
    {
        get
        {
            return Vector3.Distance(m_Target.transform.position, m_Ship.transform.position);
        }
    }

    private bool m_CanLockWeapons
    {
        get
        {
            if (m_MaxAttackDistance >= m_DistanceToTarget
                && m_MinAttackDistance <= m_DistanceToTarget
                && m_MaxAngleOfAttackFrom < m_AngleToTarget
                && m_MaxAngleOfAttackTo > m_AngleToTarget)
                return true;
            else
                return false;
        }
    }

    private Unit m_Ship;
    //private Ray m_Ray;

    private float _EnergyLevels;
    private float m_EnergyLevels
    {
        get
        {
            return _EnergyLevels;
        }

        set
        {
            if (_EnergyLevels > m_CurrentAvailableEnergy)
                _EnergyLevels = m_CurrentAvailableEnergy;
            else if (m_EnergyLevels < 0)
                _EnergyLevels = 0;
            else
                _EnergyLevels = value;
        }
    }

    #endregion

    #region Body

    protected override void Start()
    {
        base.Start();

        m_EnergyLevels = m_CurrentAvailableEnergy;

        m_Ship = GetComponentInParent<Unit>();
        StateDefaults();
    }

    protected override void StateDefaults()
    {
        base.StateDefaults();
        StartCoroutine(this.EnergyLevels());

        if (m_WeaponCharging || m_WeaponDischarging || m_MinBeamBurstTimer > 0)
        {
            StartCoroutine(this.CheckTargetLock());
        }
    }

    public virtual void AttemptWeaponLock(GameObject target)
    {
        m_Target = target;
        m_UserActivelyTargettingFlag = true;

        /*
        m_Ray = new Ray(m_Target.transform.position, m_TargetDir);
        RaycastHit raycastHit;

        Vector3 endPosition = m_Target.transform.position + (m_MaxAttackDistance * m_TargetDir);

        if (Physics.Raycast(m_Ray, out raycastHit, m_MaxAttackDistance))
        {
            endPosition = raycastHit.point;
        }
        */

        if (m_CanLockWeapons && !m_WeaponCharging && !m_WeaponDischarging && m_EnergyLevels > m_MinEnergyLevelToFire)
        {
            StartNewFiringSequence();
        }
        else if (m_CanLockWeapons && m_WeaponDischarging)
        {
            FireUpdate();
        }
    }

    public void ResetTargettingFlag()
    {
        m_UserActivelyTargettingFlag = false;
    }

    protected virtual void StartNewFiringSequence() { }

    protected virtual void FireUpdate()  { }

    protected virtual void AbortAttack()
    {
        m_UserActivelyTargettingFlag = false;
        m_WeaponDischarging = false;
        m_WeaponCharging = false;
        m_MinBeamBurstTimer = 0;
        StateDefaults();
    }

    #endregion

    #region Coroutines

    private IEnumerator CheckTargetLock()
    {
        while (true)
        {
            if (!m_CanLockWeapons)
            {
                AbortAttack();
            }
            yield return null;
        }
    }

    private IEnumerator EnergyLevels()
    {
        while (true)
        {
            if (m_WeaponCharging || m_WeaponDischarging)
            {
                m_EnergyLevels = m_EnergyLevels - (m_EnergyConsumptionPerSec * Time.deltaTime);
            }
            else
            {
                m_EnergyLevels = m_EnergyLevels + (m_EnergyRechargePerSec * Time.deltaTime);
            }

            if (m_EnergyLevels <= 0)
            {
                Debug.Log("Out of Energy");
                AbortAttack();
            }

            yield return null;
        }
    }

    #endregion
}