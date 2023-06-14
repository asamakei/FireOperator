using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdTest : MonoBehaviour
{
    [SerializeField] AdPosition adPosition;
    [SerializeField] Vector2 adSize;
    private BannerView bannerView;
    private void Start()
    {
        MobileAds.Initialize(initStatus => { });
        RequestBanner();
    }

    void RequestBanner()
    {
        string adUnitId;
#if UNITY_IPHONE
        adUnitId = "ca-app-pub-2093568338274363/8930597442";
#else
        adUnitId = "unexpected_platform";
#endif
        this.bannerView = new BannerView(adUnitId, new AdSize(Mathf.RoundToInt(adSize.x), Mathf.RoundToInt(adSize.y)), adPosition);

        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    private void OnDisable()
    {
        bannerView.Hide();
    }
}
