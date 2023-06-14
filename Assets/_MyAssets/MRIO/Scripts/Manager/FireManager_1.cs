using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System;
public enum FireState
{
    Idle,
    Shooting
}

public class FireManager_1 : MonoBehaviour
{
    [SerializeField] GameObject firePrefab;
    [SerializeField] DragDetector dragDetector;
    [SerializeField] Vector3 fireOffset;
    [SerializeField] int firePoolNum = 10;
    [SerializeField] float fireSpeed = 3;
    [SerializeField] float fireDelay = 0.4f;
    [SerializeField] float fireDistanceMax;
    [SerializeField] float firePowerMultiplyer = 1.5f;
    [SerializeField] float fireDistanceThreshold = 0.5f;
    public event Action onFire;
    FireState fireState;
    Camera mainCamera;
    Transform cameraTransform;
    Transform _transform;
    ShootingFire[] shootingFires;
    float time;
    Vector3 targetPos;
    float perDuration;
    Rect[] prohibitRects;
    bool hasProhibitRect = false;
    public void ChangeState(FireState fireState)
    {
        this.fireState = fireState;
    }
    private void Awake()
    {
        _transform = transform;
        List<Transform> buf = new List<Transform>();
        for (int i = 0; i < firePoolNum; i++)
        {
            GameObject obj = Instantiate(firePrefab, _transform);
            buf.Add(obj.transform);
            obj.SetActive(false);
        }
        shootingFires = buf.Select(_t => _t.GetComponent<ShootingFire>()).ToArray();
        dragDetector.onBeginDrag += OnBeginDrag;
        dragDetector.onDrag += OnDrag;
        dragDetector.onEndDrag += OnEndDrag;
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
        SetRect();
    }
    void SetRect()
    {
        if (FireProhibitRect.Rects == null)
        {
            hasProhibitRect = false;
            return;
        }
        prohibitRects = FireProhibitRect.Rects;
        hasProhibitRect = true;
    }
    private void FixedUpdate()
    {
        switch (fireState)
        {
            case FireState.Idle:
                break;
            case FireState.Shooting:
                time += Time.deltaTime;
                if (time < fireDelay) return;
                time = 0;
                ShootingFire shootingFire = GetShootingFire();
                if (shootingFire != null)
                {
                    shootingFire.Shoot(basePos, targetPos, perDuration);
                    if (onFire != null) onFire();
                }
                break;
        }
    }
    ShootingFire GetShootingFire()
    {
        ShootingFire result = null;
        for (int i = 0; i < shootingFires.Length; i++)
        {
            if (!shootingFires[i].gameObject.activeInHierarchy) result = shootingFires[i];
        }
        return result;
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
    Vector3 basePos;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 pos = eventData.position;
        
        pos.z = System.Math.Abs(cameraTransform.position.z - _transform.position.z);
        basePos = mainCamera.ScreenToWorldPoint(pos + fireOffset);
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        if (hasProhibitRect)
        {
            for(int i = 0; i< prohibitRects.Length; i++)
            {
                if (prohibitRects[i].Contains((Vector2)basePos)) return;
            }
        }
        Vector3 pos = eventData.position;
        pos.z = System.Math.Abs(cameraTransform.position.z - _transform.position.z);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(pos + fireOffset);
        Vector3 diff = basePos - worldPos;
        float diffAmount = Vector3.Distance(worldPos, basePos);
        if (diffAmount < fireDistanceThreshold)
        {
            ChangeState(FireState.Idle);
        }
        else
        {
            ChangeState(FireState.Shooting);
        }
        targetPos = basePos + diff.normalized * Mathf.Min(diffAmount * firePowerMultiplyer, fireDistanceMax);
        float distance = Vector3.Distance(targetPos, basePos);
        perDuration = distance / fireSpeed;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ChangeState(FireState.Idle);
    }
}
