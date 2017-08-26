using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Base class for Energy Weapons
 */

public class EnergyWeapon : Weapon
{
    #region Member Variables

    //public float m_OverAngleReFire = 20;

    [SerializeField]
    private float m_EnergyConsumptionPerSec = 20;

    [SerializeField]
    private float m_MinEnergyLevelToFire = 40;

    [SerializeField]
    protected float m_BeamDelayAfterStopTargeting = 0.1f;

    [SerializeField]
    protected float m_MinBeamBurstTime = 0.25f;
    protected float m_MinBeamBurstTimer = 0;

    protected bool m_WeaponDischarging = false;
    protected bool m_WeaponCharging = false;
    protected bool m_UserActivelyTargettingFlag = false;

    protected GameObject m_Target;
    protected EmitterPoint m_Emitter;

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
            return m_Target.transform.position - m_Ship.transform.position;
        }
    }

    protected float m_AngleToTarget
    {
        get
        {
            return Vector3.SignedAngle(m_TargetDir, m_Ship.transform.forward, Vector3.up) * -1;
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
                && m_AngleOfAttack.x < m_AngleToTarget
                && m_AngleOfAttack.y > m_AngleToTarget)
                return true;
            else
                return false;
        }
    }

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

    private Unit m_Ship;

    #endregion

    #region Body

    protected override void Start()
    {
        base.Start();

        m_Ship = GetComponentInParent<Unit>();
        m_Emitter = GetComponentInChildren<EmitterPoint>();
        m_EnergyLevels = m_CurrentAvailableEnergy;

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