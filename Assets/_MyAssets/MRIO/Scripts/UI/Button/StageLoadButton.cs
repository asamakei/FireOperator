using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageLoadButton : StageButtonBase
{
    StageVariableData loadingstageVariableData;
    public StageVariableData loadingStageVariableData
    {
        set { loadingstageVariableData = value; }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        
        if (loadingstageVariableData == null || SceneTransManager.instance == null) return;
        Variables.currentStageIndex = loadingstageVariableData.stageIndex;
        SceneTransManager.instance.SceneTrans(loadingstageVariableData.stageData.loadingScenes);
        base.OnPointerDown(eventData);
    }
}
