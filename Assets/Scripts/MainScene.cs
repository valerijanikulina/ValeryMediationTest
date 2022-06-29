using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public GameObject rewardedScreen;
    public GameObject noRewardScreen;

    void Start()
    {
        rewardedScreen.SetActive(false);
        GameObject.FindGameObjectWithTag("Music").GetComponent<FirstScene>().PlayMusic(); 
    }
    
    public void OnCloseRewardedScreenClicked()
    {
        rewardedScreen.SetActive(false);
    }

    public void OnCloseNoRewardScreenClicked()
    {
        noRewardScreen.SetActive(false);
    }
}
