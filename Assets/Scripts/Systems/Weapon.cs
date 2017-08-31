using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : SubSystems
{
    [SerializeField]
    protected GameObject m_WeaponBody;

    [SerializeField]
    protected Vector2 m_AngleOfAttack;

    [SerializeField]
    protected float m_MaxAttackDistance = 50;

    [SerializeField]
    protected float m_MinAttackDistance = 5;

    public Vector2 GetAngleOfAttack()
    {
        return m_AngleOfAttack;
    }

    private void SetAngleOfAttack(Vector2 value)
    {
        value.y = MaxAngleValues(value.y);
        value.y = MaxAngleValues(value.y);

        m_AngleOfAttack = value;
    }

    private float MaxAngleValues(float value)
    {
        if (value > 180)
            value = 180;
        else if (value < -180)
            value = -180;
        return value;
    }

    protected override void Start()
    {
        base.Start();

        SetAngleOfAttack(m_AngleOfAttack);
    }
        

}
