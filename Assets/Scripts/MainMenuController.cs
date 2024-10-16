using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // Reference to the CompanionManager
    private CompanionManager companionManager;

    // Reference to the UI Text component to display the selected option
    public Text selectedOptionText;

    // Reference to the Image component to display the selected companion's image
    public Image selectedOptionImage;

    // Start is called before the first frame update
    void Start()
    {
        // Get the CompanionManager instance
        companionManager = CompanionManager.Instance;

        // Retrieve the selected ID from PlayerPrefs
        int selectedID = PlayerPrefs.GetInt("SelectedID", -1); // Default to -1 if not found
        Debug.Log($"Selected ID retrieved: {selectedID}");

        // Change the text and image based on the selected companion
        if (selectedID >= 0 && selectedID < 12) // Adjust this limit based on your companion IDs
        {
            var selectedCompanion = companionManager.GetCompanionById(selectedID);
            if (selectedCompanion != null)
            {
                selectedOptionImage.sprite = selectedCompanion.CompanionSprite; // Assuming this property exists
                selectedOptionText.text = selectedCompanion.PetName; // Assuming this property exists

                // Debug log to confirm sprite loading
                if (selectedOptionImage.sprite != null)
                {
                    Debug.Log($"Sprite Loaded: {selectedOptionImage.sprite.name}");
                }
                else
                {
                    Debug.LogError("Sprite is null. Check if the companion has a valid sprite assigned.");
                }

                Debug.Log($"Companion Loaded: {selectedCompanion.PetName}");
            }
            else
            {
                selectedOptionImage.sprite = null;  // No image if something goes wrong
                selectedOptionText.text = "No option selected";
                Debug.Log("No companion found for the selected ID.");
            }
        }
        else
        {
            selectedOptionImage.sprite = null;  // No image if ID is invalid
            selectedOptionText.text = "No option selected";
            Debug.Log("Invalid selected ID.");
        }

        // Set the image to active and center it in the middle of the screen
        selectedOptionImage.gameObject.SetActive(true);
    }
}
