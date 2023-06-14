using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasicShooter 
{
    public void Shoot(Vector3 from, Vector3 targetPos, float duration);
}
