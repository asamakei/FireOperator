using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UnityEngine.Video;

public class DirectLoadClearEvent_3 : StageClearEventBase
{
    [SerializeField] string clearSEIdentifier;
    [SerializeField] float clearSoundDuration = 2;
    [SerializeField] Animator playerAnimator;
    public override void OnClearClose()
    {

    }

    public override void OnClearOpen()
    {
        StageData stageData = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[Variables.currentStageIndex].stageVariableData.stageData;
        if (playerAnimator.gameObject.TryGetComponent<Player>(out Player player)) player.ChangeMovable(false);
        float tmp = Random.Range(0, 100);
        ButtonUIManager.i.ChangeActivatingButton(~ActivatingState.Title & ~ActivatingState.Retry & ~ActivatingState.Skip);
        playerAnimator.SetTrigger(Strings.CLEAR);
        float volumebuf = BGMManager.Instance.Volume;
        ClearSE();
        BGMManager.Instance.ChangeBaseVolume(0);
        DOVirtual.DelayedCall(clearSoundDuration, () =>
        {
            BGMManager.Instance.Stop();
            BGMManager.Instance.ChangeBaseVolume(volumebuf);

            ButtonUIManager.i.ChangeActivatingButton(~ActivatingState.Title & ~ActivatingState.Retry & ~ActivatingState.Skip);
            SceneTrans();
        });
    }
    void ClearSE()
    {
        AudioData data = AudioDBManager.Instance.audioDataDBSO.GetAudioData(clearSEIdentifier);
        if (data != null) SEManager.Instance.Play(data.audioClip, data.volume);
    }


    void SceneTrans()
    {
        StageVariableDataSO nextStage = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[Variables.currentStageIndex].stageVariableData.stageData.directLoadData;
        Variables.currentStageIndex = nextStage.stageVariableData.stageIndex;
        if (nextStage == null) return;
        SceneTransManager.instance.SceneTrans(nextStage.stageVariableData.stageData.loadingScenes);
        EditableTextWindowWithVideo.i.onDeactivate -= SceneTrans;
    }
}
