using System;
using GoogleMobileAds.Api;
using UnityEngine;
using GoogleMobileAds.Common;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager Instance;

    InterstitialAd interstitial;
    string interstitialId;

    RewardedAd rewardAd;
    string rewardId;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        RequestInterstitial();
        RequestRewardedAd();
    }

    #region //** INTERSTITIAL ADS **//
    void RequestInterstitial()
    {

#if UNITY_ANDROID
        interstitialId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_EDITOR
        interstitialId = "ca-app-pub-3940256099942544/1033173712";
#else
        interstitialId = null;
#endif

        interstitial = new InterstitialAd(interstitialId);

        //call events
        interstitial.OnAdLoaded += HandleOnInterstitialAdLoaded;
        interstitial.OnAdFailedToLoad += HandleOnInterstitialAdFailedToLoad;
        interstitial.OnAdOpening += HandleOnInterstitialAdOpened;
        interstitial.OnAdClosed += HandleOnInterstitialAdClosed;
        interstitial.OnAdLeavingApplication += HandleOnInterstitialAdLeavingApplication;


        //create and ad request
        if (PlayerPrefs.HasKey("Consent"))
        {
            AdRequest request = new AdRequest.Builder().Build();
            interstitial.LoadAd(request); //load & show the banner ad
            Debug.Log("Interstitial Ad is Ready to Show after Consent");
        }
        else
        {
            AdRequest request = new AdRequest.Builder().AddExtra("npa", "1").Build();
            interstitial.LoadAd(request); //load & show the banner ad (non-personalised)
            Debug.Log("Interstitial Ad is Ready to Show without Consent");
        }
    }

    //show the ad
    public void ShowInterstitial()
    {
        if (PlayerPrefs.HasKey("ads") == false)
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
                RequestInterstitial();
            }
        }
        else
        {
            Debug.Log("Player have bought No Ads product");
        }
    }

    //events below
    public void HandleOnInterstitialAdLoaded(object sender, EventArgs args)
    {
        Debug.LogWarning("Interstitial Ads is Loaded!");
    }

    public void HandleOnInterstitialAdFailedToLoad(object sender, EventArgs args)
    {
        Debug.Log("Interstitial Ads is Failed to Load!");
    }

    public void HandleOnInterstitialAdOpened(object sender, EventArgs args)
    {
        Debug.LogWarning("Interstitial Ads Opened!");
        RequestInterstitial();
    }

    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        Debug.LogWarning("Interstitial Ads Closed!");
    }

    public void HandleOnInterstitialAdLeavingApplication(object sender, EventArgs args)
    {
        Debug.Log("Player leave the Application while Interstitial Ad is Playing!");
    }
    #endregion

    #region //** REWARDED ADS **//
    void RequestRewardedAd()
    {
#if UNITY_ANDROID
        rewardId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_EDITOR
        rewardId = "ca-app-pub-3940256099942544/5224354917";
#else
        rewardId = null;
#endif

        rewardAd = new RewardedAd(rewardId);

        //call events
        rewardAd.OnAdLoaded += HandleRewardAdLoaded;
        rewardAd.OnAdFailedToLoad += HandleRewardAdFailedToLoad;
        rewardAd.OnAdOpening += HandleRewardAdOpening;
        rewardAd.OnAdFailedToShow += HandleRewardAdFailedToShow;
        rewardAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardAd.OnAdClosed += HandleRewardAdClosed;


        //create and ad request
        if (PlayerPrefs.HasKey("Consent"))
        {
            AdRequest request = new AdRequest.Builder().Build();
            rewardAd.LoadAd(request); //load & show the banner ad
        }
        else
        {
            AdRequest request = new AdRequest.Builder().AddExtra("npa", "1").Build();
            rewardAd.LoadAd(request); //load & show the banner ad (non-personalised)
        }
    }

    //attach to a button that plays ad if ready
    public void ShowRewardedAd()
    {
        if (rewardAd.IsLoaded())
        {
            rewardAd.Show();
            RequestRewardedAd();
        }
    }

    //call events
    public void HandleRewardAdLoaded(object sender, EventArgs args)
    {
        //do this when ad loads
    }

    public void HandleRewardAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        //do this when ad fails to loads
        Debug.Log("Ad failed to load" + args.Message);
    }

    public void HandleRewardAdOpening(object sender, EventArgs args)
    {
        //do this when ad is opening
        RequestRewardedAd();
    }

    public void HandleRewardAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        //do this when ad fails to show
    }

    public void HandleUserEarnedReward(object sender, EventArgs args)
    {
        //reward the player here
        GameManager.Instance.AddDiamonds();
        GameManager.Instance.CloseGameOverPanel();
        BallMovement.Instance.movesLeft += 5;
        GameManager.Instance.isGameOver = false;
        BallMovement.Instance.canMove = true;
    }

    public void HandleRewardAdClosed(object sender, EventArgs args)
    {
        //do this when ad is closed
        RequestRewardedAd();
    }
    #endregion
}
