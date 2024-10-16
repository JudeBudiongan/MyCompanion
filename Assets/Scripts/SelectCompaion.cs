using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCompanion : MonoBehaviour
{
    // Reference to the notification panel
    public GameObject notificationPanel;

    // References to the buttons for options 1, 2, 3, and 4
    public Button[] optionButtons; // Array of option buttons

    // Reference to the Yes and No buttons
    public Button yesButton;
    public Button noButton;

    // Variables to store the selected option
    private string selectedOption;
    private Sprite selectedImage; // Sprite for the selected companion's image

    // Reference to CompanionManager
    private CompanionManager companionManager;

    void Start()
    {
        notificationPanel.SetActive(false);

        // Get the CompanionManager instance
        companionManager = CompanionManager.Instance;

        // Set up buttons dynamically based on the companions in the CompanionManager
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // Local copy for closure
            optionButtons[i].onClick.AddListener(() => SelectOption(companionManager.GetCompanionById(index).PetName, companionManager.GetCompanionById(index).CompanionSprite, index));
        }

        // Add listeners to Yes and No buttons
        yesButton.onClick.AddListener(GoToMainMenu);
        noButton.onClick.AddListener(GoBackToPickStarter);
    }

    // Method to select an option and show the notification panel
    void SelectOption(string option, Sprite image, int id)
    {
        selectedOption = option;
        selectedImage = image;
        PlayerPrefs.SetInt("SelectedID", id); // Store the companion ID
        notificationPanel.SetActive(true); // Show notification panel
    }

    // Method to handle Yes button click - go to Main Menu
    void GoToMainMenu()
    {
        // Store the selected option and image name in PlayerPrefs
        PlayerPrefs.SetString("SelectedOption", selectedOption);
        PlayerPrefs.SetString("SelectedImage", selectedImage.name); // Store the image name

        // Mark the chosen starter companion as "bought"
        int selectedID = PlayerPrefs.GetInt("SelectedID");
        companionManager.SetCompanionBought(selectedID);

        // Load the Main Menu scene
        SceneManager.LoadScene("Main Menu");
    }

    // Method to handle No button click - return to PickStarter
    void GoBackToPickStarter()
    {
        notificationPanel.SetActive(false); // Hide the notification panel
    }
}
