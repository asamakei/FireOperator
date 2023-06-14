using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLockManager : MonoBehaviour
{
    public static StageLockManager i;
    private void Awake()
    {
        i = this;
    }
    /// <summary>
    /// return isUnlocked
    /// </summary>
    /// <param name="stageVariableData"></param>
    /// <returns></returns>
    public bool UnlockStage(StageVariableData stageVariableData)
    {
        int index = 0;
        switch (stageVariableData.stageData.requirement)
        {
            case StageLoadRequirement.PreviousClear:
                index = Mathf.Clamp(stageVariableData.stageIndex - 1, 0, SaveDataManager.Instance.StageVariableDatas.Length - 1);
                if (SaveDataManager.Instance.saveData.savingDatas[index].isPlayable && stageVariableData.stageIndex < SaveDataManager.Instance.saveData.savingDatas.Length)
                {
                    SaveDataManager.Instance.saveData.savingDatas[stageVariableData.stageIndex].isPlayable = true;
                }
                break;
            case StageLoadRequirement.VideoWatch:
                break;
            case StageLoadRequirement.Keycode:
                SaveDataManager.Instance.saveData.savingDatas[stageVariableData.stageIndex].isPlayable = true;
                break;
            default:
                return false;
        }
        return false;
    }

    public void ForceUnlockStage(StageVariableData stageVariableData)
    {
        int index = Mathf.Clamp(stageVariableData.stageIndex - 1, 0, SaveDataManager.Instance.StageVariableDatas.Length - 1);
        if (stageVariableData.stageIndex < SaveDataManager.Instance.saveData.savingDatas.Length)
        {
            SaveDataManager.Instance.saveData.savingDatas[stageVariableData.stageIndex].isPlayable = true;
        }
    }
}
