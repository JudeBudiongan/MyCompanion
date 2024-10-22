/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CatalogueManager : MonoBehaviour
{
    // Reference to CompanionManager to get the list of companions
    private CompanionManager companionManager;

    // UI elements for catalog
    public Transform catalogContentParent; // Parent GameObject to hold instantiated slots (like a ScrollView content)

    void Start()
    {
        // Get the CompanionManager instance
        companionManager = CompanionManager.Instance;

        // Populate the catalog based on the companions from CompanionManager
        PopulateCatalog();

        // Subscribe to companion added event
        if (companionManager != null)
        {
            companionManager.OnCompanionAdded += UpdateCatalogueUI;
        }
    }

        public void PopulateCatalog()
        {
            // Clear existing slots if any (in case this method is called multiple times)
            foreach (Transform child in catalogContentParent)
            {
                Destroy(child.gameObject);
            }

            // Loop through all companions and create catalog slots
            foreach (var companion in companionManager.companions)
            {
                Debug.Log("Processing companion: " + companion.PetName + ", IsBought: " + companion.IsBought);
                
                // Check if the companion is bought or if it's a starter companion that has been selected
                if (companion.IsBought || IsStarterSelected(companion.CompanionID))
                {
                    // Create a new GameObject for the slot
                    GameObject slot = new GameObject(companion.PetName, typeof(RectTransform), typeof(Image), typeof(Button));

                    // Set parent to the catalog content
                    slot.transform.SetParent(catalogContentParent);

                    // Set up the RectTransform
                    RectTransform rectTransform = slot.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(100, 100); // Adjust the size as needed
                    rectTransform.localScale = Vector3.one;

                    // Set up the Image component and assign the sprite
                    Image slotImage = slot.GetComponent<Image>();
                    slotImage.sprite = companion.CompanionSprite; // Access the sprite directly from the Companion instance

                    // Add a Button component to make the slot interactive
                    Button slotButton = slot.GetComponent<Button>();

                    // Assign a listener to the button (e.g., to load the main menu when pressed)
                    slotButton.onClick.AddListener(() => OnCompanionSelected(companion.CompanionID));
                }
            }
        }



    // Helper method to check if a starter companion is selected
    private bool IsStarterSelected(int companionID)
    {
        int selectedID = PlayerPrefs.GetInt("SelectedID", -1); // Default to -1 if no starter has been selected
        return companionID == selectedID;
    }

    private void OnCompanionSelected(int companionID)
    {
        Debug.Log("Companion with ID " + companionID + " selected.");
        GameManager.Instance.SetSelectedCompanionID(companionID);
        PlayerPrefs.SetInt("SelectedID", companionID);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Main Menu");
    }

    // New method to refresh or update the catalog UI
    public void UpdateCatalogueUI()
    {
        PopulateCatalog();

        foreach (var companion in companionManager.companions)
{
    Debug.Log("Processing companion: " + companion.PetName + ", Bought Status: " + companion.IsBought);
    if (companion.IsBought || IsStarterSelected(companion.CompanionID))
    {
        // Create a new GameObject for the slot as usual
    }
}

    }

    // New method to count the number of bought companions
    public int GetBoughtCompanionCount()
    {
        int boughtCount = 0;
        foreach (var companion in companionManager.companions)
        {
            if (companion.IsBought)
            {
                boughtCount++;
            }
        }
        return boughtCount;
    }

    void OnDestroy()
    {
        // Unsubscribe from event to prevent memory leaks
        if (companionManager != null)
        {
            companionManager.OnCompanionAdded -= UpdateCatalogueUI;
        }
    }
}*/
