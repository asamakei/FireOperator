using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class StageBGMPlayer : MonoBehaviour
{
    [SerializeField] AudioDataDBSO audioDataDBSO;
    [SerializeField] float fadeDuration = 0.5f;
    private void Start()
    {

        //if (audioDataDBSO == null || MasterDataManager.i == null)
        //{
        //    Debug.Log("null");
        //    return;
        //}
        int index = Mathf.Min(MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs.Length - 1, Variables.currentStageIndex);
        AudioData audioData = audioDataDBSO.GetAudioData(MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[index].stageVariableData.stageData.bgmIdentifier);
        List<string> playingAudios = BGMManager.Instance.GetCurrentAudioNames();
        if (audioData != null && playingAudios.Count == 0)
        {
            BGMManager.Instance.Play(audioData.audioClip, audioData.volume);
        }
        else if (audioData != null && !playingAudios.Contains(audioData.audioClip.name))
        {
            BGMManager.Instance.FadeOut(fadeDuration, () =>
            {
                BGMManager.Instance.Play(audioData.audioClip, audioData.volume);
            });
        }
    }
}
