using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AppUsagePluginInit : MonoBehaviour
{
    public Text usageStatsText;

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
        usageStats.Sort((x, y) => y.TotalTimeInForeground.CompareTo(x.TotalTimeInForeground));

        // Build the display string
        string displayText = "";
        foreach (var stat in usageStats)
        {
            string appName = socialMediaApps.ContainsKey(stat.PackageName)
                ? socialMediaApps[stat.PackageName]
                : stat.PackageName;

            TimeSpan timeSpent = TimeSpan.FromMilliseconds(stat.TotalTimeInForeground);
            string formattedTimeSpent = $"{timeSpent.Hours}h {timeSpent.Minutes}m {timeSpent.Seconds}s";

            Debug.Log($"App: {appName}, Package: {stat.PackageName}, Time Spent: {formattedTimeSpent}");

            displayText += $"App: {appName}, Time Spent: {formattedTimeSpent}\n";
        }

        usageStatsText.text = displayText;
    }
}
