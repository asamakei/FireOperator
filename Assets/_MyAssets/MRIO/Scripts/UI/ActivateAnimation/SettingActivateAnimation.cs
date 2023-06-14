using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SettingActivateAnimation : UIActivateAnimationBase
{
    [SerializeField] RectTransform scalingTransform;
    [SerializeField] float heightDiff = 30;
    CanvasGroup canvasGroup;
    Vector3 buf;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        buf = scalingTransform.anchoredPosition;
    }
    public override void Activate()
    {

        scalingTransform.DOKill();
        scalingTransform.anchoredPosition = buf;
        gameObject.SetActive(true);
        DOTween.Sequence().Append(scalingTransform.DOMoveY(-heightDiff, 0).SetRelative())
                          .Append(scalingTransform.DOMoveY(heightDiff, duration).SetRelative().SetEase(Ease.OutSine));
        canvasGroup.alpha = 0;
        if (canvasGroup != null) DOTween.To(() => canvasGroup.alpha, (a) => canvasGroup.alpha = a, 1, duration).SetEase(Ease.OutCirc);
    }

    public override void DeActivate()
    {
        scalingTransform.DOKill();
        scalingTransform.DOMoveY(-heightDiff, duration).SetRelative().SetEase(Ease.OutSine);
        if (canvasGroup != null) DOTween.To(() => canvasGroup.alpha, (a) => canvasGroup.alpha = a, 0, duration).SetEase(Ease.OutCirc).OnComplete(() => gameObject.SetActive(false));
    }
}
