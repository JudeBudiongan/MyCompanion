using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUsageDown : MonoBehaviour
{
    public static CheckUsageDown Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep this object across scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }
//
    public void CheckDailyUsage(AndroidNotifications androidNotifications, List<string> SocialMediaPackages)
    {
        // Initialize the Android plugin
        AndroidJavaClass pluginClass = new AndroidJavaClass("com.example.appusageplugin.PluginInterface");

        // Get Unity's current activity (pass as context)
        using (AndroidJavaObject unityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject currentActivity = unityActivity.GetStatic<AndroidJavaObject>("currentActivity");
            pluginClass.CallStatic("initialize", currentActivity);

            // Check if app has usage stats permission enabled
            bool hasPermission = pluginClass.CallStatic<bool>("checkUsagePermission");
            Debug.Log($"Usage Stats Permission Granted: {hasPermission}");

            if (!hasPermission)
            {
                pluginClass.CallStatic("requestUsageStatsPermission");
                Debug.Log("Requesting Usage Stats Permission...");
                return;
            }

            // Retrieve usage stats for yesterday and the day before
            AndroidJavaObject usageStatsYesterday = pluginClass.CallStatic<AndroidJavaObject>("getUsageStatsForDay", 8);  // Yesterday
            AndroidJavaObject usageStatsDayBefore = pluginClass.CallStatic<AndroidJavaObject>("getUsageStatsForDay", 0);  // Day before yesterday

            if (usageStatsYesterday == null || usageStatsDayBefore == null)
            {
                Debug.LogError("Failed to retrieve usage stats.");
                return;
            }

            // Calculate total usage for both days
            long usageYesterday = GetTotalUsageForDay(usageStatsYesterday, SocialMediaPackages);
            long usageDayBefore = GetTotalUsageForDay(usageStatsDayBefore, SocialMediaPackages);

            // Compare usage and send notification if usage was lower yesterday
            if (usageYesterday < usageDayBefore)
            {
                long timeSavedMillis = usageDayBefore - usageYesterday;
                TimeSpan timeSaved = TimeSpan.FromMilliseconds(timeSavedMillis);
                string savedTimeFormatted = string.Format("{0}h {1}m", timeSaved.Hours, timeSaved.Minutes);

                // Send the notification
                string notificationMessage = $"Great job! Your social media usage was down by {savedTimeFormatted} yesterday.";
                androidNotifications.SendNotification("Your pet is feeling healthier!", notificationMessage, 5);
            } else {
                Debug.Log("Social media usage wasn't reduced yesterday...");
            }
        }
    }

    private long GetTotalUsageForDay(AndroidJavaObject usageStats, List<string> socialMediaPackages)
    {
        long totalUsage = 0;

        AndroidJavaObject[] usageStatsArray = usageStats.Call<AndroidJavaObject[]>("toArray");
        foreach (AndroidJavaObject usageStat in usageStatsArray)
        {
            string packageName = usageStat.Call<string>("getPackageName");

            if (socialMediaPackages.Contains(packageName))
            {
                long usageTime = usageStat.Call<long>("getTotalTimeInForeground");
                totalUsage += usageTime;
            }
        }
        return totalUsage;
    }
}
