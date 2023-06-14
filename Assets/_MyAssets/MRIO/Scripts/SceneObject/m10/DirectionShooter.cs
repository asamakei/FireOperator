using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DirectionShooter : ShooterBase, IFixedUpdater
{
    [SerializeField] Vector3 direction;
    [SerializeField] float distance = 5;
    IBasicShooter basicShooter;

    public void OnFixedUpdate()
    {
        switch (shooterState)
        {
            case ShooterState.Idle:
                break;
            case ShooterState.Shoot:
                time += Time.deltaTime;
                if (time >= shootInterval && basicShooter != null)
                {
                    time = 0;
                    Vector3 targetPos = _transform.position + direction * distance;
                    basicShooter.Shoot(_transform.position, targetPos, distance / shootSpeed);
                }
                break;
        }
    }

    private void Awake()
    {
        basicShooter = GetComponent<IBasicShooter>();
        ChangeState(ShooterState.Shoot);
        _transform = transform;
    }
}
