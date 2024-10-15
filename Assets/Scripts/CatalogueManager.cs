using UnityEngine;
using UnityEngine.UI;

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
    }

    void PopulateCatalog()
    {
        // Clear existing slots if any (in case this method is called multiple times)
        foreach (Transform child in catalogContentParent)
        {
            Destroy(child.gameObject);
        }

        // Loop through all companions and create catalog slots
        foreach (var companion in companionManager.companions)
        {
            // Create a new GameObject for the slot
            GameObject slot = new GameObject(companion.PetName, typeof(RectTransform), typeof(Image));

            // Set parent to the catalog content
            slot.transform.SetParent(catalogContentParent);

            // Set up the RectTransform
            RectTransform rectTransform = slot.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100, 100); // Adjust the size as needed
            rectTransform.localScale = Vector3.one;

            // Set up the Image component and assign the sprite
            Image slotImage = slot.GetComponent<Image>();
            slotImage.sprite = companion.CompanionSprite; // Access the sprite directly from the Companion instance

            // Optional: Add a border for owned companions
            if (companion.IsBought)
            {
                GameObject border = new GameObject("Border", typeof(RectTransform), typeof(Image));
                border.transform.SetParent(slot.transform);
                border.GetComponent<Image>().color = Color.green; // Customize border appearance
                
                // Set up the border RectTransform
                RectTransform borderRectTransform = border.GetComponent<RectTransform>();
                borderRectTransform.sizeDelta = new Vector2(110, 110); // Slightly larger than the slot
                borderRectTransform.localScale = Vector3.one;
            }
        }
    }
}
