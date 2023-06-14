using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TapPointSprite : MonoBehaviour, IActivater
{
    const float SCALEDURATION = 0.5f;
    [SerializeField] DragDetector dragDetector;
    Camera mainCamera;
    Transform cameraTransform;
    Transform _transform;
    Vector3 defaultScale;
    private void Awake()
    {
        _transform = transform;
        dragDetector.onBeginDrag += OnBeginDrag;
        dragDetector.onDrag += OnDrag;
        dragDetector.onEndDrag += OnEndDrag;
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
        gameObject.SetActive(false);
        defaultScale = _transform.localScale;
        
    }
    private void OnDestroy()
    {
        RemoveDelegate();
    }
    void RemoveDelegate()
    {
        dragDetector.onBeginDrag -= OnBeginDrag;
        dragDetector.onDrag -= OnDrag;
        dragDetector.onEndDrag -= OnEndDrag;
    }
    Sequence sequencebuf;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 pos = eventData.position;
        pos.z = System.Math.Abs(cameraTransform.position.z - _transform.position.z);
        Vector3 basePos = mainCamera.ScreenToWorldPoint(pos);
        _transform.position = basePos;
        _transform.localScale = defaultScale;
        Activate(0);
        sequencebuf = DOTween.Sequence().Append(_transform.DOScale(0.2f, SCALEDURATION).SetEase(Ease.Linear).SetRelative())

                          .Append(_transform.DOScale(-0.2f, SCALEDURATION).SetEase(Ease.Linear).SetRelative()).SetLoops(-1).SetLink(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        sequencebuf.Kill();
        _transform.localScale = defaultScale;
        _transform.DOKill();
        DeActivate(0);
    }

    public void Activate(float duration)
    {
        gameObject.SetActive(true);
    }

    public void DeActivate(float duration)
    {
        gameObject.SetActive(false);
    }
}
