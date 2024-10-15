using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCompanion : MonoBehaviour
{
    // Constants for companion IDs
    private const int ALIEN_ID = 0;
    private const int BERRY_ID = 1;
    private const int GREY_ID = 2;
    private const int WOSHI_ID = 3;

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
    public Sprite option1Image;  // Alien sprite
    public Sprite option2Image;  // Berry sprite
    public Sprite option3Image;  // Grey sprite
    public Sprite option4Image;  // Woshi sprite

    // Start is called before the first frame update
    void Start()
    {
        notificationPanel.SetActive(false);

        // Add listeners to the option buttons
        option1Button.onClick.AddListener(() => SelectOption("Option 1", option1Image, ALIEN_ID));
        option2Button.onClick.AddListener(() => SelectOption("Option 2", option2Image, BERRY_ID));
        option3Button.onClick.AddListener(() => SelectOption("Option 3", option3Image, GREY_ID));
        option4Button.onClick.AddListener(() => SelectOption("Option 4", option4Image, WOSHI_ID));

        // Add listeners to Yes and No buttons
        yesButton.onClick.AddListener(GoToMainMenu);
        noButton.onClick.AddListener(GoBackToPickStarter);
    }

    // Method to select an option and show the notification panel
    void SelectOption(string option, Sprite image, int id)
    {
        selectedOption = option;
        selectedImage = image;
        PlayerPrefs.SetInt("SelectedID", id);  // Store the companion ID
        notificationPanel.SetActive(true);  // Show notification panel
    }

    // Method to handle Yes button click - go to Main Menu
    void GoToMainMenu()
    {
        // Store the selected option and image name in PlayerPrefs
        PlayerPrefs.SetString("SelectedOption", selectedOption);
        PlayerPrefs.SetString("SelectedImage", selectedImage.name);  // Store the image name

        SceneManager.LoadScene("Main Menu");
    }

    // Method to handle No button click - return to PickStarter
    void GoBackToPickStarter()
    {
        notificationPanel.SetActive(false);  // Hide the notification panel
    }
}
