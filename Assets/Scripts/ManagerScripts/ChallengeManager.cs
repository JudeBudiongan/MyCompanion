using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{
    public CompanionManager companionManager;
    public CatalogueManager catalogueManager;  // Reference to CatalogueManager
    public GameObject notCompleteIcon;
    public TextMeshProUGUI challengeText;
    public Button claimButton;  // Reference to the claim button
    public TextMeshProUGUI claimButtonText;  // Reference to the "Claim" button text

    private int targetCompanionCount = 3;
    private bool isChallengeCompleted = false;

    void Awake() {
        if (companionManager == null)
        {
            companionManager = FindObjectOfType<CompanionManager>();
        }
    }


void Start()
{
    // If not assigned in the inspector, find CompanionManager in the scene
    if (companionManager == null)
    {
        companionManager = CompanionManager.Instance;
    }

    // Ensure the instance exists
    if (companionManager == null)
    {
        Debug.LogError("CompanionManager is missing. Make sure it's properly set in the scene.");
        return;
    }

    // Initialize UI
    notCompleteIcon.SetActive(true);
    claimButton.gameObject.SetActive(false);  // Hide the claim button initially
    UpdateChallengeText();

    // Subscribe to companion added event
    companionManager.OnCompanionAdded += CheckChallengeCompletion;

    // Initial check for challenge status
    CheckChallengeCompletion();

    // Add listener to the claim button
    claimButton.onClick.AddListener(OnClaimButtonClicked);
}


    private void UpdateChallengeText()
    {
        if (isChallengeCompleted)
        {
            challengeText.text = "Challenge Completed: Bing unlocked! Click 'Claim' to add Bing.";
        }
        else
        {
            int remainingCompanions = targetCompanionCount - catalogueManager.GetBoughtCompanionCount();
            challengeText.text = $"Buy {remainingCompanions} more companions to earn 'Bing'";
        }
    }

    private void CheckChallengeCompletion()
    {
        int boughtCount = catalogueManager.GetBoughtCompanionCount();
        if (!isChallengeCompleted && boughtCount >= targetCompanionCount)
        {
            CompleteChallenge();
        }
        else
        {
            UpdateChallengeText();  // Update the UI if not completed
        }
    }

    private void CompleteChallenge()
    {
        isChallengeCompleted = true;

        // Update UI
        notCompleteIcon.SetActive(false);
        claimButton.gameObject.SetActive(true);  // Show the claim button
        claimButtonText.text = "Claim Bing";  // Set the button text
        UpdateChallengeText();

        Debug.Log("Special companion 'Bing' unlocked for buying more than 3 companions! Click 'Claim' to add Bing.");
    }

    private void OnClaimButtonClicked()
    {
        // Unlock "Bing" companion when the claim button is pressed
        CompanionManager.Companion bingCompanion = companionManager.GetCompanionById(15);
        if (bingCompanion == null)
        {
            bingCompanion = new CompanionManager.Companion(15, "Bing", companionManager.spriteBing, "JG");
            companionManager.companions.Add(bingCompanion);
        }

        // Set Bing as bought so it shows up in the catalog
        bingCompanion.IsBought = true;
        Debug.Log("Bing has been claimed and marked as bought!");

        // Update catalogue to show new companion
        catalogueManager.UpdateCatalogueUI();

        // Hide the claim button after claiming
        claimButton.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        if (companionManager != null)
        {
            companionManager.OnCompanionAdded -= CheckChallengeCompletion;
        }
    }
}
