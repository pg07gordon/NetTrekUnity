using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Controls Beam Array Weapons (Weapons that follow a charge path before firing)
 */

public class BeamArrayWeapon : EnergyWeapon
{
    #region Member Variables

    public Beam m_BeamPrefab;
    public ArrayEmitterPath m_ArrayPathForward;
    public ArrayEmitterPath m_ArrayPathReverse;
    public float m_ArrayChargeTime = 45; // TODO: Needs to be Updated

    public float AngleMagnitudeBeforeNewBeam = -1;

    internal bool m_EmitterCollision = false; // Ready to Fire

    private DOTweenPath m_ArrayPathForwardTween;
    private DOTweenPath m_ArrayPathReverseTween;
    private GameObject m_Emitter;
    private Beam m_BeamClone;

    #endregion

    #region Body

    protected override void Start ()
    {
        base.Start();

        m_Emitter = GetComponentInChildren<EmitterPoint>().gameObject;

        m_BeamClone = Instantiate(m_BeamPrefab, m_Emitter.transform.position, m_Emitter.transform.rotation);
        m_BeamClone.Init(m_Emitter);

        m_ArrayPathForwardTween = m_ArrayPathForward.gameObject.GetComponent<DOTweenPath>();
        m_ArrayPathReverseTween = m_ArrayPathReverse.gameObject.GetComponent<DOTweenPath>();
        m_ArrayPathForward.m_PrimaryArray = true; // So only one array updates Emitter Location
        
        SetEmitterChargePathsStatus(false);
    }

    protected override void StateDefaults()
    {
        base.StateDefaults();

        if (m_MinBeamBurstTimer > 0)
        {
            StartCoroutine(MinBeamBurstStopWatch());
        }
    }

    protected override void StartNewFiringSequence()
    {
        base.StartNewFiringSequence();

        m_MinBeamBurstTimer = m_MinBeamBurstTime;
        SetState(this.MinBeamBurstStopWatch);
        SetState(this.FiringSequence);
    }

    protected override void AbortAttack()
    {
        base.AbortAttack();
        m_BeamClone.TerminateBeam();
        m_WeaponCharging = false;
        m_EmitterCollision = false;
        SetEmitterChargePathsStatus(false);
    }

    private void SetEmitterChargePathsStatus(bool status)
    {
        m_ArrayPathForward.gameObject.SetActive(status);
        m_ArrayPathReverse.gameObject.SetActive(status);
    }

    protected override void FireUpdate()
    {
        m_WeaponDischarging = true;
        m_BeamClone.AttackUpdate(m_Target.transform.position);
    }

    public void SetEmitterLocation(Vector3 emitter)
    {
        m_Emitter.transform.position = emitter;
    }
    
    #endregion

    #region Coroutines

    private IEnumerator FiringSequence()
    {
        SetEmitterChargePathsStatus(true);
        m_WeaponCharging = true;

        m_ArrayPathForwardTween.DORewind();
        m_ArrayPathReverseTween.DORewind();

        float arrayPathForwardTweenDuration = Mathf.Abs(Mathf.Abs(m_MaxAngleOfAttackTo) - Mathf.Abs(m_AngleToTarget)) / m_ArrayChargeTime;
        float arrayPathReverseTweenDuration = Mathf.Abs(Mathf.Abs(m_MaxAngleOfAttackFrom) - Mathf.Abs(m_AngleToTarget)) / m_ArrayChargeTime;

        m_ArrayPathForwardTween.GetTween().timeScale = arrayPathForwardTweenDuration;
        m_ArrayPathReverseTween.GetTween().timeScale = arrayPathReverseTweenDuration;

        m_ArrayPathForwardTween.DOPlay();
        m_ArrayPathReverseTween.DOPlay();

        while (!m_EmitterCollision)
        {
            yield return null;
        }

        m_WeaponCharging = false;

        if (m_Target != null)
        {
            SetEmitterChargePathsStatus(false);
            m_EmitterCollision = false;

            m_WeaponDischarging = true;
            m_BeamClone.AttackNew(m_Target.transform.position);
        }
    }

    private IEnumerator MinBeamBurstStopWatch()
    {
        while (true)
        {
            while (m_EmitterCollision)
            {
                yield return null;
            }

            while (m_MinBeamBurstTimer > 0)
            {
                m_MinBeamBurstTimer = Wait(m_MinBeamBurstTimer);
                yield return null;
            }

            if (m_UserActivelyTargettingFlag == false)
            {
                yield return new WaitForSeconds(m_BeamDelayAfterStopTargeting);
                AbortAttack();
            }

            yield return null;
        }
    }

    #endregion
}