using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;
[RequireComponent(typeof(Image))]
public class StageCellView : StageButtonBase, IPointerDownHandler, IPointerUpHandler
{
    static int PUSHEDPOSOFFSET = -2;
    [SerializeField] TextMeshProUGUI stageButtonName;
    [SerializeField] Image notPlayableImage;
    Action<StageVariableData> onStageSelected;
    StageVariableData stageData;
    bool isPlayable;
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (stageData != null && !isPlayable) return;
        if (stageButtonName != null) stageButtonName.rectTransform.localPosition += new Vector3(0, PUSHEDPOSOFFSET);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (stageData != null && !isPlayable) return;
        base.OnPointerUp(eventData);
        stageButtonName.rectTransform.localPosition -= new Vector3(0, PUSHEDPOSOFFSET);
        if (onStageSelected != null) onStageSelected(stageData);
    }

    public void SetData(StageVariableData stageVariableData, Action<StageVariableData> onStageSelected)
    {
        this.stageData = stageVariableData;
        this.onStageSelected = onStageSelected;
        stageData.buttonTransform = transform;
        isPlayable = SaveDataManager.Instance.saveData.savingDatas[Mathf.Min(stageVariableData.stageIndex, SaveDataManager.Instance.saveData.savingDatas.Length - 1)].isPlayable;
        if ((!isPlayable && stageVariableData.stageData.requirement == StageLoadRequirement.Keycode) || !stageVariableData.isShowable)
        {
            gameObject.SetActive(false);
        }
        if (notPlayableImage != null && !isPlayable)
        {
            notPlayableImage.sprite = stageVariableData.notPlayableSprite;
            notPlayableImage.color = Color.white;

        }
        else
        {
            notPlayableImage.color = Color.clear;
        }
        if (stageButtonName != null)
        {
            stageButtonName.SetText(stageVariableData.stageData.stageButtonNumber);
        }
    }
}
