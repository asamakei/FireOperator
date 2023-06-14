using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBase : MonoBehaviour
{
    [SerializeField] protected float shootSpeed = 4;
    [SerializeField] protected float shootInterval = 1f;
    protected ShooterState shooterState;
    protected float time = 0;
    protected Transform _transform;

    public float ShootSpeed
    {
        set { shootSpeed = value; }
        get { return shootSpeed; }
    }
    public float ShootInterval
    {
        set { shootInterval = value; }
        get { return shootInterval; }
    }

    public virtual void ChangeState(ShooterState shooterState)
    {
        this.shooterState = shooterState;
    }
}
