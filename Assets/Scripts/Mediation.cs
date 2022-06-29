using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Mediation;

public class Mediation : MonoBehaviour
{
    public GameObject interstitialShowButton;
    public GameObject intertitialLoadButton;
    public GameObject rewardedLoadButton;
    public GameObject rewardedShowButton;

    //interstitial ad
    IInterstitialAd interstitialAd;
    public string interstitialAdUnitId = "Interstitial_Android";

    //rewarded ad
    IRewardedAd rewardedAd;
    public string rewardedAdUnitId = "Rewarded_Android";
    public GameObject rewardedUI;
    public GameObject noRewardUI;
    public Text rewardText;
    int reward;

    void Start()
    {
        interstitialShowButton.SetActive(false);
        intertitialLoadButton.SetActive(false);
        rewardedLoadButton.SetActive(false);
        rewardedShowButton.SetActive(false);
        rewardedUI.SetActive(false);
        noRewardUI.SetActive(false);
        reward = 0;
        rewardText.text = reward.ToString();
    }

    //Initialize Mediation
    public async void OnMediationInitClick()
    {
        try
        {
            await UnityServices.InitializeAsync();
            InitializationComplete();
        }
        catch (Exception e)
        {
            InitializationFailed(e);
        }
    }

    void InitializationComplete()
    {
        interstitialShowButton.SetActive(true);
        intertitialLoadButton.SetActive(true);
        rewardedLoadButton.SetActive(true);
        rewardedShowButton.SetActive(true);
        Debug.Log("Initialization Completed");

        interstitialAd = MediationService.Instance.CreateInterstitialAd(interstitialAdUnitId);
        InterstitialSetup();

        rewardedAd = MediationService.Instance.CreateRewardedAd(rewardedAdUnitId);
        RewardedSetup();
    }

    void InitializationFailed(Exception e)
    {
        Debug.Log("Initialization Failed: " + e.Message);
    }

    //Intestitial ads
    public void OnLoadIntersitialAdClick()
    {
        interstitialAd.Load();
    }

    public async void OnShowIntersitialAdClick()
    {
        if (interstitialAd.AdState == AdState.Loaded)
        {
            try
            {
                InterstitialAdShowOptions showOptions = new InterstitialAdShowOptions();
                showOptions.AutoReload = false;
                await interstitialAd.ShowAsync(showOptions);
                InterstitialAdShown();
            }
            catch (ShowFailedException e)
            {
                InterstitialAdFailedShow(e);
            }
        }
    }

    public void InterstitialSetup()
    {
        interstitialAd.OnClosed += InterstitialAdClosed;
        interstitialAd.OnClicked += InterstitialAdClicked;
        interstitialAd.OnLoaded += InterstitialAdLoaded;
        interstitialAd.OnFailedLoad += InterstitialAdFailedLoad;
    }

    void InterstitialAdLoaded(object sender, EventArgs e)
    {
        Debug.Log("Ad loaded");
    }

    void InterstitialAdFailedLoad(object sender, LoadErrorEventArgs e)
    {
        Debug.Log("Failed to load ad");
        Debug.Log(e.Message);
    }
        
    void InterstitialAdShown()
    {
        Debug.Log("Ad shown!");
    }
        
    void InterstitialAdClosed(object sender, EventArgs e)
    {
        Debug.Log("Ad has closed");
    }

    void InterstitialAdClicked(object sender, EventArgs e)
    {
        Debug.Log("Ad has been clicked");
    }
        
    void InterstitialAdFailedShow(ShowFailedException e)
    {
        Debug.Log(e.Message);
    }

    //Rewarded ads
    public void OnLoadRewardedAdClick()
    {
        rewardedAd.Load();
    }

    public void OnShowRewardedAdClick()
    {
        if (rewardedAd.AdState == AdState.Loaded)
        {
            rewardedAd.Show();
            RewardedAdShown();
        }
    }

    void RewardedSetup()
    {
        rewardedAd.OnClosed += RewardedAdClosed;
        rewardedAd.OnClicked += RewardedAdClicked;
        rewardedAd.OnLoaded += RewardedAdLoaded;
        rewardedAd.OnFailedLoad += RewardedAdFailedLoad;
        rewardedAd.OnUserRewarded += RewardedUserRewarded;
    }

    void RewardedAdShown()
    {
        Debug.Log("Rewarded Ad shown!");
    }
    
    void RewardedAdFailedShow(ShowFailedException e)
    {
        Debug.Log(e.Message);
    }

    void RewardedAdClosed(object sender, EventArgs e)
    {
        Debug.Log("Rewarded Ad has closed");
        noRewardUI.SetActive(true);
    }

    void RewardedAdClicked(object sender, EventArgs e)
    {
        Debug.Log("Rewarded Ad has been clicked");
    }

    void RewardedAdLoaded(object sender, EventArgs e)
    {
        Debug.Log("Rewarded Ad loaded");
    }

    void RewardedAdFailedLoad(object sender, LoadErrorEventArgs e)
    {
        Debug.Log("Failed to load rewarded ad");
        Debug.Log(e.Message);
    }

    void RewardedUserRewarded(object sender, RewardEventArgs e)
    {
        noRewardUI.SetActive(false);
        rewardedUI.SetActive(true);
        Debug.Log("Received reward of 10");
        AddConcurrency();
    }

    void AddConcurrency()
    {
        reward += 10;
        rewardText.text = reward.ToString();
    }
}