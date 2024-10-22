using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DailyRewards : MonoBehaviour
{
    public Button claimButton;
    public GameObject notificationIcon;
    public TextMeshProUGUI rewardAmountText;
    public CoinManager coinManager; // Reference to the CoinManager
    private bool rewardAvailable;
    private const int DAILY_REWARD_AMOUNT = 250;

    void Awake()
    {
        StartCoroutine(DelayedInitialization()); // Start delayed initialization
    }

    // Delayed initialization to find the CoinManager
    IEnumerator DelayedInitialization()
    {
        yield return new WaitForSeconds(0.1f); // Small delay
        coinManager = FindObjectOfType<CoinManager>();

        if (coinManager == null)
        {
            Debug.LogError("CoinManager not found! Please ensure it exists in the scene.");
        }
        else
        {
            Debug.Log("CoinManager found.");
        }

        InitializeDailyRewards(); // Proceed with initialization once CoinManager is found
    }

    void InitializeDailyRewards()
    {
        // Load the last claimed time and check if the reward is available
        CheckDailyRewardStatus();

        // Add listener to the claim button
        claimButton.onClick.AddListener(ClaimReward);

        // Update button interactability and notification based on reward availability
        UpdateRewardButtonAndNotification();
    }

    void Update()
    {
        CheckIfRewardAvailableAgain();
    }

    private void CheckDailyRewardStatus()
    {
        string lastClaimedTimeStr = PlayerPrefs.GetString("LastClaimedTime", string.Empty);

        if (string.IsNullOrEmpty(lastClaimedTimeStr))
        {
            rewardAvailable = true;
        }
        else
        {
            var lastClaimedTime = System.DateTime.Parse(lastClaimedTimeStr);
            var currentTime = System.DateTime.Now;

            rewardAvailable = (currentTime - lastClaimedTime).TotalHours >= 24;
        }

        PlayerPrefs.SetInt("RewardAvailable", rewardAvailable ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void UpdateRewardButtonAndNotification()
    {
        claimButton.interactable = rewardAvailable;
        notificationIcon.SetActive(rewardAvailable);

        if (rewardAvailable)
        {
            rewardAmountText.text = $"CLAIM +{DAILY_REWARD_AMOUNT}";
        }
        else
        {
            rewardAmountText.text = "Come back in 24 hours!";
        }
    }

    public void ClaimReward()
    {
        if (rewardAvailable)
        {
            rewardAvailable = false;

            PlayerPrefs.SetString("LastClaimedTime", System.DateTime.Now.ToString());
            PlayerPrefs.SetInt("RewardAvailable", 0);
            PlayerPrefs.Save();

            // Use the CoinManager instance
            if (coinManager != null)
            {
                coinManager.IncreaseCoins(DAILY_REWARD_AMOUNT);
                coinManager.SaveCoins();
            }

            rewardAmountText.text = $"+{DAILY_REWARD_AMOUNT} coins claimed!";
            UpdateRewardButtonAndNotification();
        }
        else
        {
            Debug.Log("Reward not available yet.");
        }
    }

    private void CheckIfRewardAvailableAgain()
    {
        if (!rewardAvailable)
        {
            string lastClaimedTimeStr = PlayerPrefs.GetString("LastClaimedTime", string.Empty);
            if (!string.IsNullOrEmpty(lastClaimedTimeStr))
            {
                var lastClaimedTime = System.DateTime.Parse(lastClaimedTimeStr);
                var currentTime = System.DateTime.Now;

                if ((currentTime - lastClaimedTime).TotalHours >= 23) //this checks every 24 hr
                {
                    rewardAvailable = true;
                    UpdateRewardButtonAndNotification();
                }
            }
        }
    }
}
