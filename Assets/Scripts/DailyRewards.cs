using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DailyRewards : MonoBehaviour
{
    public GameObject rewardUIPanel;     // Main Rewards UI Panel (DarkPanel)
    public GameObject rewardsUI;         // Rewards UI Panel (the one showing rewards)
    public GameObject noMoreRewardsUI;   // Panel to show when no rewards are available
    public GameObject notificationIcon;  // Notification icon
    public Button claimButton;           // Claim reward button
    public Button closeButton;           // Close button inside rewards panel
    public Button openButton;            // Rewards button (opens rewards panel)
    public TextMeshProUGUI rewardAmountText; // Updated to TextMeshProUGUI
    public CoinManager coinManager;      // Reference to the CoinManager
    private bool rewardAvailable;        // Is the reward available?
    private const int DAILY_REWARD_AMOUNT = 250; // Amount of coins for daily reward

    void Start()
    {
        // Load the last claimed time and check if the reward is available
        CheckDailyRewardStatus();

        // Initially hide the entire DarkPanel (rewards UI panel)
        rewardUIPanel.SetActive(false);

        // Add listeners to the buttons
        claimButton.onClick.AddListener(ClaimReward);
        closeButton.onClick.AddListener(CloseRewardPanel);
        openButton.onClick.AddListener(ShowRewardsUI); // Add listener for the open button

        // Update UI based on saved reward availability status
        UpdateRewardAvailability();
    }

    void Update()
    {
        // This can be used for future logic updates related to rewards if needed
    }

    // Method to check if the reward can be claimed (based on daily reset)
    private void CheckDailyRewardStatus()
    {
        // Load the last claimed time
        string lastClaimedTimeStr = PlayerPrefs.GetString("LastClaimedTime", string.Empty);

        if (string.IsNullOrEmpty(lastClaimedTimeStr))
        {
            // If there is no saved claim time, allow the reward to be claimed
            rewardAvailable = true;
        }
        else
        {
            // Convert the saved string to DateTime
            DateTime lastClaimedTime = DateTime.Parse(lastClaimedTimeStr);
            DateTime currentTime = DateTime.Now;

            // Check if it's a new day
            if (currentTime.Date > lastClaimedTime.Date)
            {
                rewardAvailable = true;  // Reward is available if it's a new day
            }
            else
            {
                rewardAvailable = false; // Reward has already been claimed today
            }
        }

        PlayerPrefs.SetInt("RewardAvailable", rewardAvailable ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Method to update the UI based on the reward availability
    private void UpdateRewardAvailability()
    {
        if (rewardAvailable)
        {
            notificationIcon.SetActive(true);  // Show notification if reward is available
            rewardsUI.SetActive(true);         // Show rewards UI when reward is available
            noMoreRewardsUI.SetActive(false);  // Hide the "No More Rewards" panel
        }
        else
        {
            notificationIcon.SetActive(false);  // Hide notification when no reward is available
            rewardsUI.SetActive(false);         // Hide rewards UI
            noMoreRewardsUI.SetActive(true);    // Show "No More Rewards" panel
        }
    }

    // Method to claim the reward
    public void ClaimReward()
    {
        Debug.Log("ClaimReward button clicked");
        if (rewardAvailable)
        {
            // Disable reward after it's claimed
            rewardAvailable = false;

            // Save the current date and time as the last claim time
            PlayerPrefs.SetString("LastClaimedTime", DateTime.Now.ToString());
            PlayerPrefs.SetInt("RewardAvailable", 0); // Mark reward as claimed
            PlayerPrefs.Save();

            // Add coins to the player
            coinManager.AddCoins(DAILY_REWARD_AMOUNT); // Update coin balance using CoinManager
            rewardAmountText.text = $"+{DAILY_REWARD_AMOUNT}";

            // Update the availability UI after claiming the reward
            UpdateRewardAvailability();
        }
        else
        {
            Debug.Log("Reward not available yet.");
        }
    }

    // Method to close the reward panel
    public void CloseRewardPanel()
    {
        Debug.Log("CloseRewardPanel button clicked");
        rewardUIPanel.SetActive(false);  // Close the whole DarkPanel
    }

    // Method to show the rewards UI panel
    public void ShowRewardsUI()
    {
        Debug.Log("ShowRewardsUI button clicked");

        // Show the DarkPanel when the user clicks the Rewards button
        rewardUIPanel.SetActive(true);  

        if (rewardAvailable)
        {
            rewardsUI.SetActive(true);         // Show the rewards UI if reward is available
            noMoreRewardsUI.SetActive(false);  // Hide the "No More Rewards" panel
        }
        else
        {
            rewardsUI.SetActive(false);        // Hide the rewards UI if no rewards are available
            noMoreRewardsUI.SetActive(true);   // Show the "No More Rewards" panel
        }
    }
}
