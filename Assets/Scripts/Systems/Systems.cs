using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Project: NetTrek Unity
 * Author:  Gordon Niemann
 * File:    Base Class for all systems
 */

public class Systems : MonoBehaviour
{
    public int  m_HitPoints = 100;
    public float m_MaxRepairPerSec = 1;
    public float m_DamageResilienceRatio = 1;

    protected float m_CurrentHealth;
    protected bool m_StatusOnLine = true;

    // Coroutine State Controllers
    protected delegate IEnumerator StateMethod();
    protected StateMethod m_CurrentState;

    /// <summary>
    /// Call StateDefaults to StopAllCoroutines and starts the requested Coroutine | Important: "this" on IEnumerator defined in children
    /// </summary>
    /// <param name="newState">One Coroutine to rule them all</param>
    protected void SetState(StateMethod newState)
    {
        StateDefaults();
        m_CurrentState = newState;
        StartCoroutine(m_CurrentState());
    }

    /// <summary>
    /// Spins up default coroutines and settings when SetState is called
    /// </summary>
    protected virtual void StateDefaults()
    {
        StopAllCoroutines();
    }

    protected float Wait(float duration)
    {
        duration = duration - Time.deltaTime;

        if (duration < 0)
            duration = 0;

        return duration;
    }
}