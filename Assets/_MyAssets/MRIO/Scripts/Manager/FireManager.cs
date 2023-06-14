using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireManager : MonoBehaviour
{
    [SerializeField] GameObject firePrefab;
    [SerializeField] float duration;
    [SerializeField] DragDetector dragDetector;
    [SerializeField] Vector3 fireOffset;
    Transform fireTransform;
    IActivater fireActivater;
    Camera mainCamera;
    Transform cameraTransform;
    Transform _transform;
    private void Awake()
    {
        fireTransform = Instantiate(firePrefab, transform).transform;
        fireTransform.gameObject.SetActive(false);
        fireActivater = fireTransform.GetComponent<IActivater>();
        dragDetector.onBeginDrag += OnBeginDrag;
        dragDetector.onDrag += OnDrag;
        dragDetector.onEndDrag += OnEndDrag;
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
        _transform = transform;
    }
    void RemoveDelegate()
    {
        dragDetector.onBeginDrag -= OnBeginDrag;
        dragDetector.onDrag -= OnDrag;
        dragDetector.onEndDrag -= OnEndDrag;
    }

    private void OnDestroy()
    {
        RemoveDelegate();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 pos = eventData.position;
        pos.z = System.Math.Abs(cameraTransform.position.z - _transform.position.z);
        fireTransform.position = mainCamera.ScreenToWorldPoint(pos + fireOffset);
        if (fireActivater != null) fireActivater.Activate(duration);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = eventData.position;
        pos.z = System.Math.Abs(cameraTransform.position.z - _transform.position.z);
        fireTransform.position = mainCamera.ScreenToWorldPoint(pos + fireOffset);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (fireActivater != null) fireActivater.DeActivate(duration);
    }
}
