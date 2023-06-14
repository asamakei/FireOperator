using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
[System.Serializable]
public class StageLoadButtonWithOffset
{
    public int offset;
    public StageLoadButton stageLoadButton;
}

public class LevelSelectManager : MonoBehaviour, IActivater
{
    [SerializeField] StageLoadButtonWithOffset[] stageLoadButtonWithOffsets;
    [SerializeField] TextMeshProUGUI stageIndexText;
    [SerializeField] Player player;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Activate(float duration)
    {
        if (player != null) player.ChangeMovable(false);
        SetLoadingData();
        gameObject.SetActive(true);
    }

    public void DeActivate(float duration)
    {
        gameObject.SetActive(false);
    }
    void SetLoadingData()
    {
        Array.ForEach(stageLoadButtonWithOffsets, stageLoadButtonWithOffset =>
        {
            int loadingindex = Variables.currentStageIndex + stageLoadButtonWithOffset.offset;
            if (stageLoadButtonWithOffset.offset != 0  && (loadingindex < 0 || loadingindex >= MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs.Length || (loadingindex < SaveDataManager.Instance.saveData.savingDatas.Length && !SaveDataManager.Instance.saveData.savingDatas[loadingindex].isPlayable) || !MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[loadingindex].stageVariableData.isShowable))
            {
                stageLoadButtonWithOffset.stageLoadButton.gameObject.SetActive(false);
            }
            else
            {
                stageLoadButtonWithOffset.stageLoadButton.loadingStageVariableData = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[loadingindex].stageVariableData;
            }
        });
        if (stageIndexText == null) return;
        if (SaveDataManager.Instance.StageVariableDatas[Variables.currentStageIndex].stageData.shouldUseStageName)
        {
            stageIndexText.SetText(SaveDataManager.Instance.StageVariableDatas[Variables.currentStageIndex].stageData.stageName);
        }
        else
        {
            stageIndexText.SetText("Stage " + (Variables.currentStageIndex + 1));
        }
    }
    public StageVariableData GetCurrentStageData()
    {
        return (MasterDataManager.Instance.stageVariableDataDBSO == null) ? null : MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[Variables.currentStageIndex].stageVariableData;
    }
}
