using UnityEngine;
using static CompanionManager;

public class CoinManager : MonoBehaviour
{
    public int TotalCoins { get; private set; } = 1000; // Initialize total coins to 0
    private CompanionManager companionManager; // Reference to CompanionManager
    public static CoinManager Instance;
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        companionManager = FindObjectOfType<CompanionManager>();
        Debug.Log($"Initial Total Coins: {TotalCoins}");
    }

    // Method to deduct coins
    public void DeductCoins(float amount)
    {
        if (TotalCoins >= amount)
        {
            TotalCoins -= (int)amount; // Deduct coins and cast to int
            Debug.Log($"Deducted {amount} coins. Remaining: {TotalCoins}");
        }
        else
        {
            Debug.LogWarning("Not enough coins to deduct.");
        }
    }

    public void EarnCoinsForLevelUp(Companion companion)
    {
        TotalCoins += 10; // Earn coins for leveling up
        Debug.Log($"Earned 10 coins for leveling up {companion.PetName} to level {companion.Level}.");

        if (companion.Level == 10)
        {
            TotalCoins += 50; // Bonus for reaching level 10
            Debug.Log($"Earned an additional 50 coins for {companion.PetName} reaching level 10!");
        }

        CheckCompanionMilestones();
    }

    private void CheckCompanionMilestones()
    {
        int level5Count = 0;
        int level10Count = 0;

        foreach (var companion in companionManager.companions)
        {
            if (companion.Level >= 5)
                level5Count++;
            if (companion.Level >= 10)
                level10Count++;
        }

        if (level5Count >= 5)
        {
            TotalCoins += 50; // Earn coins for 5 companions reaching level 5
            Debug.Log("Earned 50 coins for 5 companions reaching level 5!");
        }

        if (level10Count >= 5)
        {
            TotalCoins += 100; // Earn coins for 5 companions reaching level 10
            Debug.Log("Earned 100 coins for 5 companions reaching level 10!");
        }
    }
}
