using UnityEngine;
using UnityEngine.UI;

public class CompanionSlot : MonoBehaviour
{
    public Image companionImage;
    public Text companionNameText;
    public GameObject border; // Optional: highlight or border object

    public void SetupSlot(CompanionManager.Companion companion)
    {
        // Set companion data in the slot
        companionNameText.text = companion.PetName;

        // Assuming you have a way to get the sprite by name or ID, you can assign it here
        companionImage.sprite = GetCompanionSprite(companion.CompanionID);

        // Show or hide border based on some condition, e.g., if the companion is selected
        border.SetActive(companion.IsBought);
    }

    private Sprite GetCompanionSprite(int companionID)
    {
        // Logic to get the correct sprite, e.g., switch case or a dictionary of sprites
        // This is where you should use a similar approach as CompanionManager's sprites
        switch (companionID)
        {
            case 0:
                return Resources.Load<Sprite>("AlienSprite"); // Example path
            case 1:
                return Resources.Load<Sprite>("BerrySprite");
            case 2:
                return Resources.Load<Sprite>("GreySprite");
            case 3:
                return Resources.Load<Sprite>("WoshiSprite");
            // Add cases for other sprites as needed
            default:
                return null;
        }
    }
}
