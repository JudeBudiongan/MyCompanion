using System.Collections.Generic;
using UnityEngine;
using System;

public class AppUsagePluginInit : MonoBehaviour
{
    // Start is called before the first frame update
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

                        usageStats.Add(new UsageStat
                        {
                            PackageName = packageName,
                            LastTimeUsed = lastTimeUsed
                        });

                        Debug.Log($"Retrieved UsageStat: Package: {packageName}, Last Used: {DateTimeOffset.FromUnixTimeMilliseconds(lastTimeUsed).DateTime}");
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

        foreach (var stat in usageStats)
        {
            Debug.Log($"Package: {stat.PackageName}, Last Used: {DateTimeOffset.FromUnixTimeMilliseconds(stat.LastTimeUsed).DateTime}");
        }
    }
}
