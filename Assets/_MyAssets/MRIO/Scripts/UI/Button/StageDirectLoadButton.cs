using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageDirectLoadButton : StageButtonBase
{
    [SerializeField] StageVariableDataSO loadingstageVariableDataSO;

    public override void OnPointerDown(PointerEventData eventData)
    {
        SceneTransManager.instance.SceneTrans(loadingstageVariableDataSO.stageVariableData.stageData.loadingScenes);
        base.OnPointerDown(eventData);
    }
}
