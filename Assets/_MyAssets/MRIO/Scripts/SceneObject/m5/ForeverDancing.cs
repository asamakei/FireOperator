using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ForeverDancing : MonoBehaviour
{
    [SerializeField] Vector3 moveVector;
    [SerializeField] Ease ease;
    [SerializeField] float duration;
    Sequence seq;
    private void Awake()
    {
        seq = DOTween.Sequence().Append(transform.DOMove(moveVector, duration).SetRelative().SetEase(ease)).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
    }
}
