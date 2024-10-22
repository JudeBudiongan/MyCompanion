using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SatisfactionRegression : MonoBehaviour
{
    public static SatisfactionRegression Instance { get; private set; }

    private int decreaseAmount = 10; // Amount to decrease satisfaction by
    private float decreaseInterval = 60f; // Time in seconds between decreases
    private CompanionManager.Companion selectedCompanion;
    private int selectedCompanionId;
    private long lastCheckTime; // Timestamp of the last time social media usage was checked
    private long previousSocialMediaTime; // Stores the total social media time from previous checks

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
        // Check if CompanionManager.Instance is null
        if (CompanionManager.Instance == null)
        {
            Debug.LogError("CompanionManager.Instance is null!");
            return; // Early exit to prevent further errors
        }

        // Get the currently selected companion
        selectedCompanionId = PlayerPrefs.GetInt("SelectedID");
        selectedCompanion = CompanionManager.Instance.GetCompanionById(selectedCompanionId); // Replace with the actual ID of the selected companion

        // Check if the selected companion is null
        if (selectedCompanion == null)
        {
            Debug.LogError($"No companion found with ID: {selectedCompanionId}");
            return; // Early exit to prevent further errors
        }

        lastCheckTime = GetUnixTimestampMilliseconds(); // Set the initial last check time to current time
        previousSocialMediaTime = 0; // Initialize previous social media time to 0
        StartCoroutine(CheckSocialMediaUsage());
    }

    IEnumerator CheckSocialMediaUsage()
    {
        while (true)
        {
            yield return new WaitForSeconds(decreaseInterval);

            if (selectedCompanion != null)
            {
                long currentCheckTime = GetUnixTimestampMilliseconds(); // Get the current timestamp

                // Fetch cumulative social media usage up to the current time
                long totalSocialMediaTime = FetchSocialMediaUsageSinceLastCheck(lastCheckTime, currentCheckTime);

                // Calculate the difference in social media usage since the last check
                long newSocialMediaTime = totalSocialMediaTime - previousSocialMediaTime;

                // Log the newly accumulated time spent on social media
                TimeSpan socialMediaTimeSpan = TimeSpan.FromMilliseconds(newSocialMediaTime);
                string formattedTime = $"{socialMediaTimeSpan.Hours}h {socialMediaTimeSpan.Minutes}m {socialMediaTimeSpan.Seconds}s";
                Debug.Log($"New social media usage since last check: {formattedTime}");

                // Decrease the companion's satisfaction based on the new social media usage
                int decreaseRate = (int)Math.Round(newSocialMediaTime / (60 * 60 * 1000.0)); // Set rate to 1 hour
                selectedCompanion.DecreaseSatisfaction(decreaseAmount * decreaseRate); // Decrease amount per hour
                CompanionManager.Instance.SaveCompanionData(selectedCompanion); // Save the updated satisfaction level
                Debug.Log($"{selectedCompanion.PetName}'s satisfaction decreased to {selectedCompanion.SatisfactionLevel}.");

                // Update the last check time and previous social media time
                lastCheckTime = currentCheckTime;
                previousSocialMediaTime = totalSocialMediaTime; // Store the cumulative time for the next check
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
