using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public static AdsManager Instance;

    #region VARIABLES
    string gameId_Android = "4462143";
    [HideInInspector] public string interstitial_Android = "Interstitial_Android";
    [HideInInspector] public string rewarded_Android = "Rewarded_Android";
    bool testMode = false;
    #endregion

    void Awake()
    {
        if (Instance == null)
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
        // Initialize the Ads service:
        Advertisement.Initialize(gameId_Android, testMode);

        Advertisement.AddListener(this);
    }

    #region UNITY ADS
    public void ShowInterstitialAd() // Show Skipable Interstitial Ad
    {
        //Check if Interstetial Ad is Loaded and Ready to Show
        if (Advertisement.IsReady(interstitial_Android))
        {
            //Check if Player hasn't bought "NO ADS" IAP
            if (PlayerPrefs.HasKey("ads") == false)
            {
                //Start showing Interstitial Ad
                Advertisement.Show(interstitial_Android);
            }
        }
    }

    public void ShowRewardedAd() // Show Rewarded Ad
    {
        //Check if Rewarded Ad is Loaded and Ready to Show
        if (Advertisement.IsReady(rewarded_Android))
        {
            //Start showing Rewarded Ad
            Advertisement.Show(rewarded_Android);
        }
        else
        {
            Debug.Log("*Rewarded Ad is not Ready*");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Rewarded Ad is Ready");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Rewarded Ad Started");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogWarning("ERROR: " + message);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == "Rewarded_Android" && showResult == ShowResult.Finished)
        {
            //Reward Player
            if (GameManager.Instance.isGameOver == true)
            {
                //reward Player for watching revive Ad
                GameManager.Instance.AddDiamonds();
                GameManager.Instance.CloseGameOverPanel();
                BallMovement.Instance.movesLeft += 5;
                GameManager.Instance.isGameOver = false;
                BallMovement.Instance.canMove = true;
            }
            else
            {
                //reward Player for watching restore game Ad
                SettingsMenu.Instance.ClosePausePanel();
                SettingsMenu.Instance.CloseCautionPanel();
                GameManager.Instance.RestoreGame2();
            }
        }
        else if (placementId == "Rewarded_Android" && showResult == ShowResult.Skipped)
        {
            //Don't Reward Player
            Debug.Log("Player SKIPPED the AD!");
        }
        else if (placementId == "Rewarded_Android" && showResult == ShowResult.Failed)
        {
            Debug.LogError("The Ad didn't finish due to an error");
        }
    }
    #endregion
}
