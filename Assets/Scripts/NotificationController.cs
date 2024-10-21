using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;
using System;

public class NotificationController : MonoBehaviour
{    // List of social media packages

    public static List<string> SocialMediaPackages = new List<string>
    {
        "com.facebook.katana",  // Facebook
        "com.instagram.android",  // Instagram
        "com.twitter.android",  // Twitter
        "com.snapchat.android",
        "com.whatsapp",
        "com.zhiliaoapp.musically",
        "com.google.android.youtube"
    };

    private const float delayBeforeStartingService = 3f; // Delay in seconds

    // Start is called before the first frame update
    private void Start()
    {
        AndroidJavaClass pluginClass = new AndroidJavaClass("com.example.appusageplugin.PluginInterface");

        // Get unity current activity (pass as context)
        using (AndroidJavaObject unityActivity = new AndroidJavaObject("com.unity3d.player.UnityPlayer"))
        {
            // Initialize the plugin with Unity's context
            AndroidJavaObject currentActivity = unityActivity.GetStatic<AndroidJavaObject>("currentActivity");
            pluginClass.CallStatic("initialize", currentActivity);

            bool hasPermission = pluginClass.CallStatic<bool>("checkUsagePermission");
            Debug.Log($"Usage Stats Permission Granted: {hasPermission}");

            RequestNotificationPermission();
            RequestUsagePermission(hasPermission, pluginClass);

            // Start the coroutine to delay the service start
            StartCoroutine(StartUsageMonitoringServiceAfterDelay(pluginClass, currentActivity));
        }
    }


    private IEnumerator StartUsageMonitoringServiceAfterDelay(AndroidJavaClass pluginClass, AndroidJavaObject currentActivity)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeStartingService);

        // Start the usage monitoring service
        pluginClass.CallStatic("startUsageMonitoringService", currentActivity);
    }

    void RequestUsagePermission(Boolean hasPermission, AndroidJavaClass pluginClass)
    {
        if (!hasPermission)
        {
            Debug.Log("Requesting Usage Stats Permission...");
            pluginClass.CallStatic("requestUsageStatsPermission");
        }
    }

    void RequestNotificationPermission()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }
}
