using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdManager : SingletonMonoBehaviour<AdManager>
{
    [SerializeField]
    Canvas uicanvas;
    [SerializeField]
    Button testButton;
    BannerView _bannerView;
    RewardedAd _rewardedAd;

    bool isClickRewardAds;

    public bool IsClickRewardAds { get; }

    // Start is called before the first frame update
    protected override void OnStart()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        CreateBannerView();
    }


    /// <summary>
    /// Creates a 320x50 banner view at top of the screen.
    /// </summary>
    public void CreateBannerView()
    {

#if UNITY_ANDROID
        string _adUnitId = "ca-app-pub-4686950845662491/2107560722";
#elif UNITY_IPHONE
        string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#elif UNITY_EDITOR
        string _adUnitId = "ca-app-pub-3940256099942544/6300978111";
#else
        string _adUnitId = "ca-app-pub-3940256099942544/6300978111";
#endif


        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }


        //AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();

        _bannerView.LoadAd(request);
    }


    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadCristalRewardedAd(Button rewardBtn)
    {
        if (isClickRewardAds)
            return;
        isClickRewardAds = true;
        rewardBtn.enabled = false;
#if UNITY_ANDROID
        string _adUnitId = "ca-app-pub-4686950845662491/2374605298";
#elif UNITY_IPHONE
        string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#elif UNITY_EDITOR
        string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
        string _adUnitId = "unused";
#endif
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
            });
        if (_rewardedAd != null)
            ShowCristalRewardedAd();
        isClickRewardAds = false;
        rewardBtn.enabled = true;
    }

    void ShowCristalRewardedAd()
    {
        const string rewardMsg =
        "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                int randCristal = UnityEngine.Random.Range(1, 11);
                SaveDatas.Data.etc.cristal += randCristal;
                UIManager.Instance.ShowRewardCristal(randCristal);
                UIManager.Instance.SetCristalText();
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    IEnumerator showRewarded()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";
        while (!this._rewardedAd.CanShowAd())
        {
            yield return new WaitForSeconds(0.2f);
        }
        _rewardedAd.Show((Reward reward) =>
        {
            // TODO: Reward the user.
            Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
        });
        uicanvas.sortingOrder = -1;
    }
}
