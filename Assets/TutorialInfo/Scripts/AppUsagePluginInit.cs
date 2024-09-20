using System.Collections.Generic;
using UnityEngine;
using System;

public class AppUsagePluginInit : MonoBehaviour
{
    // List of known social media package names
    private readonly Dictionary<string, string> socialMediaApps = new Dictionary<string, string>
    {
        { "com.instagram.android", "Instagram" },
        { "com.snapchat.android", "Snapchat" },
        { "com.twitter.android", "Twitter" },
        { "com.facebook.katana", "Facebook" },
        { "com.whatsapp", "WhatsApp" },
        { "com.zhiliaoapp.musically", "TikTok" },
        { "com.google.android.youtube", "YouTube" }
    };

    private AndroidJavaClass pluginClass;
    private AndroidJavaObject currentActivity;
    private bool isWaitingForPermission = false;

    void Start()
    {
        // Create an instance of the Java class
        pluginClass = new AndroidJavaClass("com.example.appusageplugin.PluginInterface");

        // Get unity current activity (pass as context)
        using (AndroidJavaObject unityActivity = new AndroidJavaObject("com.unity3d.player.UnityPlayer"))
        {
            currentActivity = unityActivity.GetStatic<AndroidJavaObject>("currentActivity");
            pluginClass.CallStatic("initialize", currentActivity);

            // Check if app has usage stats permission enabled
            CheckAndRequestPermission();
        }
    }

    void Update()
    {
        // If waiting for permission, continuously check until granted
        if (isWaitingForPermission)
        {
            bool hasPermission = pluginClass.CallStatic<bool>("checkUsagePermission");

            if (hasPermission)
            {
                Debug.Log("Usage Stats Permission Granted");
                isWaitingForPermission = false;
                RetrieveAndDisplayUsageStats();
            }
        }
    }

    void CheckAndRequestPermission()
    {
        bool hasPermission = pluginClass.CallStatic<bool>("checkUsagePermission");
        Debug.Log($"Usage Stats Permission Granted: {hasPermission}");

        if (!hasPermission)
        {
            pluginClass.CallStatic("requestUsageStatsPermission");
            Debug.Log("Requesting Usage Stats Permission...");
            isWaitingForPermission = true;  // Start checking in the Update method
        }
        else
        {
            RetrieveAndDisplayUsageStats();
        }
    }

    void RetrieveAndDisplayUsageStats()
    {
        // Retrieve usage stats if permission is granted
        using (AndroidJavaObject usageStatsList = pluginClass.CallStatic<AndroidJavaObject>("getUsageStats"))
        {
            if (usageStatsList == null)
            {
                Debug.LogError("Failed to retrieve usage stats list.");
                return;
            }

            // Convert the Java list to a C# List
            AndroidJavaObject[] usageStatsArray = usageStatsList.Call<AndroidJavaObject[]>("toArray");
            if (usageStatsArray == null)
            {
                Debug.LogError("Failed to convert usage stats list to array.");
                return;
            }

            List<UsageStat> usageStats = new();

            foreach (AndroidJavaObject usageStat in usageStatsArray)
            {
                if (usageStat == null)
                {
                    Debug.LogWarning("Encountered a null usageStat object.");
                    continue;
                }

                string packageName = usageStat.Call<string>("getPackageName");
                long lastTimeUsed = usageStat.Call<long>("getLastTimeUsed");
                long totalTimeInForeground = usageStat.Call<long>("getTotalTimeInForeground");

                if (socialMediaApps.ContainsKey(packageName))
                {
                    usageStats.Add(new UsageStat
                    {
                        PackageName = packageName,
                        LastTimeUsed = lastTimeUsed,
                        TotalTimeInForeground = totalTimeInForeground
                    });
                }
            }

            DisplayUsageStats(usageStats);
        }
    }

    void DisplayUsageStats(List<UsageStat> usageStats)
    {
        if (usageStats == null || usageStats.Count == 0)
        {
            Debug.Log("No usage stats available.");
            return;
        }

        foreach (var stat in usageStats)
        {
            string appName = socialMediaApps.ContainsKey(stat.PackageName)
                ? socialMediaApps[stat.PackageName]
                : stat.PackageName;

            TimeSpan timeSpent = TimeSpan.FromMilliseconds(stat.TotalTimeInForeground);
            string formattedTimeSpent = $"{timeSpent.Hours}h {timeSpent.Minutes}m {timeSpent.Seconds}s";

            Debug.Log($"App: {appName}, Package: {stat.PackageName}, Time Spent: {formattedTimeSpent}");
        }
    }
}
