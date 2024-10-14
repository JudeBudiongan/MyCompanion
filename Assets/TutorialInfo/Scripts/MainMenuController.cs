using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
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
        // Retrieve the selected option from PlayerPrefs
        string selectedOption = PlayerPrefs.GetString("SelectedOption", "No option selected");
        string selectedImageName = PlayerPrefs.GetString("SelectedImage", "default");

        // Change the text based on the selected companion
        switch (selectedImageName)
        {
            case "alien-normal":  // Make sure the names match the sprite names
                selectedOptionImage.sprite = option1Image;
                selectedOptionText.text = "You have selected: Alien";
                break;
            case "berry-normal":
                selectedOptionImage.sprite = option2Image;
                selectedOptionText.text = "You have selected: Berry";
                break;
            case "grey-normal":
                selectedOptionImage.sprite = option3Image;
                selectedOptionText.text = "You have selected: Grey";
                break;
            case "woshi-normal":
                selectedOptionImage.sprite = option4Image;
                selectedOptionText.text = "You have selected: Woshi";
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
