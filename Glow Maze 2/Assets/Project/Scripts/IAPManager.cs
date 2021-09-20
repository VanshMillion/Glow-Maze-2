using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    public static IAPManager Instance;

    private string noAdsID = "com.vanmillionstudios.glowmaze.noads";
    public Button noAdsButton;

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
        if(PlayerPrefs.HasKey("ads") == true)
        {
            noAdsButton.interactable = false;
        }
    }

    public void OnPurchaseComplete(Product product)
    {
        if(product.definition.id == noAdsID)
        {
            // Remove Ads Here
            RemoveAds();
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(product.definition.id + "failed because " + failureReason);
    }

    void RemoveAds() //Remove all Ads
    {
        if (PlayerPrefs.HasKey("ads") == false) //Check if Player hasn't bought No Ads IAP Product before
        {
            PlayerPrefs.SetInt("ads", 1); //Give Player ads key and don't show any Ad after that
            noAdsButton.interactable = false;

            Debug.Log("ADS REMOVED!");
        }
    }
}
