using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCompanion : MonoBehaviour
{
    // Reference to the notification panel
    public GameObject notificationPanel;

    // References to the buttons for options 1, 2, 3, and 4
    public Button option1Button;
    public Button option2Button;
    public Button option3Button;
    public Button option4Button;

    // Reference to the Yes and No buttons
    public Button yesButton;
    public Button noButton;

    // Variables to store the selected option and image
    private string selectedOption;
    private Sprite selectedImage;  // Sprite for the selected companion's image

    // References to the companion images for each option
    public Sprite option1Image;
    public Sprite option2Image;
    public Sprite option3Image;
    public Sprite option4Image;

    // Start is called before the first frame update
    void Start()
    {
        notificationPanel.SetActive(false);

        // Add listeners to the option buttons
        option1Button.onClick.AddListener(() => SelectOption("Option 1", option1Image));
        option2Button.onClick.AddListener(() => SelectOption("Option 2", option2Image));
        option3Button.onClick.AddListener(() => SelectOption("Option 3", option3Image));
        option4Button.onClick.AddListener(() => SelectOption("Option 4", option4Image));

        // Add listeners to Yes and No buttons
        yesButton.onClick.AddListener(GoToMainMenu);
        noButton.onClick.AddListener(GoBackToPickStarter);
    }

    // Method to select an option and show the notification panel
    void SelectOption(string option, Sprite image)
    {
        selectedOption = option;
        selectedImage = image;
        notificationPanel.SetActive(true);  // Show notification panel
    }

    // Method to handle Yes button click - go to Main Menu
    void GoToMainMenu()
    {
        if (selectedImage != null) // Ensure selectedImage is not null
        {
            PlayerPrefs.SetString("SelectedOption", selectedOption);
            PlayerPrefs.SetString("SelectedImage", selectedImage.name);  // Store the image name
        }
        else
        {
            Debug.LogWarning("Selected image is null!"); // Log a warning for debugging
        }

        SceneManager.LoadScene("Main Menu");
    }

    // Method to handle No button click - return to PickStarter
    void GoBackToPickStarter()
    {
        notificationPanel.SetActive(false);  // Hide the notification panel
    }
}
