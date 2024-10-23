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
    private bool isClaimed = false; // Flag to check if the claim button has already been clicked

    void Awake()
    {
        if (companionManager == null)
        {
            companionManager = FindObjectOfType<CompanionManager>();
        }

        // Load the claim state from PlayerPrefs
        isClaimed = PlayerPrefs.GetInt("IsBingClaimed", 0) == 1;
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

        // Set up the UI based on whether the challenge was claimed
        notCompleteIcon.SetActive(true);
        claimButton.gameObject.SetActive(false); // Hide the claim button initially

        UpdateChallengeText();

        // If already claimed, hide the button and update UI
        if (isClaimed)
        {
            claimButton.gameObject.SetActive(false); // Already claimed, so hide the button
            challengeText.text = "Bing has already been claimed!";
        }
        else
        {
            claimButton.onClick.AddListener(OnClaimButtonClicked);
        }
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

        // Save the claimed state to PlayerPrefs
        PlayerPrefs.SetInt("IsBingClaimed", 1);
        PlayerPrefs.Save();

        
    }

    void OnDestroy()
    {
        if (companionManager != null)
        {
            companionManager.OnCompanionAdded -= CheckChallengeCompletion;
        }
    }
}