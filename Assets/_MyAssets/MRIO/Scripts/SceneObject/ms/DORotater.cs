using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DORotater : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] Vector3 rotateValue;
    [SerializeField] Ease easingType;

    private void Awake()
    {
        transform.DORotate(rotateValue, duration,RotateMode.FastBeyond360).SetEase(easingType).SetLink(gameObject).SetLoops(-1);
    }
}
