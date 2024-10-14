using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // Reference to the UI Text component to display the selected option
    public Text selectedOptionText;

    // Reference to the Image component to display the selected companion's image
    public Image selectedOptionImage;

    // List of sprites for each companion (ensure the images are correctly assigned in the Inspector)
    public Sprite option1Image;
    public Sprite option2Image;
    public Sprite option3Image;
    public Sprite option4Image;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the selected option from PlayerPrefs
        string selectedOption = PlayerPrefs.GetString("SelectedOption", "No option selected");
        string selectedImageName = PlayerPrefs.GetString("SelectedImage", "default");

        // Update the Text component with the chosen option
        selectedOptionText.text = "You have selected: " + selectedOption;

        // Load the corresponding image based on the name stored in PlayerPrefs
        switch (selectedImageName)
        {
            case "alien-normal":  // Make sure the names match the sprite names
                selectedOptionImage.sprite = option1Image;
                break;
            case "berry-normal":
                selectedOptionImage.sprite = option2Image;
                break;
            case "grey-normal":
                selectedOptionImage.sprite = option3Image;
                break;
            case "woshi-normal":
                selectedOptionImage.sprite = option4Image;
                break;
            default:
                selectedOptionImage.sprite = null;  // No image if something goes wrong
                break;
        }

        // Set the image to active and center it in the middle of the screen
        selectedOptionImage.gameObject.SetActive(true);
    }
}
