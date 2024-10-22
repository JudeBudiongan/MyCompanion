using UnityEngine;
using UnityEngine.UI;
using static CompanionManager;

public class MainMenuController : MonoBehaviour
{
    // Reference to the CompanionManager
    private CompanionManager companionManager;

    // Reference to the UI Text component to display the selected option
    public Text selectedOptionText;

    // Reference to the Image component to display the selected companion's image
    public Image selectedOptionImage;

    // Reference to the GameDataSaver
    private GameDataSaver gameDataSaver;

    // Start is called before the first frame update
    void Start()
    {
        // Get the CompanionManager instance
        companionManager = CompanionManager.Instance;
        
        // Find GameDataSaver in the scene
        gameDataSaver = FindObjectOfType<GameDataSaver>();
        if (gameDataSaver == null)
        {
            Debug.LogError("GameDataSaver not found in the scene.");
            return; // Exit if GameDataSaver is not found
        }

        // Load companion data from PlayerPrefs
        gameDataSaver.LoadGameData();

        // Retrieve the selected ID from PlayerPrefs
        int selectedID = PlayerPrefs.GetInt("SelectedID", -1); // Default to -1 if not found
        Debug.Log($"Selected ID retrieved: {selectedID}");

        // Change the text and image based on the selected companion
        if (selectedID >= 0 && selectedID < companionManager.companions.Count) // Use the count of companions
        {
            var selectedCompanion = companionManager.GetCompanionById(selectedID);
            if (selectedCompanion != null)
            {
                selectedOptionImage.sprite = selectedCompanion.CompanionSprite; // Assuming this property exists
                selectedOptionText.text = selectedCompanion.PetName; // Assuming this property exists

                // Update the companion's emotion based on satisfaction level
                UpdateCompanionEmotion(selectedCompanion); // Pass the whole companion object

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
                ClearSelectedOption(); // Clear if something goes wrong
                Debug.Log("No companion found for the selected ID.");
            }
        }
        else
        {
            ClearSelectedOption(); // Clear if ID is invalid
            Debug.Log("Invalid selected ID.");
        }

        // Set the image to active and center it in the middle of the screen
        selectedOptionImage.gameObject.SetActive(true);
    }

    private void ClearSelectedOption()
    {
        selectedOptionImage.sprite = null;  // Clear the image if something goes wrong
        selectedOptionText.text = "No option selected"; // Reset text
    }

    // Method to update the companion's emotion based on satisfaction level
    private void UpdateCompanionEmotion(Companion companion)
    {
        if (companion.SatisfactionLevel < 10)
        {
            selectedOptionImage.sprite = companion.AngrySprite; // Set to angry sprite
            Debug.Log("Companion is angry.");
        }
        else if (companion.SatisfactionLevel < 50)
        {
            selectedOptionImage.sprite = companion.SadSprite; // Set to sad sprite
            Debug.Log("Companion is sad.");
        }
        else if (companion.SatisfactionLevel < 80)
        {
            selectedOptionImage.sprite = companion.NormalSprite; // Set to normal sprite
            Debug.Log("Companion is normal.");
        }
        else
        {
            selectedOptionImage.sprite = companion.HappySprite; // Set to happy sprite
            Debug.Log("Companion is happy.");
        }
    }
}
