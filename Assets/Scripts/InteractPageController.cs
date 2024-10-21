using UnityEngine;
using UnityEngine.UI;
using static CompanionManager;

public class InteractPageController : MonoBehaviour
{
    // Reference to the CompanionManager
    private CompanionManager companionManager;

    // Reference to the UI Text component to display the companion's name
    public Text companionNameText;

    // Reference to the Image component to display the companion's image
    public Image companionImage;

    // Reference to the UI Text component to display the companion's emotional state
    public Text companionEmotionText;

    // Private variable to store the current companion's ID
    private int currentCompanionID = -1;

    // Start is called before the first frame update
    void Start()
    {
        // Get the CompanionManager instance
        companionManager = CompanionManager.Instance;

        // Automatically show the companion info on the interact page
        ShowCompanionInfo();
    }

    // Method to display the companion's image and name on the interact page
    void ShowCompanionInfo()
    {
        // Retrieve the selected ID from PlayerPrefs
        int selectedID = PlayerPrefs.GetInt("SelectedID", -1); // Default to -1 if not found

        // If the selected ID has changed, update the companion info
        if (selectedID != currentCompanionID)
        {
            currentCompanionID = selectedID;
            Debug.Log($"Selected ID retrieved: {currentCompanionID}");

            // Fetch the companion's details and update the UI
            if (currentCompanionID >= 0 && currentCompanionID < companionManager.companions.Count)
            {
                var currentCompanion = companionManager.GetCompanionById(currentCompanionID);

                if (currentCompanion != null)
                {
                    // Update the UI with the companion's details
                    companionImage.sprite = currentCompanion.CompanionSprite; // Assuming this property exists
                    companionNameText.text = currentCompanion.PetName; // Assuming this property exists

                    // Update the companion's emotion based on satisfaction level
                    UpdateCompanionEmotion(currentCompanion);

                    // Debug log to confirm sprite and name
                    Debug.Log($"Companion Loaded: {currentCompanion.PetName}");
                }
                else
                {
                    ClearCompanionInfo();
                    Debug.Log("No companion found for the selected ID.");
                }
            }
            else
            {
                ClearCompanionInfo();
                Debug.Log("Invalid selected ID.");
            }
        }
    }

    // Method to clear companion info when no companion is found
    private void ClearCompanionInfo()
    {
        companionImage.sprite = null;
        companionNameText.text = "No companion selected";
        companionEmotionText.text = ""; // Clear emotion text
        companionImage.gameObject.SetActive(true); // Ensure the image is visible
    }

    // Method to update the companion's image based on their emotional state
    private void UpdateCompanionEmotion(Companion companion)
    {
        string emotionMessage = ""; // To store the emotion message

        // Check satisfaction level and update emotion
        if (companion.SatisfactionLevel < 10)
        {
            companionImage.sprite = companion.AngrySprite; // Set to angry sprite
            emotionMessage = "Your Companion is angry!";
            Debug.Log("Companion is angry.");
        }
        else if (companion.SatisfactionLevel < 50)
        {
            companionImage.sprite = companion.SadSprite; // Set to sad sprite
            emotionMessage = "Your Companion is sad!";
            Debug.Log("Companion is sad.");
        }
        else if (companion.SatisfactionLevel < 80)
        {
            companionImage.sprite = companion.NormalSprite; // Set to normal sprite
            emotionMessage = "Your Companion is feeling normal.";
            Debug.Log("Companion is normal.");
        }
        else
        {
            companionImage.sprite = companion.HappySprite; // Set to happy sprite
            emotionMessage = "Your Companion is happy!";
            Debug.Log("Companion is happy.");
        }

        // Update the emotion text on the UI
        companionEmotionText.text = emotionMessage;
    }

    // Update is called once per frame to check companion emotion in real-time
    void Update()
    {
        ShowCompanionInfo(); // Continuously check and update companion info
    }
}
