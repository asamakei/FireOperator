using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
public abstract class SpriteButtonBase : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] protected Sprite normalSprite;
    [SerializeField] protected Sprite pushedSprite;
    [SerializeField] protected bool canMultipleDown = false;
    public Action onPushed;
    protected SpriteRenderer spriteRenderer;
    protected bool isPushable = false;
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;
    }
    private void OnEnable()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = normalSprite;
            isPushable = true;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (spriteRenderer != null) spriteRenderer.sprite = pushedSprite;

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (!canMultipleDown) isPushable = false;
        if (spriteRenderer != null) spriteRenderer.sprite = normalSprite;
        if (onPushed != null) onPushed();
    }
}
