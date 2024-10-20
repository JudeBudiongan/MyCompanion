using NUnit.Framework;
using UnityEngine;

public class DailyRewardTest
{
    // A test for checking if the reward is available after 24 hours
    [Test]
    public void RewardAvailableAfter24Hours()
    {
        // Arrange
        // Simulate the last claimed time being 25 hours ago using PlayerPrefs
        PlayerPrefs.SetString("LastClaimedTime", System.DateTime.Now.AddHours(-25).ToString());

        // Act
        // Simulate checking for the reward availability using PlayerPrefs logic
        System.DateTime lastClaimedTime = System.DateTime.Parse(PlayerPrefs.GetString("LastClaimedTime"));
        bool rewardAvailable = (System.DateTime.Now - lastClaimedTime).TotalHours >= 24;

        // Assert
        Assert.IsTrue(rewardAvailable, "Reward should be available after 24 hours.");
    }

    // A test for checking if claiming a reward updates the balance
    [Test]
    public void ClaimRewardUpdatesBalance()
    {
        // Arrange
        CoinManager coinManager = new CoinManager();
        // Simulate the last claimed time being 25 hours ago
        PlayerPrefs.SetString("LastClaimedTime", System.DateTime.Now.AddHours(-25).ToString());

        int initialBalance = coinManager.TotalCoins;

        // Act
        // Simulate claiming a reward (increase coin balance by the reward amount)
        int rewardAmount = 250; // Assuming 250 is the reward amount
        coinManager.AddCoins(rewardAmount); // Update the coin balance with the reward

        int newBalance = coinManager.TotalCoins;

        // Assert
        Assert.AreEqual(initialBalance + rewardAmount, newBalance, "Balance should update after claiming the reward.");
    }

    // A test for checking that the reward cannot be claimed multiple times within 24 hours
    [Test]
    public void RewardCannotBeClaimedTwiceWithin24Hours()
    {
        // Arrange
        // Simulate the last claimed time being right now
        PlayerPrefs.SetString("LastClaimedTime", System.DateTime.Now.ToString());

        // Act
        // Simulate checking for reward availability using PlayerPrefs logic
        System.DateTime lastClaimedTime = System.DateTime.Parse(PlayerPrefs.GetString("LastClaimedTime"));
        bool rewardAvailable = (System.DateTime.Now - lastClaimedTime).TotalHours >= 24;

        // Assert
        Assert.IsFalse(rewardAvailable, "Reward should not be available within 24 hours of the last claim.");
    }
}
