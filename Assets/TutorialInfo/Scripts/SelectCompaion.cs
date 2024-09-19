using UnityEngine;
using UnityEngine.UI; // Import UI for buttons and panels
using UnityEngine.SceneManagement; // Import SceneManagement to load different scenes

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

    // Start is called before the first frame update
    void Start()
    {
        // Make sure the notification panel is hidden at the start
        notificationPanel.SetActive(false);

        // Add listener methods to the option buttons
        option1Button.onClick.AddListener(ShowNotification);
        option2Button.onClick.AddListener(ShowNotification);
        option3Button.onClick.AddListener(ShowNotification);
        option4Button.onClick.AddListener(ShowNotification);

        // Add listener methods to Yes and No buttons
        yesButton.onClick.AddListener(GoToMainMenu);
        noButton.onClick.AddListener(GoBackToPickStarter);

        // Ensure Yes and No buttons are initially interactable
        yesButton.interactable = true;
        noButton.interactable = true;
    }

    // Method to show the notification panel
    void ShowNotification()
    {
        notificationPanel.SetActive(true);
        // Optionally disable option buttons to prevent further interaction until decision
        option1Button.interactable = false;
        option2Button.interactable = false;
        option3Button.interactable = false;
        option4Button.interactable = false;
    }

    // Method to handle Yes button click - go to Main Menu
    void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Replace "Main Menu" with the actual scene name
    }

    // Method to handle No button click - return to PickStarter
    void GoBackToPickStarter()
    {
        SceneManager.LoadScene("PickStarter"); // Replace "PickStarter" with the actual scene name
    }
}
