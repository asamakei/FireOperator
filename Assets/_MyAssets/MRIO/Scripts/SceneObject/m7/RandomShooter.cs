using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum ShooterState
{
    Idle,
    Shoot
}
public class RandomShooter : ShooterBase, IFixedUpdater
{
    [SerializeField] float angleMin = -20;
    [SerializeField] float angleMax = 20;
    [SerializeField] float distanceMin = 5;
    [SerializeField] float distanceMax = 5;
    IBasicShooter basicShooter;
    public event Action<float> onFire;

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
                    float angle = (float)(UnityEngine.Random.Range(angleMin, angleMax) * Math.PI) / 180;
                    float distance = UnityEngine.Random.Range(distanceMin, distanceMax);
                    Vector3 targetPos = _transform.position + new Vector3((float)Math.Cos(angle) * distance, (float)Math.Sin(angle) * distance, _transform.position.z);
                    if (onFire != null) onFire(angle);
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
