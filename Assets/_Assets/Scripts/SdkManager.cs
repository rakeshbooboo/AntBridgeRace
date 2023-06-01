using System.Collections.Generic;
using Facebook.Unity;
using GameAnalyticsSDK;
using UnityEngine;
using AppsFlyerSDK;
public class SdkManager : MonoBehaviour {
    internal static SdkManager Instance { get; set; }

    private void Awake () {
        if (Instance != null) {
            Destroy (gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad (gameObject);
            InitializeFB();
        }
    }

    // Start is called before the first frame update
    void Start () {
        if (Application.isEditor == false) {
            GameAnalytics.Initialize ();
            // InitializeAppsFlyer ();
        }
    }

    void InitializeFB()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(FBInitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
    private void FBInitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isUnityShown)
    {
    }

    //Call method like this in other scripts
    //    SdkManager.LevelSuccessEvent(curLevel, curScore);
    internal static void LevelSuccessEvent(int level, int score)
    {
        if (Application.isEditor == false)
        {
            // Dictionary<string, string> sucessEvent = new Dictionary<string, string>
            // {
            //     { "Level", level.ToString() },
            //     { "Score", score.ToString() }
            // };
            // AppsFlyer.sendEvent("success", sucessEvent);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level_" + level);
            // KWCore.Umbrella.GameCore.ProgressManager.CompleteStage();
        }


        if (Application.isEditor)
        {
            print("Success : Level_" + level + " Score_" + score);
        }
    }
    internal static void LevelFailEvent(int level, int score)
    {
        if (Application.isEditor == false)
        {
            // Dictionary<string, string> failureEvent = new Dictionary<string, string>
            // {
            //     { "Level", level.ToString() },
            //     { "Score", score.ToString() }
            // };
            // AppsFlyer.sendEvent("fail", failureEvent);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level_" + level);
            // KWCore.Umbrella.GameCore.ProgressManager.FailStage();
        }

        print("Fail : Level_" + level + " Score_" + score);
    }

    internal static void LevelStartEvent(int level)
    {
        if (Application.isEditor == false)
        {
            Dictionary<string, string> startEvent = new Dictionary<string, string>
            {
                { "Level", level.ToString() }
            };
            // AppsFlyer.sendEvent("start", startEvent);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level_" + level);
            // KWCore.Umbrella.GameCore.ProgressManager.StartStage();
        }

        if (Application.isEditor)
        {
            print("Start : Level_" + level);
        }
    }

    internal static void ShopOpenedEvent(int level){
        if(Application.isEditor == false){
            // GameAnalytics.NewDesignEvent("UI:Shop:Level"+level.ToString());//(GAProgressionStatus.Start, level.ToString());
        }
        if(Application.isEditor){
            print("Shop Opened : Level_" + level);
        }
    }
    void InitializeAppsFlyer()
    {
        if (Application.isEditor == false)
        {
            string devKey = "Ugf2FWT7Hc5L4TQHpm2SyT";
#if UNITY_IOS
             AppsFlyer.initSDK(devKey, "");
#elif UNITY_ANDROID
            AppsFlyer.initSDK(devKey, null);
#endif
            AppsFlyer.startSDK();
        }
    }
}