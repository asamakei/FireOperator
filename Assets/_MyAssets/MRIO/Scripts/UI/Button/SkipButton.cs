using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GoogleMobileAds.Api;
using System;
using KanKikuchi.AudioManager;
public enum AdState
{
    Idle,
    Loading,
    Loaded,
    Show
}

public class SkipButton : StageButtonBase
{
    [SerializeField] GameObject panelObject;
    [SerializeField] GameObject loadingObject;
    [SerializeField][TextArea] string errorMessage = "広告読み込みに失敗しました";
    private RewardedAd rewardedAd;
    private AdState adState;
    public void ChangeAdState(AdState adState)
    {
        this.adState = adState;
    }
    private void FixedUpdate()
    {
        switch (adState)
        {
            case AdState.Idle:
                break;
            case AdState.Loading:
                if (rewardedAd.IsLoaded())
                {
                    ChangeAdState(AdState.Loaded);
                }
                break;
            case AdState.Loaded:
                rewardedAd.Show();
                ChangeAdState(AdState.Show);
                break;
            case AdState.Show:
                break;
        }
    }
    protected override void Awake()
    {
        int index = Mathf.Min(Variables.currentStageIndex + 1, MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs.Length - 1);
        if (MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[index].stageVariableData.stageData.requirement != StageLoadRequirement.PreviousClear)
        {
            ButtonUIManager.i.ChangeActivatingButton(ActivatingState.Title | ActivatingState.Retry & ~ActivatingState.Skip);
            return;
        }
        MobileAds.Initialize(initStatus => { });

        string adunitId;
        //        //テスト用
        //#if UNITY_IPHONE
        //        adunitId = "ca-app-pub-3940256099942544/5224354917";
        //#elif UNITY_ANDROID
        //        adunitId = "ca-app-pub-3940256099942544/5224354917";
        //#else
        //        adunitId = "unexpected_platform";
        //#endif
        //本番用
#if UNITY_IOS
                adunitId = "ca-app-pub-2093568338274363/5419118456";
#elif UNITY_ANDROID
                adunitId = "ca-app-pub-2093568338274363/4169416272";
#else
                adunitId = "unexpected_platform";
#endif
        this.rewardedAd = new RewardedAd(adunitId);
        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        base.Awake();
    }
    float bgmVolumeBuf;
    float seVolumeBuf;
    public override void OnPointerDown(PointerEventData eventData)
    {
        bgmVolumeBuf = BGMManager.Instance.Volume;
        seVolumeBuf = SEManager.Instance.Volume;
        if (!SaveDataManager.Instance.saveData.isNoAdSkipMode)
        {
            AdRequest request = new AdRequest.Builder().Build();
            this.rewardedAd.LoadAd(request);
            ChangeAdState(AdState.Loading);
            panelObject.SetActive(true);
            loadingObject.SetActive(true);
        }
        else
        {
            OnRewarded();
        }

        base.OnPointerDown(eventData);
    }

    public void OnRewarded()
    {
        if (SceneTransManager.instance == null) return;
        int index = Mathf.Min(Variables.currentStageIndex + 1, MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs.Length - 1);
        Variables.currentStageIndex = index;
        loadingObject.SetActive(false);
        panelObject.SetActive(false);
        StageVariableData stageVariableData = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[index].stageVariableData;
        SceneTransManager.instance.SceneTrans(stageVariableData.stageData.loadingScenes);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
    }
    void MuteBGMAndSE()
    {
        BGMManager.Instance.ChangeBaseVolume(0);
        SEManager.Instance.ChangeBaseVolume(0);
    }

    void RevertBGMAndSE()
    {
        BGMManager.Instance.ChangeBaseVolume(bgmVolumeBuf);
        SEManager.Instance.ChangeBaseVolume(seVolumeBuf);
    }
    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        EditableTextWindowWithVideo.i.SetVideo(null);
        EditableTextWindowWithVideo.i.EditText(errorMessage);
        EditableTextWindowWithVideo.i.Activate();
        panelObject.SetActive(false);
        loadingObject.SetActive(false);
        Time.timeScale = 1;

    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MuteBGMAndSE();
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        EditableTextWindowWithVideo.i.SetVideo(null);
        EditableTextWindowWithVideo.i.EditText(errorMessage);
        panelObject.SetActive(false);
        loadingObject.SetActive(false);
        EditableTextWindowWithVideo.i.Activate();
        Time.timeScale = 1;
        RevertBGMAndSE();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        OnRewarded();
        ChangeAdState(AdState.Idle);
        RevertBGMAndSE();
        Time.timeScale = 1;
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {

    }
}
