using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KanKikuchi.AudioManager;
public class DirectLoadClearEvent : StageClearEventBase
{
    [SerializeField] float clearAnimationDuration = 1f;
    [SerializeField] Animator playerAnimator;
    [SerializeField] string SEIdentifier;
    public override void OnClearClose()
    {

    }

    public override void OnClearOpen()
    {
        if (playerAnimator != null && playerAnimator.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.ChangeMovable(false);
            playerAnimator.SetTrigger(Strings.CLEAR);
        }
        AudioData audioData = AudioDataManager.Instance.GetAudioData(SEIdentifier);
        if(audioData != null)
        {
            SEManager.Instance.Play(audioData.audioClip, audioData.volume);
        }
        StageVariableDataSO nextStage = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[Variables.currentStageIndex].stageVariableData.stageData.directLoadData;
        Variables.currentStageIndex = nextStage.stageVariableData.stageIndex;
        DOVirtual.DelayedCall(clearAnimationDuration, () =>
        {
            SceneTransManager.instance.SceneTrans(nextStage.stageVariableData.stageData.loadingScenes);
        });
        
    }
}
