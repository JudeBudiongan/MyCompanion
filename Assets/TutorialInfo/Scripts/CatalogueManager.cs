using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatalogueManager : MonoBehaviour
{
    // Array of GameObjects representing each companion's slot in the catalog
    public GameObject[] companionSlots;

    // List of sprites for each companion (ensure the images are correctly assigned in the Inspector)
    public Sprite option1Image;  // Alien sprite
    public Sprite option2Image;  // Berry sprite
    public Sprite option3Image;  // Grey sprite
    public Sprite option4Image;  // Woshi sprite

    // Array of GameObjects representing the black borders for each companion
    public GameObject[] companionBorders;

    // Constants for companion IDs
    private const int ALIEN_ID = 0;
    private const int BERRY_ID = 1;
    private const int GREY_ID = 2;
    private const int WOSHI_ID = 3;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the selected ID from PlayerPrefs
        int selectedID = PlayerPrefs.GetInt("SelectedID", -1); // Default to -1 if not found

        // Debug log to check if selectedID is retrieved correctly
        Debug.Log($"Selected ID: {selectedID}, Slots Length: {companionSlots.Length}, Borders Length: {companionBorders.Length}");

        // Display only the selected companion in the catalog
        ShowCompanion(selectedID);
    }

    // Method to show the selected companion and hide the rest
    void ShowCompanion(int id)
    {
        Debug.Log($"ShowCompanion called with ID: {id}");

        // Hide all companion slots and borders initially
        foreach (GameObject slot in companionSlots)
        {
            slot.SetActive(false);
        }

        foreach (GameObject border in companionBorders)
        {
            border.SetActive(false);
        }

        // Check if the id is within the valid range (0 to 13)
        if (id >= 0 && id <= 13)
        {
            // Debug log for attempting to show the companion
            Debug.Log($"Attempting to show companion with ID: {id}");

            // Display the corresponding companion based on the ID and update the image and border
            companionSlots[id].SetActive(true);

            // This switch statement is safe because we've already validated the id
            switch (id)
            {
                case ALIEN_ID:
                    companionSlots[ALIEN_ID].GetComponent<Image>().sprite = option1Image;
                    companionBorders[ALIEN_ID].SetActive(true); // Activate border
                    break;
                case BERRY_ID:
                    companionSlots[BERRY_ID].GetComponent<Image>().sprite = option2Image;
                    companionBorders[BERRY_ID].SetActive(true); // Activate border
                    break;
                case GREY_ID:
                    companionSlots[GREY_ID].GetComponent<Image>().sprite = option3Image;
                    companionBorders[GREY_ID].SetActive(true); // Activate border
                    break;
                case WOSHI_ID:
                    companionSlots[WOSHI_ID].GetComponent<Image>().sprite = option4Image;
                    companionBorders[WOSHI_ID].SetActive(true); // Activate border
                    break;
                default:
                    Debug.LogWarning("Companion ID not yet assigned to an image.");
                    break;
            }
        }
        else
        {
            Debug.LogWarning($"Selected ID is out of range: {id}. Valid range is 0 to 13.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // You can add any updates here if needed
    }
}
