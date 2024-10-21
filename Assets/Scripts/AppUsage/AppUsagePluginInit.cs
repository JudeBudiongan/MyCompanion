using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AppUsagePluginInit : MonoBehaviour
{
    public GameObject appUsageItemPrefab; // Reference to your prefab
    public Transform contentPanel; // Reference to the Content panel of the Scroll View

    void Start()
    {
        // Create an instance of the Java class
        AndroidJavaClass pluginClass = new AndroidJavaClass("com.example.appusageplugin.PluginInterface");

        // Get unity current activity (pass as context)
        using (AndroidJavaObject unityActivity = new AndroidJavaObject("com.unity3d.player.UnityPlayer"))
        {
            // Initialize the plugin with Unity's context
            AndroidJavaObject currentActivity = unityActivity.GetStatic<AndroidJavaObject>("currentActivity");
            pluginClass.CallStatic("initialize", currentActivity);

            // Check if app has usage stats permission enabled
            bool hasPermission = pluginClass.CallStatic<bool>("checkUsagePermission");
            Debug.Log($"Usage Stats Permission Granted: {hasPermission}");

            if (!hasPermission)
            {
                pluginClass.CallStatic("requestUsageStatsPermission");
                Debug.Log("Requesting Usage Stats Permission...");
            }
            else
            {
                // Retrieve usage stats if permission is granted
                using (AndroidJavaObject usageStatsList = pluginClass.CallStatic<AndroidJavaObject>("getUsageStatsToday"))
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

                    Dictionary<string, UsageStat> usageStats = new();

                    long totalSocialMediaTime = 0;

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

                        if (SocialMediaApps.socialMediaApps.ContainsKey(packageName))
                        {
                            totalSocialMediaTime += totalTimeInForeground;
                            if (usageStats.ContainsKey(packageName))
                            {
                                usageStats[packageName].TotalTimeUsed += totalTimeInForeground; // Update time if duplicate entry encountered
                            }
                            else
                            {
                                usageStats[packageName] = new UsageStat
                                {
                                    PackageName = packageName,
                                    LastTimeUsed = lastTimeUsed,
                                    TotalTimeUsed = totalTimeInForeground
                                };
                            }
                        }
                    }

                    DisplayUsageStats(new List<UsageStat>(usageStats.Values));
                    DisplayTotalUsageStats(totalSocialMediaTime);
                }
            }
        }
    }

    void DisplayUsageStats(List<UsageStat> usageStats)
    {
        if (usageStats == null || usageStats.Count == 0)
        {
            Debug.Log("No usage stats available.");
            return;
        }

        // Sort the list in descending order of time spent
        usageStats.Sort((x, y) => y.TotalTimeUsed.CompareTo(x.TotalTimeUsed));

        // Clear existing items in the content panel
        foreach (Transform child in contentPanel)
        {
            if (child.name != "TotalUsageText")
            {
                Destroy(child.gameObject);
            }
        }

        foreach (var stat in usageStats)
        {
            GameObject newItem = Instantiate(appUsageItemPrefab, contentPanel);
            var appNameText = newItem.transform.Find("AppName").GetComponent<Text>();
            var timeUsedText = newItem.transform.Find("TimeUsed").GetComponent<Text>();

            string appName = SocialMediaApps.socialMediaApps[stat.PackageName];
            TimeSpan timeUsed = TimeSpan.FromMilliseconds(stat.TotalTimeUsed);
            string formattedTimeUsed = $"{timeUsed.Hours}h {timeUsed.Minutes}m {timeUsed.Seconds}s";
            Debug.Log($"App: {appName}, Time Spent: {formattedTimeUsed}");

            appNameText.text = appName;
            timeUsedText.text = formattedTimeUsed;
        }
    }

    void DisplayTotalUsageStats(long totalTime)
    {
        TimeSpan totalUsageTime = TimeSpan.FromMilliseconds(totalTime);
        string formattedTime = $"{totalUsageTime.Hours}h {totalUsageTime.Minutes}m {totalUsageTime.Seconds}s";

        Debug.Log($"Total social media Usage for today: {formattedTime}");

        var totalUsageText = contentPanel.Find("TotalUsageText").GetComponent<Text>();
        if (totalUsageText != null)
        {
            totalUsageText.text = $"Total Time Today: {formattedTime}";
        }
    }
}
