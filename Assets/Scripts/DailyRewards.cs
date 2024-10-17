using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class DailyRewards : MonoBehaviour
{
    public CoinManager coinManager;  // Reference to CoinManager
    public GameObject rewardUIPanel; // The UI panel for displaying rewards
    public GameObject noMoreRewardsPanel; // Panel to show when no rewards available
    public TextMeshProUGUI rewardAmountText; // Text for reward amount
    public Button claimButton; // Button for claiming rewards
    public Button closeButton; // Button for closing the reward panel
    public Button openButton; // Button for opening the rewards UI panel
    public Image notificationIcon; // Notification icon to indicate availability of reward

    private DateTime lastClaimedTime;
    private bool rewardAvailable;
    private const int DAILY_REWARD_AMOUNT = 250; // Reward amount
    private const int REWARD_INTERVAL_HOURS = 23; // Reward interval

    void Start()
    {
        // Load last claimed time
        if (PlayerPrefs.HasKey("LastClaimedTime"))
        {
            lastClaimedTime = DateTime.Parse(PlayerPrefs.GetString("LastClaimedTime"));
        }
        else
        {
            lastClaimedTime = DateTime.MinValue;
        }

        UpdateRewardAvailability();

        // Add listeners to the buttons
        claimButton.onClick.AddListener(ClaimReward);
        closeButton.onClick.AddListener(CloseRewardPanel);
        openButton.onClick.AddListener(ShowRewardsUI); // Add listener for the open button
    }

    void Update()
    {
        UpdateRewardAvailability();
    }

    // Method to check if the reward is available based on last claimed time
    private void UpdateRewardAvailability()
    {
        TimeSpan timeSinceLastClaim = DateTime.Now - lastClaimedTime;
        rewardAvailable = timeSinceLastClaim.TotalHours >= REWARD_INTERVAL_HOURS;

        if (rewardAvailable)
        {
            notificationIcon.enabled = true;
            rewardUIPanel.SetActive(false);
        }
        else
        {
            notificationIcon.enabled = false;
            noMoreRewardsPanel.SetActive(true);
        }
    }

    // Method to claim the reward
    public void ClaimReward()
    {
        Debug.Log("ClaimReward button clicked");
        if (rewardAvailable)
        {
            lastClaimedTime = DateTime.Now;
            PlayerPrefs.SetString("LastClaimedTime", lastClaimedTime.ToString());
            PlayerPrefs.Save();

            coinManager.AddCoins(DAILY_REWARD_AMOUNT); // Update coin balance using CoinManager
            rewardAmountText.text = $"+{DAILY_REWARD_AMOUNT}";

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
        rewardUIPanel.SetActive(false);
    }

    // Method to show the rewards UI panel
    public void ShowRewardsUI()
    {
        Debug.Log("ShowRewardsUI button clicked");
        rewardUIPanel.SetActive(true);
        noMoreRewardsPanel.SetActive(!rewardAvailable);
    }
}
