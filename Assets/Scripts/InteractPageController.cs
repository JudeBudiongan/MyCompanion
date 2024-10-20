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
        Debug.Log($"Selected ID retrieved: {selectedID}");

        // Fetch the companion's details and update the UI
        if (selectedID >= 0 && selectedID < companionManager.companions.Count) // Use companions.Count for safety
        {
            var selectedCompanion = companionManager.GetCompanionById(selectedID);
            if (selectedCompanion != null)
            {
                companionImage.sprite = selectedCompanion.CompanionSprite; // Assuming this property exists
                companionNameText.text = selectedCompanion.PetName; // Assuming this property exists

                // Update the companion's emotion based on satisfaction level
                UpdateCompanionEmotion(selectedCompanion); // Pass the whole companion object

                // Debug log to confirm sprite and name
                if (companionImage.sprite != null)
                {
                    Debug.Log($"Sprite Loaded: {companionImage.sprite.name}");
                }
                else
                {
                    Debug.LogError("Sprite is null. Check if the companion has a valid sprite assigned.");
                }

                Debug.Log($"Companion Loaded: {selectedCompanion.PetName}");
            }
            else
            {
                companionImage.sprite = null;
                companionNameText.text = "No companion selected";
                Debug.Log("No companion found for the selected ID.");
            }
        }
        else
        {
            companionImage.sprite = null;
            companionNameText.text = "No companion selected";
            Debug.Log("Invalid selected ID.");
        }

        // Set the image to active and make it visible on the interact page
        companionImage.gameObject.SetActive(true);
    }

    // Method to update the companion's image based on their emotional state
    private void UpdateCompanionEmotion(Companion companion)
    {
        if (companion.SatisfactionLevel < 10)
        {
            companionImage.sprite = companion.AngrySprite; // Set to angry sprite
            Debug.Log("Companion is angry.");
        }
        else if (companion.SatisfactionLevel < 50)
        {
            companionImage.sprite = companion.SadSprite; // Set to sad sprite
            Debug.Log("Companion is sad.");
        }
        else if (companion.SatisfactionLevel < 80)
        {
            companionImage.sprite = companion.NormalSprite; // Set to normal sprite
            Debug.Log("Companion is normal.");
        }
        else
        {
            companionImage.sprite = companion.HappySprite; // Set to happy sprite
            Debug.Log("Companion is happy.");
        }
    }
}
