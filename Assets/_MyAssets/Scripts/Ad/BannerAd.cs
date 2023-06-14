using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
public class BannerAd : MonoBehaviour
{
    [SerializeField] AdPosition adPosition;
    [SerializeField] Vector2 adSize;
    [SerializeField] bool isSmartBanner;
    private BannerView bannerView;
    private void OnEnable()
    {
        MobileAds.Initialize(initStatus => { });
        RequestBanner();
    }
    private void OnDisable()
    {
        bannerView.Hide();
    }

    void RequestBanner()
    {
        string adUnitId;
        //        //テスト用
        //#if UNITY_IPHONE
        //        adUnitId = "ca-app-pub-3940256099942544/6300978111";
        //#elif UNITY_ANDROID
        //        adUnitId = "ca-app-pub-3940256099942544/6300978111";
        //#else
        //        adUnitId = "unexpected_platform";
        //#endif
        //本番用
#if UNITY_IOS
        adUnitId = "ca-app-pub-2093568338274363/3616750497";
#elif UNITY_ANDROID
        adUnitId = "ca-app-pub-2093568338274363/3982548164";
#else
                adUnitId = "unexpected_platform";
#endif
        this.bannerView = (isSmartBanner) ? new BannerView(adUnitId, AdSize.SmartBanner, adPosition) : new BannerView(adUnitId, new AdSize(Mathf.RoundToInt(adSize.x), Mathf.RoundToInt(adSize.y)), adPosition);

        AdRequest request = new AdRequest.Builder().Build();
        if (request != null) this.bannerView.LoadAd(request);
        // Load the banner with the request.

    }

}
