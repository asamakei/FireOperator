using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class DragDetector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event Action<PointerEventData> onBeginDrag;
    public event Action<PointerEventData> onDrag;
    public event Action<PointerEventData> onEndDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null) onBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag(eventData);
    }
}
