using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SatisfactionRegression : MonoBehaviour
{
    public static SatisfactionRegression Instance { get; private set; }

    private int decreaseAmount = 20; // Amount to decrease satisfaction by
    private float decreaseInterval = 30f; // Time in seconds between decreases

    private CompanionManager.Companion currentCompanion;

    private long lastCheckTime; // Timestamp of the last time social media usage was checked

    private void Awake()
    {
        // Singleton implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Makes the object persist across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure there's only one instance of the script
        }
    }

    void Start()
    {
        // Get the currently selected companion
        currentCompanion = CompanionManager.Instance.GetCompanionById(0); // Replace with the actual ID of the selected companion
        lastCheckTime = GetUnixTimestampMilliseconds(); // Set the initial last check time to current time
        StartCoroutine(CheckSocialMediaUsage());
    }

    IEnumerator CheckSocialMediaUsage()
    {
        while (true)
        {
            yield return new WaitForSeconds(decreaseInterval);

            if (currentCompanion != null)
            {
                long currentCheckTime = GetUnixTimestampMilliseconds(); // Get the current timestamp

                // Fetch social media usage from the last check to now
                long socialMediaTime = FetchSocialMediaUsageSinceLastCheck(lastCheckTime, currentCheckTime);
                int decreaseRate = (int)Math.Round(socialMediaTime / (60 * 60 * 1000.0)); // Decrease amount per hour

                // Log the time spent on social media since the last check
                TimeSpan socialMediaTimeSpan = TimeSpan.FromMilliseconds(socialMediaTime);
                string formattedTime = $"{socialMediaTimeSpan.Hours}h {socialMediaTimeSpan.Minutes}m {socialMediaTimeSpan.Seconds}s";
                Debug.Log($"Social media usage since last check: {formattedTime}");

                // Decrease the companion's satisfaction based on the social media usage
                currentCompanion.DecreaseSatisfaction(decreaseAmount * decreaseRate);
                CompanionManager.Instance.SaveCompanionData(currentCompanion); // Save the updated satisfaction level
                Debug.Log($"{currentCompanion.PetName}'s satisfaction decreased to {currentCompanion.SatisfactionLevel}.");

                // Update the last check time
                lastCheckTime = currentCheckTime;
            }
        }
    }

    // Fetch social media usage between lastCheckTime and currentCheckTime
    public long FetchSocialMediaUsageSinceLastCheck(long lastCheckTime, long currentCheckTime)
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
                return 0; // Return 0 if permission is not granted
            }

            // Retrieve usage stats for the time range between lastCheckTime and currentCheckTime
            using (AndroidJavaObject usageStatsList = pluginClass.CallStatic<AndroidJavaObject>("getUsageStatsForTimeRange", lastCheckTime, currentCheckTime))
            {
                if (usageStatsList == null)
                {
                    Debug.LogError("Failed to retrieve usage stats list.");
                    return 0; // Return 0 if stats retrieval fails
                }

                // Convert the Java list to a C# List
                AndroidJavaObject[] usageStatsArray = usageStatsList.Call<AndroidJavaObject[]>("toArray");
                if (usageStatsArray == null)
                {
                    Debug.LogError("Failed to convert usage stats list to array.");
                    return 0; // Return 0 if array conversion fails
                }

                long totalSocialMediaTime = 0;

                foreach (AndroidJavaObject usageStat in usageStatsArray)
                {
                    if (usageStat == null)
                    {
                        Debug.LogWarning("Encountered a null usageStat object.");
                        continue;
                    }

                    string packageName = usageStat.Call<string>("getPackageName");
                    long totalTimeInForeground = usageStat.Call<long>("getTotalTimeInForeground");

                    if (SocialMediaApps.socialMediaApps.ContainsKey(packageName))
                    {
                        totalSocialMediaTime += totalTimeInForeground; // Accumulate time for social media apps
                    }
                }

                Debug.Log($"Total social media time since last check: {totalSocialMediaTime} milliseconds.");
                return totalSocialMediaTime; // Return total time spent on social media since last check
            }
        }
    }

    // Utility function to get the current Unix timestamp in milliseconds
    private long GetUnixTimestampMilliseconds()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
