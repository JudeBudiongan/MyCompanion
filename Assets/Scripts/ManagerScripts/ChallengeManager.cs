using UnityEngine;
using TMPro;

public class ChallengeManager : MonoBehaviour
{
    public CompanionManager companionManager;
    public CatalogueManager catalogueManager;  // Reference to CatalogueManager
    public GameObject notCompleteIcon;
    public GameObject completeIcon;
    public TextMeshProUGUI challengeText;

    private int targetCompanionCount = 3;
    private bool isChallengeCompleted = false;

    void Start()
    {
        // Initialize UI
        notCompleteIcon.SetActive(true);
        completeIcon.SetActive(false);
        UpdateChallengeText();

        // Subscribe to companion added event
        if (companionManager != null)
        {
            companionManager.OnCompanionAdded += CheckChallengeCompletion;
        }

        // Initial check for challenge status
        CheckChallengeCompletion();
    }

    private void UpdateChallengeText()
    {
        if (isChallengeCompleted)
        {
            challengeText.text = "Challenge Completed: Bing unlocked!";
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
        completeIcon.SetActive(true);
        UpdateChallengeText();

        // Unlock "Bing" companion
        CompanionManager.Companion bingCompanion = new CompanionManager.Companion(15, "Bing", companionManager.spriteBing, "JG");
        if (!companionManager.companions.Contains(bingCompanion))
        {
            companionManager.companions.Add(bingCompanion);
        }

        Debug.Log("Special companion 'Bing' unlocked for buying more than 3 companions!");
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
