using UnityEngine;
using static CompanionManager;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int TotalCoins { get; private set; } = 1000; // Initialize total coins
    private CompanionManager companionManager; // Reference to CompanionManager
    public static CoinManager Instance;

    [SerializeField]
    private TextMeshProUGUI coinText;

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
        UpdateCoinText(); // Update the UI at the start
        Debug.Log($"Initial Total Coins: {TotalCoins}");
    }

    void OnEnable()
    {
        UpdateCoinText(); // Ensure the coin text is updated when the object becomes active
    }

    public void SetCoinTextReference(TextMeshProUGUI text)
    {
        coinText = text;
        UpdateCoinText();
    }

    // Method to add coins (for daily rewards)
    public void AddCoins(int amount)
    {
        TotalCoins += amount;
        Debug.Log($"Added {amount} coins. Total now: {TotalCoins}");
        UpdateCoinText(); // Update the UI
    }

    // Method to deduct coins
    public void DeductCoins(float amount)
    {
        if (TotalCoins >= amount)
        {
            TotalCoins -= (int)amount; // Deduct coins and cast to int
            Debug.Log($"Deducted {amount} coins. Remaining: {TotalCoins}");
            UpdateCoinText(); // Update the UI
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
        UpdateCoinText(); // Update the UI

        if (companion.Level == 10)
        {
            TotalCoins += 50; // Bonus for reaching level 10
            Debug.Log($"Earned an additional 50 coins for {companion.PetName} reaching level 10!");
            UpdateCoinText(); // Update the UI
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
            UpdateCoinText(); // Update the UI
        }

        if (level10Count >= 5)
        {
            TotalCoins += 100; // Earn coins for 5 companions reaching level 10
            Debug.Log("Earned 100 coins for 5 companions reaching level 10!");
            UpdateCoinText(); // Update the UI
        }
    }

    // Method to update the UI text
    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = TotalCoins.ToString("0000"); // Format to display coins as "0000"
        }
    }
}
