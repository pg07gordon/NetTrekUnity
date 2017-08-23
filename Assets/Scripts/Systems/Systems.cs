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

    protected float m_CurrentHealth;
    protected bool m_StatusOnLine = true;

    // Coroutine State Controllers
    protected delegate IEnumerator StateMethod();
    protected StateMethod m_CurrentState;

    /// <summary>
    /// Starts a new coroutine and stops all previous ones | Important: "this" on IEnumerator defined in children
    /// </summary>
    /// <param name="newState">One Coroutine to rule them all</param>
    protected void SetState(StateMethod newState)
    {
        StateDefaults();
        m_CurrentState = newState;
        StartCoroutine(m_CurrentState());
    }

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


