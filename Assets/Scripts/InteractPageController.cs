using UnityEngine;
using UnityEngine.UI;

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
        if (selectedID >= 0 && selectedID < 15) // Adjust this limit based on your companion IDs
        {
            var selectedCompanion = companionManager.GetCompanionById(selectedID);
            if (selectedCompanion != null)
            {
                companionImage.sprite = selectedCompanion.CompanionSprite; // Assuming this property exists
                companionNameText.text = selectedCompanion.PetName; // Assuming this property exists

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
}
