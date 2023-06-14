using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActivateButton : StageButtonBase
{
    [SerializeField] GameObject targetObject;
    [SerializeField] bool willActive;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (targetObject.TryGetComponent<UIActivateAnimationBase>(out UIActivateAnimationBase uIActivate))
        {
            if (willActive) uIActivate.Activate();
            if (!willActive) uIActivate.DeActivate();
        }
    }
}
