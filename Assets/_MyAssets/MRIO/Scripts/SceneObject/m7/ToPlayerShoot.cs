using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToPlayerShoot : ShooterBase, IFixedUpdater
{
    [SerializeField] float distanceMin = 5;
    [SerializeField] float distanceMax = 5;
    IBasicShooter basicShooter;
    Transform playerTransform;
    private void Awake()
    {
        basicShooter = GetComponent<IBasicShooter>();
        ChangeState(ShooterState.Shoot);
        _transform = transform;
    }
    public void OnFixedUpdate()
    {
        if (playerTransform == null)
        {
            playerTransform = (PlayerInstance.Instance == null) ? null : PlayerInstance.Instance.GetPlayer().transform;
            return;
        }
        if(_transform == null)
        {
            _transform = transform;
        }
        switch (shooterState)
        {
            case ShooterState.Idle:
                break;
            case ShooterState.Shoot:
                time += Time.deltaTime;
                if (time >= shootInterval && basicShooter != null)
                {
                    time = 0;

                    float distance = UnityEngine.Random.Range(distanceMin, distanceMax);
                    Vector3 targetPos = _transform.position + (playerTransform.position - _transform.position) .normalized * distance;
                    basicShooter.Shoot(_transform.position, targetPos, distance / shootSpeed);
                }
                break;
        }
    }
}
