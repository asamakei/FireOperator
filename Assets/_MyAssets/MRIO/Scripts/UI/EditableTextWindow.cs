using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class EditableTextWindow : UIActivateAnimationBase
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    public static EditableTextWindow i;
    Transform _transform;
    private void Awake()
    {
        i = this;
        _transform = transform;
        gameObject.SetActive(false);
    }
    public void EditText(string text)
    {
        if (textMeshProUGUI != null) textMeshProUGUI.SetText(text);
    }

    public override void Activate()
    {
        gameObject.SetActive(true);
        _transform.DOKill();
        _transform.localScale = Vector3.zero;
        _transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack).SetLink(gameObject);
    }

    public override void DeActivate()
    {
        _transform.DOKill();
        _transform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack).SetLink(gameObject).OnComplete(()=> gameObject.SetActive(false));
    }
}
