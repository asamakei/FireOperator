using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using KanKikuchi.AudioManager;
[RequireComponent(typeof(ImageAnimation))]
public class GoButton : StageButtonBase
{
    [SerializeField] StageInfoManager stageInfoManager;
    ImageAnimation imageAnimation;
    protected override void Awake()
    {
        base.Awake();
        imageAnimation = GetComponent<ImageAnimation>();
        
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (stageInfoManager.stageVariableData == null)
        {
            SEManager.Instance.Play(SEPath.ERROR);
            return;
        }
        Variables.currentStageIndex = stageInfoManager.stageVariableData.stageIndex;
        imageAnimation.EndAnimation();
        SceneTransManager.instance.SceneTrans(stageInfoManager.stageVariableData.stageData.loadingScenes);
    }
}
