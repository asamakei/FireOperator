using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UnityEngine.Video;

public class DirectLoadClearEvent_2 : StageClearEventBase
{
    [SerializeField] string SEIdentifier;
    [SerializeField][TextArea] string text;
    [SerializeField] VideoClip videoClip;
    public override void OnClearClose()
    {

    }

    public override void OnClearOpen()
    {
        AudioData audioData = AudioDataManager.Instance.GetAudioData(SEIdentifier);
        if (audioData != null) SEManager.Instance.Play(audioData.audioClip, audioData.volume);
        StageData stageData = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[Variables.currentStageIndex].stageVariableData.stageData;
        EditableTextWindowWithVideo.i.EditText(stageData.endText);
        EditableTextWindowWithVideo.i.SetVideo(stageData.endClip);
        EditableTextWindowWithVideo.i.onDeactivate += SceneTrans;
        EditableTextWindowWithVideo.i.Activate();
    }

    void SceneTrans()
    {
        StageVariableDataSO nextStage = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[Variables.currentStageIndex].stageVariableData.stageData.directLoadData;
        Variables.currentStageIndex = nextStage.stageVariableData.stageIndex;
        SceneTransManager.instance.SceneTrans(nextStage.stageVariableData.stageData.loadingScenes);
        EditableTextWindowWithVideo.i.onDeactivate -= SceneTrans;
    }
}
