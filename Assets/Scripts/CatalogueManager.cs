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
        Debug.Log("Selected ID: " + selectedID);

        // Display only the selected companion in the catalog
        ShowCompanion(selectedID);
    }

    // Method to show the selected companion and hide the rest
    void ShowCompanion(int id)
    {
        // Hide all companion slots initially
        foreach (GameObject slot in companionSlots)
        {
            slot.SetActive(false);
        }

        // Display the corresponding companion based on the ID and update the image
        switch (id)
        {
            case ALIEN_ID:
                companionSlots[ALIEN_ID].SetActive(true);
                companionSlots[ALIEN_ID].GetComponent<Image>().sprite = option1Image;
                break;
            case BERRY_ID:
                companionSlots[BERRY_ID].SetActive(true);
                companionSlots[BERRY_ID].GetComponent<Image>().sprite = option2Image;
                break;
            case GREY_ID:
                companionSlots[GREY_ID].SetActive(true);
                companionSlots[GREY_ID].GetComponent<Image>().sprite = option3Image;
                break;
            case WOSHI_ID:
                companionSlots[WOSHI_ID].SetActive(true);
                companionSlots[WOSHI_ID].GetComponent<Image>().sprite = option4Image;
                break;
            default:
                Debug.LogWarning("Invalid Companion ID or no companion selected.");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // You can add any updates here if needed
    }
}
