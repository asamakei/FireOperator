using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.Video;

public class EditableTextWindowWithVideo : UIActivateAnimationBase
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Transform textOnlyPosTransform;
    public static EditableTextWindowWithVideo i;
    Transform _transform;
    Vector3 defaultTextPos;
    public Action onDeactivate;
    private void Awake()
    {
        i = this;
        _transform = transform;
        defaultTextPos = textMeshProUGUI.transform.position;
        gameObject.SetActive(false);
    }
    public void EditText(string text)
    {
        if (textMeshProUGUI != null) textMeshProUGUI.SetText(text);
    }

    public void SetVideo(VideoClip videoClip)
    {
        if (videoClip != null)
        {
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.clip = videoClip;
            textMeshProUGUI.transform.position = defaultTextPos;
        }
        else if (videoClip == null && textOnlyPosTransform != null)
        {
            videoPlayer.gameObject.SetActive(false);
            videoPlayer.clip = null;
            textMeshProUGUI.transform.position = textOnlyPosTransform.position;
        }
    }

    public override void Activate()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        _transform.DOKill();
        _transform.localScale = Vector3.zero;
        _transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack).SetLink(gameObject).SetUpdate(UpdateType.Normal, true);
    }

    public override void DeActivate()
    {
        if (onDeactivate != null) onDeactivate();
        Time.timeScale = 1;
        _transform.DOKill();
        _transform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack).SetLink(gameObject).OnComplete(() => gameObject.SetActive(false)).SetUpdate(UpdateType.Normal, true); ;
    }
}
