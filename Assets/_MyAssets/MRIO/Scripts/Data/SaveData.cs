using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
/// <summary>
/// 何かセーブデータに必要なものがあれば追加可能
/// </summary>
[System.Serializable]
public class SaveData 
{
    public SavingStageData[] savingDatas;
    public OptionData optionData;
    public int lastPlayedStageIndex = 0;
    public bool wasTutorialPlayed = false;
    public bool isNoAdSkipMode = false;
    public ShopData shopData;

    public SaveData(StageVariableData[] stageVariableDatas)
    {
        optionData = new OptionData();
        shopData = new ShopData();
        if (stageVariableDatas == null) return;
        savingDatas = stageVariableDatas.Select(stageVariableData =>
        {
            SavingStageData savingStageData = new SavingStageData();
            savingStageData.isPlayable = (stageVariableData == null) ? false : stageVariableData.isPlayable;
            return savingStageData;
        }).ToArray();
    }
}
