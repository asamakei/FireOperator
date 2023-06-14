using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinObject : MonoBehaviour
{
    Transform _transform;
    public Transform coinTransform
    {
        get
        {
            if (_transform == null) _transform = transform;

            return _transform;
        }
    }
    public void MoveTo(Vector3 to, float duration, int addCoinNum)
    {
        _transform.DOKill();
        _transform.DOMove(to, duration).SetEase(Ease.InQuart);
    }
}
