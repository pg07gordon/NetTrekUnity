﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSystems : Systems
{
    public int m_MaxAllocatedEnergyUnits = 120;

    protected float m_CurrentAllocatedEnergy;

    protected float m_CurrentAvailableEnergy;

    protected float m_CurrentRepairPerSec;

    protected virtual void Start()
    {
        m_CurrentAllocatedEnergy = 100; // TODO: Update
        m_CurrentAvailableEnergy = 100; // TODO: Update
    }
}