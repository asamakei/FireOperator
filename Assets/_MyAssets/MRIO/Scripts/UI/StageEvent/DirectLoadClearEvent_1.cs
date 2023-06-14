using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KanKikuchi.AudioManager;
public class DirectLoadClearEvent_1 : StageClearEventBase
{
    [SerializeField] string SEIdentifier;
    [SerializeField] Texture transTexture;
    public override void OnClearClose()
    {

    }

    public override void OnClearOpen()
    {
        AudioData audioData = AudioDataManager.Instance.GetAudioData(SEIdentifier);
        if (audioData != null) SEManager.Instance.Play(audioData.audioClip, audioData.volume);
        StageLockManager.i.ForceUnlockStage(MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[Variables.currentStageIndex].stageVariableData);
        ButtonUIManager.i.ChangeActivatingButton(~ActivatingState.Title & ~ActivatingState.Retry & ~ActivatingState.Skip);
        StageVariableDataSO nextStage = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[Variables.currentStageIndex].stageVariableData.stageData.directLoadData;
        StageLockManager.i.UnlockStage(nextStage.stageVariableData);
        Variables.currentStageIndex = nextStage.stageVariableData.stageIndex;
        SceneTransManager.instance.AddListener(OnSceneLoadEnd);
        SceneTransManager.instance.SceneTrans(nextStage.stageVariableData.stageData.loadingScenes, true, true, transTexture);

    }

    void OnSceneLoadEnd()
    {
        SceneTransManager.instance.RemoveListener(OnSceneLoadEnd);
        SceneTransManager.instance.SetDefaultTexture();
    }
}
