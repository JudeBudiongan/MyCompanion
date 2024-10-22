using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static CompanionManager;

public class ChallengeManager : MonoBehaviour
{
    public CompanionManager companionManager;
    public GameObject notCompleteIcon;
    public TextMeshProUGUI challengeText;
    public Button claimButton;
    public TextMeshProUGUI claimButtonText;

    private int targetCompanionCount = 3;
    private bool isChallengeCompleted = false;

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
        claimButton.gameObject.SetActive(false);
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
            int remainingCompanions = targetCompanionCount - companionManager.GetBoughtCompanionCount();
            challengeText.text = $"Buy {remainingCompanions} more companions to earn 'Bing'";
        }
    }

    private void CheckChallengeCompletion()
    {
        int boughtCount = companionManager.GetBoughtCompanionCount();
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
        claimButton.gameObject.SetActive(true);
        claimButtonText.text = "Claim Bing";
        UpdateChallengeText();

        Debug.Log("Special companion 'Bing' unlocked for buying more than 3 companions! Click 'Claim' to add Bing.");
    }

    private void OnClaimButtonClicked()
    {
        CompanionManager.Companion bingCompanion = companionManager.GetCompanionById(15);
        if (bingCompanion == null)
        {
            bingCompanion = new CompanionManager.Companion(15, "Bing", companionManager.spriteBing, "JG");
            companionManager.companions.Add(bingCompanion);
        }

        bingCompanion.IsBought = true;
        Debug.Log("Bing has been claimed and marked as bought!");

        claimButton.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (companionManager != null)
        {
            companionManager.OnCompanionAdded -= CheckChallengeCompletion;
        }
    }
}
