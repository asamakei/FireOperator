using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class RetryButton : StageButtonBase
{
    [SerializeField] LevelSelectManager levelSelectManager;
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (levelSelectManager == null) return;
        base.OnPointerDown(eventData);
        DOTween.KillAll();
        SceneTransManager.instance.SceneTrans(levelSelectManager.GetCurrentStageData().stageData.loadingScenes);
    }

}
