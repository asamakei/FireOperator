using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class AudioDBManager : SingletonMonoBehaviour<AudioDBManager>
{
    [SerializeField] AudioDataDBSO audiodataDBSO;

    public AudioDataDBSO audioDataDBSO
    {
        get { return audiodataDBSO; }
    }

    private void Start()
    {
        SaveDataManager.Instance.OnSaveDataModified += OnSaveDataModified;
        OnSaveDataModified(SaveDataManager.Instance.saveData);
    }
    private void OnDestroy()
    {
        SaveDataManager.Instance.OnSaveDataModified -= OnSaveDataModified;
    }

    public void OnSaveDataModified(SaveData saveData)
    {
        BGMManager.Instance.ChangeBaseVolume(1-saveData.optionData.bgmVolumeRate);
        SEManager.Instance.ChangeBaseVolume(1-saveData.optionData.seVolumeRate);
    }
}
