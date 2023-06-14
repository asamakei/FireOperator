using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class StraightFireShooter : MonoBehaviour, IBasicShooter
{
    [SerializeField] Transform poolParent;
    [SerializeField] GameObject firePrefab;
    [SerializeField] int firePoolNum = 30;
    ShootingFire[] shootingFires;
    public event Action onFire;
    Transform _transform;
    public Transform _Transform
    {
        get { return _transform; }
    }

    private void Awake()
    {
        _transform = transform;
        List<Transform> buf = new List<Transform>();
        for (int i = 0; i < firePoolNum; i++)
        {
            GameObject obj = Instantiate(firePrefab, poolParent);
            buf.Add(obj.transform);
            obj.SetActive(false);
        }
        shootingFires = buf.Select(_t => _t.GetComponent<ShootingFire>()).ToArray();
    }

    public void Shoot(Vector3 from, Vector3 targetPos, float duration)
    {
        ShootingFire shootingFire = GetShootingFire();
        if (shootingFire == null) return;
        shootingFire.Shoot(from, targetPos, duration);
        if (onFire != null) onFire();
    }
    ShootingFire GetShootingFire()
    {
        ShootingFire result = null;
        for (int i = 0; i < shootingFires.Length; i++)
        {
            if (!shootingFires[i].gameObject.activeInHierarchy) result = shootingFires[i];
        }
        return result;
    }

}
