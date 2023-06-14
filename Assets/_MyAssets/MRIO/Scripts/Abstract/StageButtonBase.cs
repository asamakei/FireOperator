using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]

public abstract class StageButtonBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] protected Sprite normalSprite;
    [SerializeField] protected Sprite pushedSprite;
    [SerializeField] protected bool canMultipleDown = false;
    public Action onPushed;
    protected Image image;
    protected virtual void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = normalSprite;
    }
    private void OnEnable()
    {
        if (image != null)
        {
            image.sprite = normalSprite;
            image.raycastTarget = true;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (image != null) image.sprite = pushedSprite;
        
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (!canMultipleDown) image.raycastTarget = false;
        if (image != null) image.sprite = normalSprite;
        if (onPushed != null) onPushed();
    }
}
