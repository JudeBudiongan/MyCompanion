using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{
    public CompanionManager companionManager;
    public GameObject notCompleteIcon;
    public TextMeshProUGUI challengeText;
    public Button claimButton;
    public TextMeshProUGUI claimButtonText;

    private int targetCompanionCount = 3;
    private bool isChallengeCompleted = false;
    private bool isClaimed = false; // New flag to check if the claim button has already been clicked

    void Awake()
    {
        if (companionManager == null)
        {
            companionManager = FindObjectOfType<CompanionManager>();
        }
    }

    IEnumerator DelayedInitialization()
    {
        yield return new WaitForSeconds(0.1f);
        companionManager = FindObjectOfType<CompanionManager>();

        if (companionManager != null)
        {
            companionManager.OnCompanionAdded += CheckChallengeCompletion;
            CheckChallengeCompletion();
        }
    }

    void Start()
    {
        StartCoroutine(DelayedInitialization());

        if (companionManager == null)
        {
            companionManager = CompanionManager.Instance;
        }

        if (companionManager == null)
        {
            Debug.LogError("CompanionManager is missing. Make sure it's properly set in the scene.");
            return;
        }

        notCompleteIcon.SetActive(true);
        claimButton.gameObject.SetActive(false); // Hide the claim button until the challenge is completed
        UpdateChallengeText();

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
            int remainingCompanions = targetCompanionCount - companionManager.GetBoughtCompanionsCount();
            challengeText.text = $"Buy {remainingCompanions} more companions to earn 'Bing'";
        }
    }

    private void CheckChallengeCompletion()
    {
        int boughtCount = companionManager.GetBoughtCompanionsCount();
        if (!isChallengeCompleted && boughtCount >= targetCompanionCount)
        {
            CompleteChallenge();
        }
        else
        {
            UpdateChallengeText();
        }
    }

    private void CompleteChallenge()
    {
        isChallengeCompleted = true;

        notCompleteIcon.SetActive(false);
        claimButton.gameObject.SetActive(true); // Show the claim button after the challenge is completed
        claimButtonText.text = "Claim Bing";
        UpdateChallengeText();

        Debug.Log("Special companion 'Bing' unlocked for buying more than 3 companions! Click 'Claim' to add Bing.");
    }

    private void OnClaimButtonClicked()
    {
        if (isClaimed)
        {
            return; // Prevent the button from being clicked multiple times
        }

        isClaimed = true; // Mark as claimed

        CompanionManager.Companion bingCompanion = companionManager.GetCompanionById(15);
        if (bingCompanion == null)
        {
            bingCompanion = new CompanionManager.Companion(15, "Bing", companionManager.spriteBing, "JG");
            companionManager.companions.Add(bingCompanion);
        }

        bingCompanion.IsBought = true;
        Debug.Log("Bing has been claimed and marked as bought!");

        // Level up the companion after claiming the reward
        CompanionManager.Companion companion = companionManager.GetCompanionById(0); // Select a companion to level up
        if (companion != null)
        {
            companion.LevelUp();  // Level up the companion when claimed
        }

        claimButton.gameObject.SetActive(false); // Hide the claim button after it's clicked
    }

    void OnDestroy()
    {
        if (companionManager != null)
        {
            companionManager.OnCompanionAdded -= CheckChallengeCompletion;
        }
    }
}
