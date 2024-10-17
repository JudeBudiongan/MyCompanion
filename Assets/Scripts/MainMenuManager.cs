using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public CoinManager coinManager;  // Reference to CoinManager
    [SerializeField]
    private TextMeshProUGUI coinText; // Reference to the UI Text component for displaying coins

    void Awake() {
        if (coinManager == null)
        {
            coinManager = FindObjectOfType<CoinManager>();
        }
    }

    void Start()
    {
        if (coinManager != null)
        {
            coinManager.SetCoinTextReference(coinText);
        }
        UpdateCoinDisplay();
    }

    void OnEnable()
    {
        if (coinManager == null)
        {
            coinManager = FindObjectOfType<CoinManager>();
        }
        if (coinManager != null)
        {
            coinManager.SetCoinTextReference(coinText);
            UpdateCoinDisplay(); // Ensure the coin text is updated when the object becomes active
        }
    }

    private void UpdateCoinDisplay()
    {
        if (coinManager != null && coinText != null)
        {
            coinText.text = "Coins: " + coinManager.TotalCoins.ToString("0000"); // Synchronize with CoinManager
        }
    }
}
