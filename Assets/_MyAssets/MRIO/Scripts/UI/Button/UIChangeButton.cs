using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIChangeButton : StageButtonBase
{
    [SerializeField] UIState changingUIState;
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (SaveDataManager.Instance.saveData.wasTutorialPlayed) UIManager.uIState = changingUIState;
        
    }
}
