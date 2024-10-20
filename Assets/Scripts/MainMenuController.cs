using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // Constants for companion IDs
    private const int ALIEN_ID = 0;
    private const int BERRY_ID = 1;
    private const int GREY_ID = 2;
    private const int WOSHI_ID = 3;

    // Reference to the UI Text component to display the selected option
    public Text selectedOptionText;

    // Reference to the Image component to display the selected companion's image
    public Image selectedOptionImage;

    // List of sprites for each companion (ensure the images are correctly assigned in the Inspector)
    public Sprite option1Image;  // Alien sprite
    public Sprite option2Image;  // Berry sprite
    public Sprite option3Image;  // Grey sprite
    public Sprite option4Image;  // Woshi sprite

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the selected option and ID from PlayerPrefs
        string selectedOption = PlayerPrefs.GetString("SelectedOption", "No option selected");
        string selectedImageName = PlayerPrefs.GetString("SelectedImage", "default");
        int selectedID = PlayerPrefs.GetInt("SelectedID", -1); // Default to -1 if not found

        // Change the text and image based on the selected companion
        switch (selectedID)
        {
            case ALIEN_ID:
                selectedOptionImage.sprite = option1Image;
                selectedOptionText.text = "Alien";
                break;
            case BERRY_ID:
                selectedOptionImage.sprite = option2Image;
                selectedOptionText.text = "Berry";
                break;
            case GREY_ID:
                selectedOptionImage.sprite = option3Image;
                selectedOptionText.text = "Grey";
                break;
            case WOSHI_ID:
                selectedOptionImage.sprite = option4Image;
                selectedOptionText.text = "Woshi";
                break;
            default:
                selectedOptionImage.sprite = null;  // No image if something goes wrong
                selectedOptionText.text = "No option selected";
                break;
        }

        // Set the image to active and center it in the middle of the screen
        selectedOptionImage.gameObject.SetActive(true);
    }
}
