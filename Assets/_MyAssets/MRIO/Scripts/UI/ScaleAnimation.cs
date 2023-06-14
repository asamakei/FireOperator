using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleAnimation : ImageAnimation
{
    [SerializeField] float scaleAmount = 0.4f;
    [SerializeField] float scaleDuration = 0.4f;
    [SerializeField] Ease scaleEasingType = Ease.OutBack;
    Sequence sequence;
    Vector3 defaultScale;
    public override void EndAnimation()
    {
        sequence.Kill();
        transform.localScale = defaultScale;
    }

    public override void StartAnimation()
    {

        defaultScale = transform.localScale;
        sequence = DOTween.Sequence().Append(_transform.DOScale(scaleAmount, scaleDuration).SetEase(scaleEasingType).SetRelative())
                                                       .Append(_transform.DOScale(Vector3.one, scaleDuration).SetEase(scaleEasingType))
                                                       .SetLoops(-1).SetLink(gameObject);
    }
    private void OnDestroy()
    {
        if (sequence != null) sequence.Kill();
    }
    private void OnDisable()
    {
        if (sequence != null) sequence.Kill();
    }
}
