using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SaveDataManager : MonoBehaviour
{
    private const string SAVEHEAD = "Data";
    [SerializeField] StageVariableDataDBSO stageVariableDataDBSO;
    private static SaveDataManager i;
    public static SaveDataManager Instance
    {
        get
        {
            if (i == null)
            {

                //シーン内からインスタンスを取得
                i = (SaveDataManager)FindObjectOfType(typeof(SaveDataManager));

                //シーン内に存在しない場合はエラー
                if (i == null)
                {

                }

            }

            return i;
        }
    }
    public event Action<SaveData> OnSaveDataModified;
    SaveData savedata;
    StageVariableData[] stageVariableDatas;
    public UIState titleState = UIState.Title;
    public SaveData saveData
    {
        get
        {
            if (savedata == null)
            {
                savedata = new SaveData(stageVariableDatas);
            }
            if (savedata.savingDatas == null && IsExistSaveData(Values.SAVENUMBER))
            {
                LoadSaveData(Values.SAVENUMBER);
            }
            if (OnSaveDataModified != null) OnSaveDataModified(savedata);
            return savedata;
        }
    }

    public StageVariableData[] StageVariableDatas
    {
        get
        {
            if (stageVariableDatas == null) RefreshStageVariableDatas();
            return stageVariableDatas;
        }
    }
    public StageVariableData[] ShowableVariableDatas
    {
        get
        {
            return GetShowableVariableDatas();
        }
    }
    private void Awake()
    {
        if (Instance == null) i = this;
        RefreshStageVariableDatas();
        SetIndex();
        if (IsExistSaveData(Values.SAVENUMBER))
        {
            LoadSaveData(Values.SAVENUMBER);

        }
        else
        {
            savedata = new SaveData(stageVariableDatas);
        }

    }

    public void RefreshStageVariableDatas()
    {
        stageVariableDatas = stageVariableDataDBSO.stageVariableDataSOs.Select(stageVariableDataSO => stageVariableDataSO.stageVariableData).ToArray();
    }
    public void SetIndex()
    {
        for (int i = 0; i < stageVariableDataDBSO.stageVariableDataSOs.Length; i++)
        {
            stageVariableDataDBSO.stageVariableDataSOs[i].stageVariableData.stageIndex = i;
        }
    }
    public bool IsExistSaveData(int index)
    {
        DataBank bank = DataBank.Open();
        string saveName = SAVEHEAD + index;
        bank.Load<SaveData>(saveName);

        return bank.ExistsKey(saveName);
    }

    public void LoadSaveData(int index)
    {
        DataBank bank = DataBank.Open();
        string saveName = SAVEHEAD + index;
        bank.Load<SaveData>(saveName);

        savedata = bank.Get<SaveData>(saveName);
        if (savedata.savingDatas.Length < stageVariableDataDBSO.stageVariableDataSOs.Length)
        {
            List<SavingStageData> savingStageDatas = new List<SavingStageData>();
            for (int i = 0; i < stageVariableDataDBSO.stageVariableDataSOs.Length; i++)
            {
                SavingStageData savingStageData;

                if (i >= savedata.savingDatas.Length)
                {
                    savingStageData = new SavingStageData();
                    savingStageData.isPlayable = (stageVariableDataDBSO.stageVariableDataSOs[i].stageVariableData == null) ? false : stageVariableDataDBSO.stageVariableDataSOs[i].stageVariableData.isPlayable;
                }
                else
                {
                    savingStageData = savedata.savingDatas[i];
                }
                savingStageDatas.Add(savingStageData);
            }
            savedata.savingDatas = savingStageDatas.ToArray();
        }
        if (OnSaveDataModified != null) OnSaveDataModified(savedata);
    }

    public void Save(int index)
    {
        DataBank bank = DataBank.Open();
        string saveName = SAVEHEAD + index;
        bank.Store(saveName, saveData);
        bank.SaveAll();
        bank.Clear();
    }

    public bool Delete(int index)
    {
        DataBank bank = DataBank.Open();
        string saveName = SAVEHEAD + index;
        return bank.Delete(saveName);
    }

    public void DeleteTempSaveData()
    {
        savedata = null;
    }

    public StageVariableData[] GetShowableVariableDatas()
    {
        List<StageVariableData> stageVariableDataList = new List<StageVariableData>();

        for (int i = 0; i < StageVariableDatas.Length; i++)
        {
            if ((StageVariableDatas[i].stageData.requirement == StageLoadRequirement.Keycode && saveData.savingDatas[StageVariableDatas[i].stageIndex].isPlayable) || (StageVariableDatas[i].stageData.requirement != StageLoadRequirement.Keycode && StageVariableDatas[i].isShowable)) stageVariableDataList.Add(StageVariableDatas[i]);
        }
        return stageVariableDataList.ToArray();
    }
}
