using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KanKikuchi.AudioManager;
using TMPro;
public class GeneralClearEvent : StageClearEventBase
{
    [SerializeField] LevelSelectManager levelSelectManager;
    [SerializeField] Animator playerAnimator;
    [SerializeField] float clearAnimationDuration = 2f;
    [SerializeField] float activateDuration = 0.5f;
    [SerializeField] string clearSEIdentifier;
    [SerializeField] string clearBGMIdentifier;
    [SerializeField] float muscleRate = 0.1f;
    [SerializeField] float clearBGMVolume = 0.2f;
    [SerializeField] int coinNumPerLevel = 10;
    [SerializeField] int coinValue = 100;
    [SerializeField] CoinCountAnimation coinCountAnimation;
    [SerializeField] float coinViewDuration = 0.3f;
    [SerializeField] string[] randomMessages;
    [SerializeField] TextMeshProUGUI clearMessage;
    //[SerializeField] float jumpHeight = 0.5f;
    //[SerializeField] float jumpPower = 1;
    public override void OnClearOpen()
    {
        ButtonUIManager.i.ChangeActivatingButton(~ActivatingState.Title & ~ActivatingState.Retry & ~ActivatingState.Skip);
        StageLockManager.i.ForceUnlockStage(MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[Variables.currentStageIndex].stageVariableData);
        int nextIndex = Variables.currentStageIndex + 1;
        if (nextIndex < MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs.Length)
        {
            StageVariableData nextStageData = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[nextIndex].stageVariableData;
            if (nextStageData.stageData.requirement == StageLoadRequirement.PreviousClear) StageLockManager.i.UnlockStage(nextStageData);
        }
        if (playerAnimator.gameObject.TryGetComponent<Player>(out Player player)) player.ChangeMovable(false);
        float tmp = Random.Range(0, 100);
        if (tmp < muscleRate * 100)
        {
            playerAnimator.SetTrigger(Strings.CLEAR_MUSCLE);
        }
        else
        {
            playerAnimator.SetTrigger(Strings.CLEAR);
        }
        int addCoinNum = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[Variables.currentStageIndex].stageVariableData.stageData.level * coinNumPerLevel;
        int coinFrom = SaveDataManager.Instance.saveData.shopData.coinNum;
        CoinManager.Instance.Addcoin(addCoinNum);
        //playerAnimator.transform.DOJump(playerAnimator.transform.position + new Vector3(0,jumpHeight,0),jumpPower,1,clearAnimationDuration);
        BGMManager.Instance.ChangeBaseVolume(clearBGMVolume);
        ClearSE();
        if (clearMessage != null)
        {
            clearMessage.SetText(randomMessages[Random.Range(0, randomMessages.Length - 1)]);
        }
        DOVirtual.DelayedCall(clearAnimationDuration, () =>
        {
            SaveDataManager.Instance.Save(Values.SAVENUMBER);
            gameObject.SetActive(true);
            if (levelSelectManager != null && levelSelectManager.TryGetComponent<IActivater>(out IActivater activater)) activater.Activate(activateDuration);
            if (coinCountAnimation != null)
            {
                coinCountAnimation.Activate(coinViewDuration);
                coinCountAnimation.DoCoinAddAnimation(coinFrom, addCoinNum, coinValue);
            }


        });
    }
    public override void OnClearClose()
    {
        gameObject.SetActive(false);
        if (levelSelectManager != null && levelSelectManager.TryGetComponent<IActivater>(out IActivater activater)) activater.DeActivate(activateDuration);
    }
    void ClearSE()
    {
        AudioData data = AudioDBManager.Instance.audioDataDBSO.GetAudioData(clearSEIdentifier);
        if (data != null) SEManager.Instance.Play(data.audioClip, data.volume);
    }
}
